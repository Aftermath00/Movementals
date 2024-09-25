// using UnityEngine;
// using UnityEngine.UI;
// using Unity.Netcode;
// using Unity.Netcode.Transports.UTP;
// using Unity.Networking.Transport.Relay;
// using System.Collections;
// using TMPro;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
// using Unity.Services.Relay;
// using Unity.Services.Relay.Models;
// using System.Collections.Generic;
// using System.Linq;

// public class RelayManager : MonoBehaviour
// {
//     [SerializeField] Button hostButton;
//     [SerializeField] Button startGameButton;
//     [SerializeField] TextMeshProUGUI codeText;
//     [SerializeField] TextMeshProUGUI playerCountText;
//     [SerializeField] TextMeshProUGUI[] playerStatusTexts;
//     [SerializeField] Image[] characterImages;
//     [SerializeField] Sprite[] characterSprites;

//     private const int maxPlayers = 4;
//     private int currentPlayerCount = 0;
//     private Dictionary<ulong, int> playerCharacterMap = new Dictionary<ulong, int>();
//     private List<int> availableCharacterIndices;

//     async void Start()
//     {
//         await UnityServices.InitializeAsync();
//         await AuthenticationService.Instance.SignInAnonymouslyAsync();

//         hostButton.onClick.AddListener(CreateRelay);
//         startGameButton.interactable = false;

//         NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
//         NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

//         InitializeAvailableCharacters();
//     }

//     private void InitializeAvailableCharacters()
//     {
//         availableCharacterIndices = Enumerable.Range(0, characterSprites.Length).ToList();
//     }

//     async void CreateRelay()
//     {
//         try
//         {
//             Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
//             string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

//             RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
//             NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
//             NetworkManager.Singleton.StartHost();

//             codeText.text = $"Room Code: {joinCode}";

//             // Don't assign a character to the host
//             UpdatePlayerCount();
//         }
//         catch (RelayServiceException e)
//         {
//             Debug.LogError($"Relay create failed: {e.Message}");
//         }
//     }

//     private void OnClientConnected(ulong clientId)
//     {
//         if (NetworkManager.Singleton.IsServer && clientId != NetworkManager.Singleton.LocalClientId)
//         {
//             AssignCharacter(clientId);
//             UpdatePlayerCount();
//         }
//     }

//     private void OnClientDisconnected(ulong clientId)
//     {
//         if (NetworkManager.Singleton.IsServer && clientId != NetworkManager.Singleton.LocalClientId)
//         {
//             if (playerCharacterMap.TryGetValue(clientId, out int characterIndex))
//             {
//                 availableCharacterIndices.Add(characterIndex);
//             }
//             playerCharacterMap.Remove(clientId);
//             UpdatePlayerCount();
//         }
//     }

//     private void UpdatePlayerCount()
//     {
//         currentPlayerCount = NetworkManager.Singleton.ConnectedClientsIds.Count - 1; // Exclude host
//         playerCountText.text = $"Players Connected: {currentPlayerCount}/{maxPlayers}";

//         for (int i = 0; i < maxPlayers; i++)
//         {
//             if (i < currentPlayerCount)
//             {
//                 playerStatusTexts[i].text = "Connected";
//                 ulong clientId = NetworkManager.Singleton.ConnectedClientsIds
//                     .Where(id => id != NetworkManager.Singleton.LocalClientId)
//                     .ElementAt(i);

//                 if (playerCharacterMap.TryGetValue(clientId, out int characterIndex))
//                 {
//                     characterImages[i].sprite = characterSprites[characterIndex];
//                 }
//             }
//             else
//             {
//                 playerStatusTexts[i].text = "Not Connected";
//                 int randomCharacterIndex = GetRandomAvailableCharacterIndex();
//                 characterImages[i].sprite = characterSprites[randomCharacterIndex];
//             }
//         }

//         startGameButton.interactable = (currentPlayerCount == maxPlayers);
//     }

//     private void AssignCharacter(ulong clientId)
//     {
//         if (!playerCharacterMap.ContainsKey(clientId))
//         {
//             int characterIndex = GetRandomAvailableCharacterIndex();
//             playerCharacterMap[clientId] = characterIndex;
//             AssignCharacterClientRpc(clientId, characterIndex);
//         }
//     }

//     private int GetRandomAvailableCharacterIndex()
//     {
//         if (availableCharacterIndices.Count == 0)
//         {
//             InitializeAvailableCharacters();
//         }

//         int index = Random.Range(0, availableCharacterIndices.Count);
//         int characterIndex = availableCharacterIndices[index];
//         availableCharacterIndices.RemoveAt(index);
//         return characterIndex;
//     }

//     [ClientRpc]
//     private void AssignCharacterClientRpc(ulong clientId, int characterIndex)
//     {
//         if (NetworkManager.Singleton.LocalClientId == clientId)
//         {
//             Debug.Log($"Assigned character {characterIndex} to client {clientId}");
//         }
//     }

//     void Update()
//     {
//         if (NetworkManager.Singleton.IsServer)
//         {
//             int connectedPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count - 1; // Exclude host
//             if (connectedPlayers != currentPlayerCount)
//             {
//                 UpdatePlayerCount();
//             }
//         }
//     }
// }


using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Collections;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Collections.Generic;
using System.Linq;

public class RelayManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI[] playerStatusTexts;
    [SerializeField] private Image[] characterImages;
    [SerializeField] private Sprite[] characterSprites;
    [SerializeField] private Canvas lobby;
    [SerializeField] private GameManager gameManager;

    private const int maxPlayers = 4;
    private int currentPlayerCount = 0;
    private Dictionary<ulong, int> playerCharacterMap = new Dictionary<ulong, int>();
    private List<int> availableCharacterIndices = new List<int> { 0, 1, 2, 3 };
    private const float HeartbeatInterval = 1f; // Send heartbeat every 1 second
    private float lastHeartbeatTime;
    private const float ConnectionCheckInterval = 5f;
    private float lastConnectionCheckTime;
    

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostButton.onClick.AddListener(CreateRelay);
        startGameButton.interactable = false;
        startGameButton.onClick.AddListener(StartGame);

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

        InitializeAvailableCharacters();
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void InitializeAvailableCharacters()
    {
        availableCharacterIndices = new List<int> { 0, 1, 2, 3 }; // Fire, Earth, Water, Wind
    }

    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            codeText.text = $"Room Code: {joinCode}";
            Debug.Log($"Relay created. Join code: {joinCode}");

            UpdatePlayerCount();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Failed to create relay: {e.Message}");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
        if (NetworkManager.Singleton.IsServer && clientId != NetworkManager.Singleton.LocalClientId)
        {
            if (!playerCharacterMap.ContainsKey(clientId))
            {
                Debug.Log($"Attempting to assign character to client {clientId}");
                AssignCharacter(clientId);
            }
            else
            {
                Debug.Log($"Character already assigned to client {clientId}");
            }
        }
        UpdatePlayerCount();
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.LogWarning($"Client {clientId} disconnected");
        if (NetworkManager.Singleton.IsServer)
        {
            HandleClientDisconnect(clientId);
        }

        if (NetworkManager.Singleton.IsServer && clientId != NetworkManager.Singleton.LocalClientId)
        {
            if (playerCharacterMap.TryGetValue(clientId, out int characterIndex))
            {
                availableCharacterIndices.Add(characterIndex);
                playerCharacterMap.Remove(clientId);
                Debug.Log($"Removed character assignment for client {clientId}");
                UpdateCharacterAssignmentsClientRpc();
            }
        }
        UpdatePlayerCount();
    }

    private void UpdatePlayerCount()
    {
        currentPlayerCount = NetworkManager.Singleton.ConnectedClientsIds.Count - 1; // Exclude host
        Debug.Log($"Connected clients (excluding host): {currentPlayerCount}");
        playerCountText.text = $"Players Connected: {currentPlayerCount}/{maxPlayers}";

        for (int i = 0; i < maxPlayers; i++)
        {
            if (i < currentPlayerCount)
            {
                playerStatusTexts[i].text = "Connected";
                ulong clientId = NetworkManager.Singleton.ConnectedClientsIds
                    .Where(id => id != NetworkManager.Singleton.LocalClientId)
                    .ElementAt(i);

                if (playerCharacterMap.TryGetValue(clientId, out int characterIndex))
                {
                    characterImages[i].sprite = characterSprites[characterIndex];
                }
            }
            else
            {
                playerStatusTexts[i].text = "Not Connected";
                characterImages[i].sprite = characterSprites[i];
            }
        }

        startGameButton.interactable = (currentPlayerCount == maxPlayers); // -1 because we exclude the host
    }

    private int GetAvailableCharacterIndex()
    {
        if (availableCharacterIndices.Count == 0)
        {
            Debug.LogError("No available character indices!");
            return -1;
        }

        int index = UnityEngine.Random.Range(0, availableCharacterIndices.Count);
        int characterIndex = availableCharacterIndices[index];
        availableCharacterIndices.RemoveAt(index);
        return characterIndex;
    }

    private void AssignCharacter(ulong clientId)
    {
        Debug.Log($"AssignCharacter called for client {clientId}");
        if (!playerCharacterMap.ContainsKey(clientId))
        {
            int characterIndex = GetAvailableCharacterIndex();
            playerCharacterMap[clientId] = characterIndex;
            Debug.Log($"Assigned character {characterIndex} to client {clientId}");
            AssignCharacterClientRpc(clientId, characterIndex);
            
            // Notify all clients about the new assignment
            UpdateCharacterAssignmentsClientRpc();
        }
        else
        {
            Debug.Log($"Character already assigned to client {clientId}: {playerCharacterMap[clientId]}");
        }
    }

    [ClientRpc]
    private void AssignCharacterClientRpc(ulong clientId, int characterIndex)
    {
        Debug.Log($"Client {clientId} received character assignment: {characterIndex}");
    }

    private void StartGame()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            StartGameServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc()
    {
        StartGameClientRpc();
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        lobby.gameObject.SetActive(false);

        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"Starting game. Connected clients: {NetworkManager.Singleton.ConnectedClientsIds.Count - 1}"); // Exclude host
            
            Debug.Log("playerCharacterMap contents:");
            foreach (var kvp in playerCharacterMap)
            {
                Debug.Log($"Client {kvp.Key}: Character {kvp.Value}");
            }

            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (clientId != NetworkManager.Singleton.LocalClientId) // Exclude host
                {
                    Debug.Log($"Processing client: {clientId}");
                    if (playerCharacterMap.TryGetValue(clientId, out int characterIndex))
                    {
                        Debug.Log($"Spawning player for client {clientId} with character index {characterIndex}");
                        gameManager.SpawnPlayerServerRpc(clientId, characterIndex);
                    }
                    else
                    {
                        Debug.LogWarning($"No character assigned for client {clientId}");
                    }
                }
            }
            
            gameManager.SpawnMonsterServerRpc();
            StartCoroutine(WaitForGameSceneManager());
        }
        Debug.Log("Game started!");
    }

    private IEnumerator WaitForGameSceneManager()
    {
        while (GameSceneManager.Instance == null)
        {
            yield return null;
        }

        GameSceneManager.Instance.RequestSceneChangeServerRpc();
    }

    [ClientRpc]
    private void SendHeartbeatClientRpc()
    {
        // Clients respond to the heartbeat
        if (!NetworkManager.Singleton.IsServer)
        {
            RespondToHeartbeatServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RespondToHeartbeatServerRpc(ServerRpcParams rpcParams = default)
    {
        // Server receives the heartbeat response
        Debug.Log($"Received heartbeat response from client {rpcParams.Receive.SenderClientId}");
    }

    private void CheckConnectionStatus()
    {
        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId)
            {
                CheckClientConnectionServerRpc(clientId);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CheckClientConnectionServerRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
        {
            Debug.Log($"Client {clientId} is still connected");
        }
        else
        {
            Debug.LogWarning($"Client {clientId} has disconnected");
            HandleClientDisconnect(clientId);
        }
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        // Remove the disconnected client from playerCharacterMap
        if (playerCharacterMap.ContainsKey(clientId))
        {
            playerCharacterMap.Remove(clientId);
        }

        // Update UI or game state as needed
        UpdatePlayerCount();

        // Optionally, you can try to reconnect the client
        AttemptReconnectClientRpc(clientId);
    }

    [ClientRpc]
    private void AttemptReconnectClientRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            StartCoroutine(ReconnectCoroutine());
        }
    }

    private IEnumerator ReconnectCoroutine()
    {
        Debug.Log("Attempting to reconnect...");
        
        // Wait for a short time before attempting to reconnect
        yield return new WaitForSeconds(2f);

        // Attempt to reconnect
        NetworkManager.Singleton.Shutdown();
        yield return new WaitForSeconds(1f);
        
        // You might need to modify this part based on how you're initially connecting to the relay
        // For example, you might need to rejoin the relay using the original join code
        NetworkManager.Singleton.StartClient();
    }

    [ClientRpc]
    private void UpdateCharacterAssignmentsClientRpc()
    {
        foreach (var kvp in playerCharacterMap)
        {
            Debug.Log($"Client {kvp.Key}: Character {kvp.Value}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReceiveTriggeredAttackServerRpc(string element, ulong clientId)
    {
        Debug.Log($"Received triggered attack from client {clientId} for element: {element}");
        // Call the HandleTriggeredAttack method in the GameManager
        gameManager.HandleTriggeredAttack(element, clientId);
    }



    private void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            int connectedPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count - 1;  // Exclude host
            if (connectedPlayers != currentPlayerCount)
            {
                UpdatePlayerCount();
            }

            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (clientId != NetworkManager.Singleton.LocalClientId && !playerCharacterMap.ContainsKey(clientId))
                {
                    AssignCharacter(clientId);
                }
            }

            if (Time.time - lastHeartbeatTime > HeartbeatInterval)
            {
                SendHeartbeatClientRpc();
                lastHeartbeatTime = Time.time;
            }

            if (Time.time - lastConnectionCheckTime > ConnectionCheckInterval)
            {
                CheckConnectionStatus();
                lastConnectionCheckTime = Time.time;
            }
        }
    }

}
