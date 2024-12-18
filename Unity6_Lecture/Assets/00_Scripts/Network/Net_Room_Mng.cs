using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using UnityEngine;

public partial class Net_Mng : MonoBehaviour
{
    public async void JoinGameWithCode(string inputJoinCode)
    {
        if (string.IsNullOrEmpty(inputJoinCode))
        {
            Debug.Log("��ȿ���� ���� �ڵ��Դϴ�.");
            return;
        }
        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(inputJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
                );
            StartClient();
            Debug.Log("Join code�� ���� ���� ����");
        }
        catch (RelayServiceException e)
        {
            Debug.Log("���� ���� ���� : "+e);
        }
    }
    public async void StartMatchMaking()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("�α��� ���� �ʾҽ��ϴ�.");
            return;
        }
        currentlobby = await FindAvailableLobby();

        if (currentlobby == null)
        {
            await CreateNewLobby();
        }
        else
        {
            await JoinLobby(currentlobby.Id);
        }
    }

    private async Task<Lobby> FindAvailableLobby()
    {
        try
        {
            var querryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            if (querryResponse.Results.Count > 0)
            {
                return querryResponse.Results[0];
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("�κ� ã�� ����" + e);
        }
        return null;
    }
    private async Task CreateNewLobby()
    {
        try
        {
            currentlobby = await LobbyService.Instance.CreateLobbyAsync("������Ī��", maxPlayers);
            Debug.Log("���ο� �� ������:" + currentlobby.Id);
            await AllocateRelayServerAndJoin(currentlobby);
            StartHost();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("�κ� ���� ����." + e);
        }
    }
    private async Task JoinLobby(string lobbyId)
    {
        try
        {
            currentlobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            Debug.Log("�濡 ���ӵǾ����ϴ�." + currentlobby.Id);
            StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("�κ� ���� ���� : " + e);
        }
    }
    private async Task AllocateRelayServerAndJoin(Lobby lobby)
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(lobby.MaxPlayers);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            JoinCodeText.text = joinCode;
            Debug.Log("Relay ���� �Ҵ� �Ϸ�. Join Code : " + joinCode);
        }
        catch (RelayServiceException e)
        {
            Debug.Log("Relay ���� �Ҵ� ����" + e);
        }
    }
    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("ȣ��Ʈ�� ���۵Ǿ����ϴ�.");
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnHostDisconnected;
    }
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Ŭ���̾�Ʈ�� ���۵Ǿ����ϴ�.");
     
    }
    private void OnClientConnected(ulong clientId)
    {
        OnPlayerJoined();
    }

    private void OnHostDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId && NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnHostDisconnected;
        }
    }
}
