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

    public Round[] round;

    private int _roundUnit = 1;
    private int _leftUnit = 0;


    public int leftPro
    {
        get
        {
            return 100 - (int)((float)_leftUnit / (float)_roundUnit * 100f);
        }
    }
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

        StartCoroutine(WaitAllStart());

       // TextManager.instance.ViewLeftUnit();
    }

    private IEnumerator WaitAllStart()
    {
        yield return null;
        StartCoroutine(Round(round[GameManager.instance.round]));

    }

    private IEnumerator Round(Round round)
    {

        yield return new WaitForSeconds(3f);

        TextManager.instance.ViewRound();

        _roundUnit = 0;

        for (int i = 0; i < round.wave.Length; i++)
        {
            EnemyManager.instance.SetEnemy(round.wave[i].type, round.wave[i].num);
            _roundUnit += round.wave[i].num;
        }

        _leftUnit = _roundUnit;

        StartCoroutine(ViewManager.instance.BlurOn());
        yield return StartCoroutine(TextManager.instance.ViewMessage(round.name));
        StartCoroutine(ViewManager.instance.BlurOff());

        for (int i = 0; i < round.wave.Length; i++)
        {
            yield return StartCoroutine(Wave(round.wave[i]));
        }

        EnemyManager.instance.ClearEnemy();
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
            
            TextManager.instance.ViewLeftUnit();
            yield return new WaitForSeconds(0.2f);
        }

        if (wave.waveTime < 0) // last wave
        {
            while(leftUnit > 0) // wait kill  all unit
            {
                yield return null; 
            }
        }
        else // exist next wave
        {
            yield return new WaitForSeconds(wave.waveTime); // start next wave
        }
    }

    public void DeathUnit()
    {
        _leftUnit--;
        TextManager.instance.ViewLeftUnit();
        if (_leftUnit == 0) StartCoroutine(RoundClear());
    }

    private IEnumerator RoundClear()
    {
        StartCoroutine(ViewManager.instance.BlurOn());
        yield return StartCoroutine(TextManager.instance.ViewMessage("시련 클리어"));
        GameManager.instance.TakeMoney(round[GameManager.instance.round].gold);
        GameManager.instance.RoundClear();
        ViewManager.instance.OpenShop();
    }

    public void StartNextRound()
    {
        StartCoroutine(Round(round[GameManager.instance.round]));
    }
}
