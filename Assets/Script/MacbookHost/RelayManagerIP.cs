// using UnityEngine;
// using UnityEngine.UI;
// using Unity.Netcode;
// using Unity.Netcode.Transports.UTP;
// using Unity.Networking.Transport.Relay;
// using System.Collections;
// using System.Collections.Generic;
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
//         var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
//         var relayServerData = new RelayServerData(joinAllocation, "dtls");
//         NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

//         NetworkManager.Singleton.StartClient();
//          Debug.Log("PLAYER CREATED");


//     }
// }

using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class RelayManagerIP : MonoBehaviour
{
    [SerializeField] Button joinButton;
    [SerializeField] TMP_InputField joinInput;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        joinButton.onClick.AddListener(() => JoinRelay(joinInput.text));
    }

    async void JoinRelay(string joinCode)
    {
        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            Debug.Log("Player joined");
        }
        catch (RelayServiceException e)
        {
            Debug.LogError($"Relay join failed: {e.Message}");
        }
    }
}
