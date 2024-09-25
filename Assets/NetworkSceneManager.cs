using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class NetworkSceneManager : NetworkBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        Debug.Log($"Scene {sceneName} loaded. Clients completed: {clientsCompleted.Count}, Clients timed out: {clientsTimedOut.Count}");
        
        // You can add additional logic here, such as initializing scene-specific objects
    }

    // Keep OnDestroy public
    public override void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
        }
        base.OnDestroy(); // Call the base class OnDestroy
    }
}
