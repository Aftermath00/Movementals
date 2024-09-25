using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class ElementPlugin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void InitializeElementViewModel();

    [DllImport("__Internal")]
    private static extern void StartListening();

    [DllImport("__Internal")]
    private static extern void StopListening();

    [DllImport("__Internal")]
    private static extern IntPtr GetRecognizedTextNative();

    [DllImport("__Internal")]
    private static extern bool GetIsListening();

    [DllImport("__Internal")]
    private static extern IntPtr GetErrorMessagesNative();

    [DllImport("__Internal")]
    private static extern void StartCamera();

    [DllImport("__Internal")]
    private static extern void StopCamera();

    [DllImport("__Internal")]
    private static extern IntPtr GetCurrentElementName();

    [DllImport("__Internal")]
    private static extern IntPtr GetRecognizedElementName();

    [DllImport("__Internal")]
    private static extern void FreeString(IntPtr ptr);

    [DllImport("__Internal")]
    private static extern void SwitchToDefense();

    [DllImport("__Internal")]
    private static extern void SwitchToAttack();

    private void Awake()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        InitializeElementViewModel();
        #endif
    }

    public void StartListeningForSpeech()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        StartListening();
        #endif
    }

    public void StopListeningForSpeech()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        StopListening();
        #endif
    }

    public string GetRecognizedText()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        IntPtr ptr = GetRecognizedTextNative();
        if (ptr != IntPtr.Zero)
        {
            string result = Marshal.PtrToStringAnsi(ptr);
            FreeString(ptr);
            return result;
        }
        #endif
        return string.Empty;
    }

    public bool IsListening()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        return GetIsListening();
        #else
        return false;
        #endif
    }

    public string GetErrorMessages()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        IntPtr ptr = GetErrorMessagesNative();  // Changed this line
        if (ptr != IntPtr.Zero)
        {
            string result = Marshal.PtrToStringAnsi(ptr);
            FreeString(ptr);
            return result;
        }
        #endif
        return string.Empty;
    }

    public void StartCameraCapture()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        StartCamera();
        #endif
    }

    public void StopCameraCapture()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        StopCamera();
        #endif
    }

    public string GetCurrentElement()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        IntPtr ptr = GetCurrentElementName();
        if (ptr != IntPtr.Zero)
        {
            string result = Marshal.PtrToStringAnsi(ptr);
            FreeString(ptr);
            return result;
        }
        #endif
        return string.Empty;
    }

    public string GetRecognizedElement()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        IntPtr ptr = GetRecognizedElementName();
        if (ptr != IntPtr.Zero)
        {
            string result = Marshal.PtrToStringAnsi(ptr);
            FreeString(ptr);
            return result;
        }
        #endif
        return string.Empty;
    }

    public void SetDefenseState()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        SwitchToDefense();
        #endif
    }

    public void SetAttackState()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        SwitchToAttack();
        #endif
    }
}

