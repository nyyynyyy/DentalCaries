using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviour {

    public Text text;
    private bool isUsed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        WaitInput();
	}

    private void WaitInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isUsed = false;
            StartCoroutine(ViewManager.instance.BlurOff());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A");
            GameManager.instance.PowerUp();
        }
    }

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

}
