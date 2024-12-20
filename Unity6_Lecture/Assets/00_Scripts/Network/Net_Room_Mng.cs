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
            Debug.Log("유효하지 않은 코드입니다.");
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
            Debug.Log("Join code로 게임 접속 성공");
        }
        catch (RelayServiceException e)
        {
            Debug.Log("게임 접속 실패 : "+e);
        }
    }
    public async void StartMatchMaking()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("로그인 되지 않았습니다.");
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
            Debug.Log("로비 찾기 실패" + e);
        }
        return null;
    }
    private async Task CreateNewLobby()
    {
        try
        {
            currentlobby = await LobbyService.Instance.CreateLobbyAsync("랜덤매칭방", maxPlayers);
            Debug.Log("새로운 방 생성됨:" + currentlobby.Id);
            await AllocateRelayServerAndJoin(currentlobby);
            StartHost();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("로비 생성 실패." + e);
        }
    }
    private async Task JoinLobby(string lobbyId)
    {
        try
        {
            currentlobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            Debug.Log("방에 접속되었습니다." + currentlobby.Id);
            StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("로비 참가 실패 : " + e);
        }
    }
    private async Task AllocateRelayServerAndJoin(Lobby lobby)
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(lobby.MaxPlayers);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            JoinCodeText.text = joinCode;
            Debug.Log("Relay 서버 할당 완료. Join Code : " + joinCode);
        }
        catch (RelayServiceException e)
        {
            Debug.Log("Relay 서버 할당 실패" + e);
        }
    }
    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("호스트가 시작되었습니다.");
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnHostDisconnected;
    }
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("클라이언트가 시작되었습니다.");
     
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
