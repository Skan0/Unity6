using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
public partial class Net_Mng : MonoBehaviour
{
    // Lobby -> �÷��̾ ���ϴ� ������ ã�ų� �� ������ ����� ����� �� �ִ� 
    // Relay -> ��Ī�� �÷��̾���� Relay�� Join Code�� ����Ǿ�, ȣ��Ʈ-Ŭ���̾�Ʈ ������� �ǽð� ��Ƽ�÷��� ȯ���� ����
    // -> ������Ī�� ������ �ִ� ���� �θ��� �˾Ƽ� �� �濡 �־��ְ� ���� �÷����ϰ� ������ִ°�
    private Lobby currentlobby;

    private const int maxPlayers = 2;
    public string gamePlaySceneName = "GamePlayScene";
    public Button StartMatchButton, JoinMatchButton;
    public InputField input;
    public Text JoinCodeText;
    // �񵿱� -> ���ÿ� �Ͼ�� �ʴ´�.
    // web�� ��� ��û�� ������ �� delay�� �ִµ� �̷��� ��Ⱑ �ִ� �۾��� �񵿱� �۾��̴�.
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        StartMatchButton.onClick.AddListener(() => StartMatchMaking());
        JoinMatchButton.onClick.AddListener(() => JoinGameWithCode(input.text));
    }

}
