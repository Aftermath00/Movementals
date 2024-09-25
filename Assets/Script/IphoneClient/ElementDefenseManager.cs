using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ElementDefenseManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI currentElementLabel;
    public Image elementEmojiImage;
    public TextMeshProUGUI elementNameText;
    public TextMeshProUGUI elementLanguageText;
    public TextMeshProUGUI elementPhraseText;
    public TextMeshProUGUI transcribedWordsLabel;
    public TextMeshProUGUI transcribedWordsText;
    public TextMeshProUGUI recognizedElementLabel;
    public TextMeshProUGUI recognizedElementText;
    public TextMeshProUGUI listeningText;

    private ElementPlugin elementPlugin;

    void Start()
    {
        elementPlugin = FindObjectOfType<ElementPlugin>();
        SetupInitialUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void SetupInitialUI()
    {
        titleText.text = "Element Defense";
        currentElementLabel.text = "Current Element:";
        transcribedWordsLabel.text = "Transcribed Words:";
        recognizedElementLabel.text = "Recognized Element:";
        
        transcribedWordsText.text = "Waiting for speech...";
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
            elementLanguageText.text = "Language: " + GetLanguageForElement(currentElement);
            elementPhraseText.text = "Say: " + GetPhraseForElement(currentElement);
        }
        else
        {
            elementNameText.text = "No element detected";
            elementLanguageText.text = "Language: -";
            elementPhraseText.text = "Say: -";
        }

        string recognizedText = elementPlugin.GetRecognizedText();
        if (!string.IsNullOrEmpty(recognizedText))
        {
            transcribedWordsText.text = recognizedText;
        }

        string recognizedElement = elementPlugin.GetRecognizedElement();
        if (!string.IsNullOrEmpty(recognizedElement))
        {
            recognizedElementText.text = recognizedElement;
        }

        listeningText.text = elementPlugin.IsListening() ? "Listening..." : "";
        listeningText.color = elementPlugin.IsListening() ? Color.green : Color.clear;
    }

    private string GetLanguageForElement(string element)
    {
        switch (element.ToLower())
        {
            case "water": return "Japanese";
            case "fire": return "Arabic";
            case "wind": return "Italian";
            case "rock": return "German";
            default: return "Unknown";
        }
    }

    private string GetPhraseForElement(string element)
    {
        switch (element.ToLower())
        {
            case "water": return "水の守り";
            case "fire": return "لهب النار";
            case "wind": return "vento rapido";
            case "rock": return "felsenkraft";
            default: return "";
        }
    }

    public void ActivateDefenseMode()
    {
        elementPlugin.SetDefenseState();
        gameObject.SetActive(true);
    }

    public void DeactivateDefenseMode()
    {
        gameObject.SetActive(false);
    }
}
