using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class GameSceneManager : NetworkBehaviour
{
    public static GameSceneManager Instance;

    [SerializeField] private string RecognitionSceneName = "RecognitionScene";

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
        ChangeSceneClientRpc();
    }

    [ClientRpc]
    private void ChangeSceneClientRpc()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(RecognitionSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        
        // Wait until the scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
