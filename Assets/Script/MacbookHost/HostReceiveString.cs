using Unity.Netcode;
using UnityEngine;
using TMPro; // Library untuk TextMeshPro

public class HostReceiveString : NetworkBehaviour
{
    public TextMeshProUGUI messageText; // Reference ke UI Text

    void Start()
    {
        NetworkManager.Singleton.StartHost();
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Server is ready to receive messages.");
           
        }
    }

    // [ClientRpc]
    // public void DisplayMessageClientRpc(string message)
    // {
    //     Debug.Log("Message from client: " + message);
    //     UpdateUI(message); // Update tampilan UI
    // }

    // void UpdateUI(string message)
    // {
    //     messageText.text = message; // Menampilkan pesan di UI
    // }

    [ClientRpc]
    public void ReceiveStringClientRpc(string message)
    {
        if (IsClient)
        {
            messageText.text = message;
            Debug.Log($"Received message: {message}");
        }
    }
}
