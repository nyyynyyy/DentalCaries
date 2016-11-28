using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextManager : MonoBehaviour {

    public static TextManager instance;

    public Text hp;
    public Text gold;
    public Text round;
    public Text leftUnit;

    public Text message;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : TextManager");
        }
        instance = this;
    }

    // Use this for initialization
    void Start () {
        Debug.Log("TEXT MANAGER IS READY");
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void ResetUI()
    {
        ViewHp();
        ViewGold();
        ViewRound();
        ViewLeftUnit();
    }

    public IEnumerator ViewMessage(string msg)
    {
        message.gameObject.SetActive(true);
        message.text = msg;
        yield return new WaitForSeconds(3f);
        message.gameObject.SetActive(false);
    }

    public void ViewHp()
    {
        hp.text = "HP: " + GameManager.instance.hp.ToString();
    }

    public void ViewGold()
    {
        gold.text = "GOLD: " + GameManager.instance.money.ToString();
    }

    public void ViewRound()
    {
        round.text = "ROUND : " + (GameManager.instance.round + 1).ToString();
    }

    public void ViewLeftUnit()
    {
        leftUnit.text = "LEFT : " + RoundManager.instance.leftUnit.ToString();
    }
}
