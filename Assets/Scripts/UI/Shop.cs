using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviourC {

    public Text text;
    private bool isUsed = false;

    public void OpenShop()
    {
        isUsed = true;
    }

    public void ExitShop()
    {
        isUsed = false;
        StartCoroutine(ViewManager.instance.BlurOff());
        RoundManager.instance.StartNextRound();
    }

    public void ClickUpgrade()
    {
        GameManager.instance.PowerUp();
    }
}
