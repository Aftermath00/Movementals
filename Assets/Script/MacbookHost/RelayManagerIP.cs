

// using UnityEngine;
// using UnityEngine.UI;
// using Unity.Netcode;
// using Unity.Netcode.Transports.UTP;
// using Unity.Networking.Transport.Relay;
// using TMPro;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
// using Unity.Services.Relay;
// using Unity.Services.Relay.Models;

// public class RelayManagerIP : MonoBehaviour
// {
//     [SerializeField] Button joinButton;
//     [SerializeField] TMP_InputField joinInput;

//     async void Start()
//     {
//         await UnityServices.InitializeAsync();
//         await AuthenticationService.Instance.SignInAnonymouslyAsync();

//         joinButton.onClick.AddListener(() => JoinRelay(joinInput.text));
//     }

//     async void JoinRelay(string joinCode)
//     {
//         try
//         {
//             var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
//             var relayServerData = new RelayServerData(joinAllocation, "dtls");
//             NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

//             NetworkManager.Singleton.StartClient();
//             Debug.Log("Player joined");
//         }
//         catch (RelayServiceException e)
//         {
//             Debug.LogError($"Relay join failed: {e.Message}");
//         }
//     }
// }

// using UnityEngine;
// using UnityEngine.UI;
// using Unity.Netcode;
// using Unity.Netcode.Transports.UTP;
// using Unity.Networking.Transport.Relay;
// using TMPro;
// using Unity.Services.Relay;
// using Unity.Services.Relay.Models;
// using Unity.Services.Core;
// using Unity.Services.Authentication;
// using System.Threading.Tasks;

// public class RelayManagerIP : MonoBehaviour
// {
//     [SerializeField] private Button joinButton;
//     [SerializeField] private TMP_InputField roomCodeInput;
//     [SerializeField] private TextMeshProUGUI statusText;
//     [SerializeField] private GameObject joinUI;
//     [SerializeField] private GameObject waitingUI;

//     private async void Start()
//     {
//         await UnityServices.InitializeAsync();
//         await AuthenticationService.Instance.SignInAnonymouslyAsync();

//         joinButton.onClick.AddListener(JoinRelay);
//     }

//     private async void JoinRelay()
//     {
//         string joinCode = roomCodeInput.text;

//         try
//         {
//             JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

//             RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
//             NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

//             NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
//             NetworkManager.Singleton.StartClient();
//             statusText.text = "Joining Room...";
//         }
//         catch (RelayServiceException e)
//         {
//             Debug.LogError($"Failed to join relay: {e.Message}");
//             statusText.text = "Failed to join room!";
//         }
//     }

//     private void OnClientConnected(ulong clientId)
//     {
//         if (clientId == NetworkManager.Singleton.LocalClientId)
//         {
//             statusText.text = "Connected to game!";
//             joinUI.SetActive(false);
//             waitingUI.SetActive(true);
//         }
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using TMPro;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class RelayManagerIP : MonoBehaviour
{
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField roomCodeInput;
    [SerializeField] private TextMeshProUGUI statusText;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        joinButton.onClick.AddListener(JoinRelay);
    }

    private async void JoinRelay()
    {
        string joinCode = roomCodeInput.text;

        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.StartClient();
            statusText.text = "Joining Room...";
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Failed to join relay: {e.Message}");
            statusText.text = "Failed to join room!";
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            statusText.text = "Connected to game! Waiting for host to start...";
        }
    }
}
