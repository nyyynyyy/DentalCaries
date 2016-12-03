using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour {

    public Canvas _ui;
    public Canvas _set;

    private IEnumerator _openCor;
    private IEnumerator _closeCor;

    public void openSetting()
    {
        if (ViewManager.instance.isBlur) return;

        GameManager.instance.PauseGame();
        _ui.renderMode = RenderMode.ScreenSpaceCamera;
        _set.gameObject.SetActive(true);

        _openCor = ViewManager.instance.SuperBlurOn();
        _closeCor = ViewManager.instance.SuperBlurOff();

        StopCoroutine(_closeCor);
        StartCoroutine(_openCor);
      //  Debug.Log("open");

    }

    public void closeSetting()
    {
        if (!GameManager.instance.pause) return;

        GameManager.instance.ResumeGame();

        _openCor = ViewManager.instance.SuperBlurOn();
        _closeCor = ViewManager.instance.SuperBlurOff();

        StopCoroutine(_openCor);
        StartCoroutine(_closeCor);
       // Debug.Log("close");

        _set.gameObject.SetActive(false);
        _ui.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}
