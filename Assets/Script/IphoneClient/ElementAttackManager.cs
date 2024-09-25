using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class ElementAttackManager : NetworkBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI currentElementLabel;
    public Image elementEmojiImage;
    public TextMeshProUGUI elementNameText;
    public TextMeshProUGUI recognizedElementLabel;
    public TextMeshProUGUI recognizedElementText;

    private ElementPlugin elementPlugin;

    void Start()
    {
        elementPlugin = FindObjectOfType<ElementPlugin>();
        SetupInitialUI();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void SetupInitialUI()
    {
        titleText.text = "Element Attack";
        currentElementLabel.text = "Current Element:";
        recognizedElementLabel.text = "Recognized Element:";
        
        recognizedElementText.text = "No element recognized yet";
    }

    void UpdateUI()
    {
        string currentElement = elementPlugin.GetCurrentElement();
        if (!string.IsNullOrEmpty(currentElement))
        {
            elementNameText.text = currentElement;
            // You'd need to map element names to emoji sprites
            // elementEmojiImage.sprite = GetEmojiForElement(currentElement);
        }

        string recognizedElement = elementPlugin.GetRecognizedElement();
        if (!string.IsNullOrEmpty(recognizedElement))
        {
            recognizedElementText.text = recognizedElement;
            SendRecognizedElementToServerServerRpc(recognizedElement);
        }
    }

    public void ActivateAttackMode()
    {
        elementPlugin.SetAttackState();
        gameObject.SetActive(true);
    }

    public void DeactivateAttackMode()
    {
        gameObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendRecognizedElementToServerServerRpc(string element, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        Debug.Log($"Server received recognized element: {element} from client {clientId}");
        // Process the recognized element on the server
        ProcessRecognizedElement(element, clientId);
    }


    private void ProcessRecognizedElement(string element, ulong clientId)
    {
        // Implement your server-side logic here
        Debug.Log($"Processing recognized element: {element} from client {clientId}");

        // Trigger the attack on the server based on the recognized element and client ID
        TriggerAttackOnServer(element, clientId);
    }

    private void TriggerAttackOnServer(string element, ulong clientId)
    {
        Debug.Log($"Triggering attack on server for element: {element} from client {clientId}");

        // Send the triggered attack to the macOS side
        RelayManager relayManager = FindObjectOfType<RelayManager>();
        if (relayManager != null)
        {
            relayManager.ReceiveTriggeredAttackServerRpc(element, clientId);
        }
        else
        {
            Debug.LogError("RelayManager not found!");
        }

        // Notify all clients about the triggered attack
        NotifyClientsAboutTriggeredAttackClientRpc(element, clientId);
    }

    [ClientRpc]
    private void NotifyClientsAboutTriggeredAttackClientRpc(string element, ulong clientId)
    {
        Debug.Log($"Received triggered attack from server for element: {element} from client {clientId}");
        // Update client-side UI or game state based on the triggered attack
    }

}
