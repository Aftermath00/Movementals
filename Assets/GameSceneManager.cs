using UnityEngine;
using Unity.Netcode;

public class GameSceneManager : NetworkBehaviour
{
    public static GameSceneManager Instance;

    [SerializeField] private string RecognitionSceneName = "RecognitionScene";
    [SerializeField] private string LobbyScene = "LobbyScene";

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameSceneManager instance initialized");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate GameSceneManager instance destroyed");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestSceneChangeServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        ChangeSceneClientRpc(clientId);
    }

    [ClientRpc]
    private void ChangeSceneClientRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(RecognitionSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestReturnToMainGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        UnityEngine.SceneManagement.SceneManager.LoadScene(LobbyScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void ActivateAttackMode()
    {
        if (!IsServer)
        {
            RequestSceneChangeServerRpc();
        }
    }
}
