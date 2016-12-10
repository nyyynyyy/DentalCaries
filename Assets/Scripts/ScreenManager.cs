using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour {

    public static ScreenManager instance;
    
    public GameObject _fade;

    private CanvasGroup _fadeGroup;

    private bool _isFading = false;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : ScreenManager");
        }
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator FadeIn(string scene)
    {
        if (_isFading) yield break;
        _isFading = true;
        GameObject fade = (GameObject)Instantiate(_fade, Vector3.zero, Quaternion.identity);
        fade.transform.SetSiblingIndex(100);
        _fadeGroup = fade.GetComponent<CanvasGroup>();
        _fadeGroup.alpha = 0;
        while (_fadeGroup.alpha < 1)
        {
            _fadeGroup.alpha += 0.02f;
            yield return null;
        }
        _isFading = false;
        SceneManager.LoadScene(scene);
    }

    public IEnumerator FadeOut()
    {
        if (_isFading) yield break;
        _isFading = true;
        GameObject fade = (GameObject)Instantiate(_fade, Vector3.zero, Quaternion.identity);
        fade.transform.SetSiblingIndex(100);
        _fadeGroup = fade.GetComponent<CanvasGroup>();
        _fadeGroup.alpha = 1;
        while (_fadeGroup.alpha > 0)
        {
            _fadeGroup.alpha -= 0.01f;
            yield return null;
        }
        Destroy(fade);
        _isFading = false;
    }

}
