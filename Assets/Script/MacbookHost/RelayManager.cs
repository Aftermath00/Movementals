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

// public class RelayManager : MonoBehaviour
// {
//     [SerializeField] Button hostButton;
//     // [SerializeField] Button joinButton;
//     // [SerializeField] TMP_InputField joinInput;
//     [SerializeField] TextMeshProUGUI codeText;
//     [SerializeField] TextMeshProUGUI playerCountText;

//     private const int maxPlayers = 4;

//     async void Start()
//     {
//         await UnityServices.InitializeAsync();
//         await AuthenticationService.Instance.SignInAnonymouslyAsync();

//         hostButton.onClick.AddListener(CreateRelay);
//        // joinButton.onClick.AddListener(() => JoinRelay(joinInput.text));

//         // Update jumlah player setiap ada perubahan koneksi
//         NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
//         NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
//     }

//     async void CreateRelay()
//     {
//         try
//         {
//             // Relay allocation untuk maksimal 3 player (di luar host)
//             Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
//             string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
//             codeText.text = "Code: " + joinCode;

//             var relayServerData = new RelayServerData(allocation, "dtls");
//             NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

//             NetworkManager.Singleton.StartHost();
//         }
//         catch (RelayServiceException e)
//         {
//             Debug.LogError($"Relay create failed: {e.Message}");
//         }
//     }

//         // Update jumlah player saat ada player baru terhubung
//     private void OnClientConnected(ulong clientId)
//     {
//         StartCoroutine(UpdatePlayerCountWithDelay());
//     }

//     // Update jumlah player saat ada player terputus
//     private void OnClientDisconnected(ulong clientId)
//     {
//         playerCountText.text = "Players Connected: " + NetworkManager.Singleton.ConnectedClients.Count + "/" + maxPlayers;
//     }

//     // Coroutine untuk memberi waktu network sinkron
//     private IEnumerator UpdatePlayerCountWithDelay()
//     {
//         yield return new WaitForSeconds(0.5f); // Beri jeda waktu agar network sinkron
//         int connectedPlayers = NetworkManager.Singleton.ConnectedClients.Count;
//         playerCountText.text = "Players Connected: " + connectedPlayers + "/" + maxPlayers;

//         // Jika jumlah pemain mencapai batas maksimum, disable join
//         if (connectedPlayers >= maxPlayers)
//         {
//             Debug.Log("Maximum players connected, no more can join.");
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

public class RelayManager : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] TextMeshProUGUI codeText;
    [SerializeField] TextMeshProUGUI playerCountText;

    private const int maxPlayers = 4;
    private int currentPlayerCount = 0;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostButton.onClick.AddListener(CreateRelay);

        // Update jumlah player setiap ada perubahan koneksi, tetapi hanya pada server
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    async void CreateRelay()
    {
        try
        {
            // Relay allocation untuk maksimal 3 player (di luar host)
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            codeText.text = "Code: " + joinCode;

            var relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Relay create failed: {e.Message}");
        }
    }

    // Update jumlah player saat ada player baru terhubung, tapi hanya pada server
    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            UpdatePlayerCount(); // Update segera ketika ada client yang terhubung
        }
    }

    // Update jumlah player saat ada player terputus, tapi hanya pada server
    private void OnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            UpdatePlayerCount(); // Update segera ketika ada client yang terputus
        }
    }

    // Real-time update untuk jumlah player, hanya di server
    void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            int connectedPlayers = NetworkManager.Singleton.ConnectedClients.Count;

            // Hanya update UI jika ada perubahan jumlah player
            if (connectedPlayers != currentPlayerCount)
            {
                UpdatePlayerCount();
            }
        }
    }

    // Update jumlah player dan UI, hanya dijalankan di server
    private void UpdatePlayerCount()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            currentPlayerCount = NetworkManager.Singleton.ConnectedClients.Count;
            playerCountText.text = "Players Connected: " + currentPlayerCount + "/" + maxPlayers;

            // Jika jumlah pemain mencapai batas maksimum, log pesan
            if (currentPlayerCount >= maxPlayers)
            {
                Debug.Log("Maximum players connected, no more can join.");
            }
        }
    }
}

