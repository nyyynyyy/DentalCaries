using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewManager : MonoBehaviour {

    public static ViewManager instance;

    public Camera _cam;
    public GameObject _hand;
    public GameObject _weapon;
    public GameObject _player;

    public Image _background;

    public Transform _fps;
    public Transform _tps;

    public float _speed;

    private float _arrow;

    private bool _isTps = true;

    private Blur[] _blurs;
    private Blur _blur;
    private Blur _superBlur;

    private enum Arrow
    {
        Right,
        Left,
    }

    public bool isTps {
        get
        {
            return _isTps;
        }
    }

    public bool isBlur
    {
        get
        {
            return _superBlur.enabled;
        }
    }

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : ViewManager");
        }
        instance = this;

        _blurs = _cam.GetComponents<Blur>();
        _blur = _blurs[0];
        _superBlur = _blurs[1];
    }

    void Start () {
        Debug.Log("VIEW MANAGER IS READY");

        Init();
    }
	
	void Update () {
        ReadyKey();
	}

    private void Init()
    {
        _arrow = _fps.rotation.eulerAngles.y;
        ChangeViewFps();
    }

    private void ReadyKey()
    {
        if (GameManager.instance.pause) return;

        if (Input.GetKey(KeyCode.Space))
        {
            ChangeViewTps();
            ChangeHandAngle();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ChangeViewFps();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateView(Arrow.Right);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateView(Arrow.Left);
        }
    }

    private void RotateView(Arrow arrow)
    {
        if (arrow == Arrow.Left) _arrow -= _speed;
        if (arrow == Arrow.Right) _arrow += _speed;
        _fps.rotation = Quaternion.Euler(_fps.rotation.eulerAngles.x, _arrow, 0);

        if (_isTps)
        {
            ChangeHandAngle();
        }
        else
        {
            _cam.transform.rotation = _fps.rotation;
        }
    }

    private void ChangeHandAngle()
    {
        _hand.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -_arrow);
    }

    private void ChangeViewFps()
    {
        if (!_isTps) return; 
        _cam.transform.position = _fps.position;
        _cam.transform.rotation = _fps.rotation;
        _hand.SetActive(false);
        _weapon.SetActive(true);
        _player.SetActive(false);
        _isTps = false;
    }

    private void ChangeViewTps()
    {
        if (_isTps) return;
        _cam.transform.position = _tps.position;
        _cam.transform.rotation = _tps.rotation;
        _hand.SetActive(true);
        _weapon.SetActive(false);
        _player.SetActive(true);
        _isTps = true;
    }

    public IEnumerator BlurOn()
    {
        _background.gameObject.SetActive(true);
        while (_blur.iterations < 10)
        {
            _blur.iterations++;
            _background.color = new Color(50 / 255f, 50 / 255f, 50 / 255f, (float) _blur.iterations * 15f / 255f);
            yield return new WaitForSeconds(0.02f);
        }
        _blur.enabled = true;
    }

    public IEnumerator BlurOff()
    {
        while (_blur.iterations > 0)
        {
            _blur.iterations--;
            _background.color = new Color(50 / 255f, 50 / 255f, 50 / 255f, (float)_blur.iterations * 15f / 255f);
            yield return new WaitForSeconds(0.02f);
        }
        _background.gameObject.SetActive(false);
        _blur.enabled = false;
    }

    public IEnumerator SuperBlurOn()
    {
        while (_superBlur.iterations < 10)
        {
            _superBlur.iterations++;
            yield return new WaitForSecondsRealtime(0.02f);
           // Debug.Log("ON");
        }
        _superBlur.enabled = true;
    }

    public IEnumerator SuperBlurOff()
    {
        while (_superBlur.iterations > 0)
        {
            _superBlur.iterations--;
            yield return new WaitForSecondsRealtime(0.02f);
          //  Debug.Log("OFF");

        }
        _superBlur.enabled = false;
    }
}
