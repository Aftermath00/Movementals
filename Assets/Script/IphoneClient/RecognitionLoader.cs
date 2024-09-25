using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class SimpleSceneLoader : NetworkBehaviour
{
    [SerializeField] private Button loadSceneButton;
    [SerializeField] private string sceneToLoad = "RecognitionScene";

    private void Start()
    {
        if (loadSceneButton != null)
        {
            loadSceneButton.onClick.AddListener(() => RequestLoadSceneServerRpc());
        }
        else
        {
            Debug.LogError("Load Scene button not assigned in SimpleSceneLoader");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestLoadSceneServerRpc()
    {
        Debug.Log($"Server received request to load scene: {sceneToLoad}");
        NetworkManager.Singleton.SceneManager.LoadScene(sceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
