﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode{
    Tutorial,
    Easy,
    Normal,
    Hard,
    Crazy,
}

public class TitleManager : MonoBehaviourC {

    public Canvas title;
    public Canvas menu;
    private CanvasGroup titleGroup;
    private CanvasGroup menuGroup;

    public static bool isTitle = true;
    public static bool isMenu = false;

    [Header ("Temp Button")]
    public Text btnNyyynyyy;
    public Text btnINSI;

    private string targetScene = "Test Nyyynyyy";

    void Awake()
    {
        titleGroup = title.GetComponent<CanvasGroup>();
        menuGroup = menu.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        FirstGame();
        Init();
    }

    void Update()
    {
        WaitTouch();
    }

    private void FirstGame()
    {
        if (!PlayerPrefs.HasKey("PLAY_TIME"))
        {
            Debug.Log("First Running");
            PlayerPrefs.SetInt("PLAY_TIME", 0);
            PlayerPrefs.SetInt("EXP", 0);
        }
    }

    private void Init()
    {
        if (isTitle)
        {
            titleGroup.alpha = 1;
            menuGroup.alpha = 0;
            title.gameObject.SetActive(true);
            menu.gameObject.SetActive(false);
        }
        else if(!isTitle)
        {
            titleGroup.alpha = 0;
            menuGroup.alpha = 1;
            title.gameObject.SetActive(false);
            menu.gameObject.SetActive(true);
            StartCoroutine(ScreenManager.instance.FadeOut());
        }
    }

    private void WaitTouch()
    { 
        if (GetPingerDown())
        {
            StartCoroutine(ChangeAnim());
        }
    }

    private IEnumerator ChangeAnim()
    {
        isTitle = false;
        while(titleGroup.alpha > 0)
        {
            titleGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        title.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        while(menuGroup.alpha < 1)
        {
            menuGroup.alpha += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        isMenu = true;
    }

    public void StartGame()
    {
        if (!isMenu) return;

        PlayerPrefs.SetString("GAME_MODE", GameMode.Tutorial.ToString());
        StartCoroutine(ScreenManager.instance.FadeIn(targetScene));
        isMenu = true;
    }

    #region Temp Method
    public void ButtonNyyynyyy()
    {
        btnINSI.color = new Color(133 / 255f, 133 / 255f, 133 / 255f);
        btnNyyynyyy.color = new Color(255 / 255, 255 / 255, 255 / 255);
        targetScene = "Test Nyyynyyy";
    }

    public void ButtonINSI()
    {
        btnNyyynyyy.color = new Color(133 / 255f, 133 / 255f, 133 / 255f);
        btnINSI.color = new Color(255 / 255, 255 / 255, 255 / 255);
        targetScene = "Test INSI";
    }
    #endregion
}
