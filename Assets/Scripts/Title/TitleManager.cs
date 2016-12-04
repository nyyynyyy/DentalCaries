using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviourC {

    public Canvas title;
    public Canvas menu;
    public Canvas fade;
    private CanvasGroup titleGroup;
    private CanvasGroup menuGroup;
    private CanvasGroup fadeGroup;

    private bool isTitle = true;
    private bool isMenu = false;

	void Awake()
    {
        titleGroup = title.GetComponent<CanvasGroup>();
        menuGroup = menu.GetComponent<CanvasGroup>();
        fadeGroup = fade.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        WaitTouch();
    }

    private void Init()
    {
        titleGroup.alpha = 1;
        menuGroup.alpha = 0;
        fadeGroup.alpha = 0;
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
        StartCoroutine(FadeIn("Test Nyyynyyy"));
    }

    private IEnumerator FadeIn(string scene)
    {
        isMenu = false;
        fade.gameObject.SetActive(true);
        while(fadeGroup.alpha < 1)
        {
            fadeGroup.alpha += 0.02f;
            yield return null;
        }
        SceneManager.LoadScene(scene);
    }
}
