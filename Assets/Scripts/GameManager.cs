using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("UI")]
    public Text hpTxt;

    [Header("Game State")]
    public float a = 1f;

    [Header("Player State")]
    public float health = 1000;
    public float power = 1f;

    private int round = 1;
    private bool isGame = false;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : GameManger");
        }
        instance = this;
    }

    void Start () {
        Init();
        StartCoroutine(Round());
    }
	
	void Update () {
	
	}

    private IEnumerator Round()
    {
        EnemyManager.instance.CreateEnemy(10, 0.5f, 1f, 1f);
        yield return new WaitForSeconds(1F);
        StartCoroutine(Round());
    }

    private void Init()
    {
        UIReset();   
    }

    private void UIReset()
    {
        hpTxt.text = health.ToString();
    }

    public void Damage(float damage)
    {
        health -= damage;
        UIReset();
        if (health <= 0) GameOver();
    }

    public bool IsGame()
    {
        return isGame;
    }

    public void GameOver()
    {
        isGame = false;
    }
}
