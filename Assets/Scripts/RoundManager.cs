using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public struct Wave
{
    public string name;
    public EnemyType type;
    public int num;
    public int hp;
    public float moveSpeed;
    public float attackPower;
    public float attackSpeed;
    public float waveTime;
    public int gold;
}

[System.Serializable]
public struct Round{
    public string name;
    public Wave[] wave;
    public int gold;
}

public class RoundManager : MonoBehaviour {
    public static RoundManager instance;

    public GameObject shop;

    public Round[] round;

    private int _leftUnit = 0;

    public int leftUnit
    {
        get
        {
            return _leftUnit;
        }
    }

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : RoundManager");
        }
        instance = this;
    }

    void Start() {
        Debug.Log("ROUND MANAGER IS READY");

        StartCoroutine(Round(round[GameManager.instance.round]));

        TextManager.instance.ViewLeftUnit();
    }

    private IEnumerator Round(Round round)
    {
        TextManager.instance.ViewRound();
        for (int i = 0; i < round.wave.Length; i++)
        {
            EnemyManager.instance.SetEnemy(round.wave[i].type, round.wave[i].num);
        }

        yield return StartCoroutine(TextManager.instance.ViewMessage(round.name));

        for (int i = 0; i < round.wave.Length; i++)
        {
            yield return StartCoroutine(Wave(round.wave[i]));
        }

        //EnemyManager.instance.ClearEnemy();
    }

    private IEnumerator Wave(Wave wave)
    {
        for (int i = 0; i < wave.num; i++)
        {
            EnemyManager.instance.CreateEnemy(
                wave.type, // type
                wave.name, // name
                wave.hp, // hp
                wave.moveSpeed, // moveSpeed
                wave.attackPower, // attackPower
                wave.attackSpeed, // attackSpeed
                wave.gold
            );
            _leftUnit++;
            TextManager.instance.ViewLeftUnit();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(wave.waveTime);
    }

    public void DeathUnit()
    {
        _leftUnit--;
        TextManager.instance.ViewLeftUnit();
        if (_leftUnit == 0) RoundClear();
    }

    private void RoundClear()
    {
        StartCoroutine(TextManager.instance.ViewMessage("시련 클리어"));
        GameManager.instance.TakeMoney(round[GameManager.instance.round].gold);
        GameManager.instance.RoundClear();
        StartCoroutine(OpenShop());
    }

    private IEnumerator OpenShop()
    {
        yield return new WaitForSeconds(4f);
        shop.SetActive(true);
        shop.SetActive(false);
        StartCoroutine(Round(round[GameManager.instance.round]));
    }
}
