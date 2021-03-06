﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameOverType
{
    Clear,
    Give,
    Death,
}

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("Game State")]
    [SerializeField] private int _round = 1;
    [SerializeField] private int _money = 0;
    [SerializeField] private int _exp = 0;
    [SerializeField] private bool _gamePaues = false;

    [Header("Player State")]
    [SerializeField] private float _maxHealth = 1000f;
    [SerializeField] private float _nowHealth = 1000f;
    [SerializeField] private float _power = 1f;

    private bool _isGame = false;

    #region Property
    public int money
    {
        get
        {
            return _money;
        }
    }
    public float maxHp
    {
        get
        {
            return _maxHealth;
        }
    }
    public float nowHp
    {
        get
        {
            return _nowHealth;
        }
    }
    public int round
    {
        get
        {
            return _round;
        }
    }
    public float power
    {
        get
        {
            return _power;
        }
    }
    public bool pause
    {
        get
        {
            return _gamePaues;
        }
    }
    #endregion

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : GameManger");
        }
        instance = this;
    }

    void Start () {
        Debug.Log("GAME MANAGER IS READY");
    }

    public void TakeMoney(int cost)
    {
        _money += cost;
        _exp += cost; // 마인 설치 시 문제
        TextManager.instance.ViewGold();
    }

	public bool SpendMoney(int pay) {
		if (_money < pay)
			return false;
		_money -= pay;
		TextManager.instance.ViewGold();
		return true;
	}

    public void Damage(float damage)
    {
        _nowHealth -= damage;
        StartCoroutine(PlayerHpBar.instance.BarDecrease());
        if (_nowHealth <= 0) Gameover(GameOverType.Death);
    }

    public bool IsGame()
    {
        return _isGame;
    }

    public void RoundClear()
    {
        _round++;
    }

    public void PowerUp()
    {
        int pay = 100;
        if (_money >= pay)
        {
            _power++;
            _money -= pay;
        }
        TextManager.instance.ViewAttack();
        TextManager.instance.ViewGold();
    }

    public void PauseGame()
    {
        _gamePaues = true;
        Time.timeScale = 0.2f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _gamePaues = false;
    }

    public void GiveGame()
    {
        ResumeGame();
        Gameover(GameOverType.Give);
    }

    public void ClearGame()
    {
        ResumeGame();
        Gameover(GameOverType.Clear);
    }

    private void Gameover(GameOverType result) {
        _isGame = false;

        UserManager.instance.SetUserData(GameKey.OverType, (int)result);
        UserManager.instance.SetUserData(GameKey.Exp, _exp);
        UserManager.instance.SetUserData(GameKey.Time, 65);

        StopAllCoroutines();
        StartCoroutine(ScreenManager.instance.FadeIn("Game over"));
    }
}
