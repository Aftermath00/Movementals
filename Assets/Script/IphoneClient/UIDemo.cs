// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Unity.Netcode;
// public class UIDemo : MonoBehaviour
// {
//  public InputField roomCode;
//  public static string ValueToKeep;
//  public void DemoButton(){
//     string dataToKeep = roomCode.text;
//     ValueToKeep = dataToKeep;
//     Debug.Log(roomCode.text);
//     NetworkManager.Singleton.StartClient();
//  }

// }

// using Unity.Netcode;
// using UnityEngine;
// using UnityEngine.UI;

// public class UIDemo : NetworkBehaviour
// {
//     // public InputField roomCode;
//     // public static string ValueToKeep;
//     private const string StaticMessage = "Hello from iPhone!";
//     void Start()
//     {
//         NetworkManager.Singleton.StartClient();
 
//     }

//     // Fungsi untuk mengirim string ke server
//     public void SendStringToServer()
//     {
//          if (IsClient)
//         {
//             SendMessageServerRpc(StaticMessage);
//         }
//         // var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
//         // StringServerRpc(messageBytes);
//     }

//     // [ServerRpc]
//     // void StringServerRpc(byte[] messageBytes)
//     // {
//     //     string message = System.Text.Encoding.UTF8.GetString(messageBytes);
//     //     DisplayMessageClientRpc(message); // Mengirim ke semua client dan host
//     //    //HostReceiveString.DisplayMessageClientRpc(message);
//     // }
//     // [ClientRpc]
//     // void DisplayMessageClientRpc(string message)
//     // {
//     //     Debug.Log("Message received: " + message);
//     // }

//     [ServerRpc(RequireOwnership = false)]
//     void SendMessageServerRpc(string message)
//     {
//         HostReceiveString hostManager = NetworkManager.Singleton.ConnectedClients[NetworkManager.ServerClientId]
//             .PlayerObject.GetComponent<HostReceiveString>();
//         hostManager.ReceiveStringClientRpc(message);
//     }

// }
