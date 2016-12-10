using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviourC {

    public Text _result;

    private string _time;
    private int _gameExp;
    private int _userExp;
    private string _gameResult;
    private string _gameMode;

	// Use this for initialization
	void Start () {
        StartCoroutine(ScreenManager.instance.FadeOut());
        GetData();
        SetText();
    }
	
	// Update is called once per frame
	void Update () {
        WaitTouch();
	}

    private void GetData()
    {
        _time = PlayerPrefs.GetString("GAME_TIME");
        PlayerPrefs.DeleteKey("GAME_TIME");
        _userExp = PlayerPrefs.GetInt("EXP");
        _gameExp = PlayerPrefs.GetInt("GAME_EXP");
        PlayerPrefs.DeleteKey("GAME_EXP");
        _gameMode = PlayerPrefs.GetString("GAME_MODE");
        PlayerPrefs.DeleteKey("GAME_MODE");
        _gameResult = PlayerPrefs.GetString("GAME_RESULT");
        PlayerPrefs.DeleteKey("GAME_RESULT");
    }

    private void SetText()
    {
        _result.text = _gameMode + " " + _gameResult;
    }

    private void WaitTouch()
    {
        if (GetPingerDown())
        {
            StartCoroutine(ScreenManager.instance.FadeIn("Title"));
        }
    }
}
