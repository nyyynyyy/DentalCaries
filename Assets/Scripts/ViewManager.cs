using UnityEngine;
using System.Collections;

public class ViewManager : MonoBehaviour {

    public Camera cam;

    public Transform fps;
    public Transform tps;

    public float _speed;

    private float _arrow;

    private enum Arrow
    {
        Right,
        Left,
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
        _arrow = fps.rotation.eulerAngles.y;
        Debug.Log(_arrow);
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
        if (cam.transform.position == tps.position) return;
        if (arrow == Arrow.Left){ _arrow -= _speed; Debug.Log("L"); }
        if(arrow == Arrow.Right){ _arrow += _speed; Debug.Log("R"); }
        fps.rotation = Quaternion.Euler(fps.rotation.eulerAngles.x, _arrow, 0);
        cam.transform.rotation = fps.rotation;
    }

    private void ChangeViewFps()
    {
        if (cam.transform.position == fps.position) return; 
        cam.transform.position = fps.position;
        cam.transform.rotation = fps.rotation;
    }

    private void ChangeViewTps()
    {
        if (cam.transform.position == tps.position) return;
        cam.transform.position = tps.position;
        cam.transform.rotation = tps.rotation;
    }
}
