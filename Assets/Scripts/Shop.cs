using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviourC {

    public Text text;
    private bool isUsed = false;

    public void OpenShop()
    {
        isUsed = true;
        gameObject.SetActive(true);
    }

    public IEnumerator WaitClose()
    {
        while (isUsed)
        {
            yield return null;
        }

        CloseShop();
    }

    private void CloseShop()
    {
        gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        isUsed = false;
        StartCoroutine(ViewManager.instance.BlurOff());
    }

    public void ClickUpgrade()
    {
        GameManager.instance.PowerUp();
    }
}
