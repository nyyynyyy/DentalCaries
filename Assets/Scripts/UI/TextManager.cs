using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextManager : MonoBehaviour {

    public static TextManager instance;

    public Text hp;
    public Text gold;
    public Text attack;
    public Text round;
    public Text leftUnit;

    public Text message;

    private GameManager _gm;
    private RoundManager _rm;

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
        _gm = GameManager.instance;
        _rm = RoundManager.instance;

        StartCoroutine(WaitAllStart());
    }

    // Update is called once per frame
    void Update () {
	
	}

    private IEnumerator WaitAllStart()
    {
        yield return null;
        ResetUI();
    }

    public void ResetUI()
    {
        ViewHp();
        ViewGold();
        ViewAttack();
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
        hp.text = _gm.nowHp.ToString() + " / " + _gm.maxHp.ToString();
    }

    public void ViewGold()
    {
        gold.text = _gm.money.ToString();
    }

    public void ViewAttack()
    {
        attack.text = _gm.power.ToString();
    }

    public void ViewRound()
    {
        round.text = _rm._round[_gm.round].name.ToString();
    }

    public void ViewLeftUnit()
    {
       // leftUnit.text = _rm.leftPro.ToString() + "%";
    }
}
