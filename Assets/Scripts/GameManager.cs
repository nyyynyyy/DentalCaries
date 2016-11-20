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
    public float health = 100;
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
	}
	
	void Update () {
	
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
