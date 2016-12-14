using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    Tutorial,
    Easy,
    Normal,
    Hard,
    Crazy,
}

public class TitleManager : MonoBehaviourC {

    public Canvas _title;
    public Canvas _menu;
    private CanvasGroup _titleGroup;
    private CanvasGroup _menuGroup;

    public static bool _isTitle = true;
    public static bool _isMenu = false;

    [Header ("Temp Button")]
    public Text _btnNyyynyyy;
    public Text _btnINSI;

    private string _targetScene = "Test Nyyynyyy";

    void Awake()
    {
        _titleGroup = _title.GetComponent<CanvasGroup>();
        _menuGroup = _menu.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Init();
        Time.timeScale = 1f;
    }

    void Update()
    {
        WaitTouch();
    }

    private void Init()
    {
        if (_isTitle)
        {
            _titleGroup.alpha = 1;
            _menuGroup.alpha = 0;
            _title.gameObject.SetActive(true);
            _menu.gameObject.SetActive(false);
        }
        else if(!_isTitle)
        {
            _titleGroup.alpha = 0;
            _menuGroup.alpha = 1;
            _title.gameObject.SetActive(false);
            _menu.gameObject.SetActive(true);
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
        _isTitle = false;
        while(_titleGroup.alpha > 0)
        {
            _titleGroup.alpha -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        _title.gameObject.SetActive(false);
        _menu.gameObject.SetActive(true);
        while(_menuGroup.alpha < 1)
        {
            _menuGroup.alpha += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        _isMenu = true;
    }

    public void StartGame()
    {
        if (!_isMenu) return;

        UserManager.instance.SetUserData(GameKey.Mode, (int)GameMode.Tutorial);
        StartCoroutine(ScreenManager.instance.FadeIn(_targetScene));
        _isMenu = true;
    }

    #region Temp Method
    public void ButtonNyyynyyy()
    {
        _btnINSI.color = new Color(133 / 255f, 133 / 255f, 133 / 255f);
        _btnNyyynyyy.color = new Color(255 / 255, 255 / 255, 255 / 255);
        _targetScene = "Test Nyyynyyy";
    }

    public void ButtonINSI()
    {
        _btnNyyynyyy.color = new Color(133 / 255f, 133 / 255f, 133 / 255f);
        _btnINSI.color = new Color(255 / 255, 255 / 255, 255 / 255);
        _targetScene = "Test INSI";
    }

    public void ButtonClear()
    {
        UserManager.instance.ClearUserData(true);
    }
    #endregion
}
