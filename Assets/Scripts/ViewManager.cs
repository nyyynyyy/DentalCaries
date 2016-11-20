using UnityEngine;
using System.Collections;

public class ViewManager : MonoBehaviour {

    public static ViewManager instance;

    public Camera _cam;
    public GameObject _hand;

    public Transform _fps;
    public Transform _tps;

    public float _speed;

    private float _arrow;

    private bool _isTps;

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

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : ViewManager");
        }
        instance = this;
    }

    void Start () {
        Init();
    }
	
	void Update () {
        ReadyKey();
	}

    private void Init()
    {
        ChangeViewFps();
        _arrow = _fps.rotation.eulerAngles.y;
    }

    private void ReadyKey()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ChangeViewTps();
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
            _hand.transform.rotation = Quaternion.Euler(0, _arrow, 0);
        }
        else
        { 
            _cam.transform.rotation = _fps.rotation;
        }
    }

    private void ChangeViewFps()
    {
        if (!_isTps) return; 
        _cam.transform.position = _fps.position;
        _cam.transform.rotation = _fps.rotation;
        _hand.SetActive(false);
        _isTps = false;
    }

    private void ChangeViewTps()
    {
        if (_isTps) return;
        _cam.transform.position = _tps.position;
        _cam.transform.rotation = _tps.rotation;
        _hand.SetActive(true);
        _hand.transform.rotation = Quaternion.Euler(0,_arrow,0);
        _isTps = true;
    }
}
