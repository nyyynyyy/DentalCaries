using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviourC {

    public Text _result;

    public ExpBar _bar;

    private int _time;
    private int _gameExp;
    private int _userExp;
    private int _userLevel;
    private int _userTicket;
    private int _gameOver;
    private int _gameMode;

	void Start ()
    {
        StartCoroutine(ScreenManager.instance.FadeOut());
        GetData();
        TakeExp();
        SetText();

        StartCoroutine(_bar.BarDecrease(_gameExp));
    }
	
	void Update ()
    {
        WaitTouch();
	}

    private void GetData()
    {
        _time = UserManager.instance.GetUserData(GameKey.Time);

        _userExp = UserManager.instance.GetUserData(UserKey.Exp);
        _gameExp = UserManager.instance.GetUserData(GameKey.Exp);
        _userLevel = UserManager.instance.GetUserData(UserKey.Level);
        _userTicket = UserManager.instance.GetUserData(UserKey.Ticket);

        _gameMode = UserManager.instance.GetUserData(GameKey.Mode);
        _gameOver = UserManager.instance.GetUserData(GameKey.OverType);
    }

    private void TakeExp()
    {
        _userExp += _gameExp;

        int levelUpPoint = _userExp / 1000;

        _userLevel += levelUpPoint;
        _userTicket += levelUpPoint;
        _userExp = _userExp % 1000;

        UserManager.instance.SetUserData(UserKey.Level, _userLevel);
        UserManager.instance.SetUserData(UserKey.Exp, _userExp);
        UserManager.instance.SetUserData(UserKey.Ticket, _userTicket);
    }

    private void SetText()
    {
        _result.text = ((GameMode)_gameMode).ToString() + " " + ((GameOverType)_gameOver).ToString();
    }

    private void WaitTouch()
    {
        if (GetPingerDown())
        {
            StartCoroutine(ScreenManager.instance.FadeIn("Title"));
        }
    }
}
