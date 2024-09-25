using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public ElementDefenseManager defenseManager;
    public ElementAttackManager attackManager;
    public ElementPlugin elementPlugin;

    private void Start()
    {
        elementPlugin.StartCameraCapture();
        SwitchToDefenseMode(); 
    }

    public void SwitchToDefenseMode()
    {
        defenseManager.gameObject.SetActive(true);
        attackManager.gameObject.SetActive(false);
        elementPlugin.SetDefenseState();
    }

    public void SwitchToAttackMode()
    {
        defenseManager.gameObject.SetActive(false);
        attackManager.gameObject.SetActive(true);
        elementPlugin.SetAttackState();
    }


    private void OnDisable()
    {
        elementPlugin.StopCameraCapture();
    }
}

