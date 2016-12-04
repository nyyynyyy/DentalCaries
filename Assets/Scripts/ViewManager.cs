using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ViewType
{
    TPS,
    FPS,
    MINE,
}

public class ViewManager : MonoBehaviourC {

    public static ViewManager instance;

    [Header("Player")]
    public Camera _cam;
    public GameObject _hand;
    public GameObject _weapon;
    public GameObject _player;
    public float _rotateSpeed;

    [Header("Shop")]
    public Shop _shop;

    [Header("Footer")]
    public GameObject _footer;
    public GameObject _viewChangeBtn;
    public GameObject _skill;

    [Header("Mine")]
    public GameObject _mines;

    [Header("Background")]
    public Image _background;
    public Image _headerBack;

    [Header("Fade")]
    public Canvas _fade;

    [Header("Pos")]
    public Transform _fps;
    public Transform _tps;
    public Transform _mine;

    [Header("Weapon")]
	public WeaponModelMove weaponModel;

    private float _arrow;

    private ViewType _viewMode = ViewType.FPS;

    private CanvasGroup _fadeGroup;

    private Blur[] _blurs;
    private Blur _blur;
    private Blur _superBlur;

    public enum Arrow
    {
        Right,
        Left,
    }

    public ViewType viewMode {
        get
        {
            return _viewMode;
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

        _fadeGroup = _fade.GetComponent<CanvasGroup>();

        _blurs = _cam.GetComponents<Blur>();
        _blur = _blurs[0];
        _superBlur = _blurs[1];
    }

    void Start () {
        Debug.Log("VIEW MANAGER IS READY");

        StartCoroutine(FadeOut());

        Init();

#if UNITY_EDITOR
         StartCoroutine(CheckKey());
#endif
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

        if (GetArea(PingerCode.Right))
        {
            RotateView(Arrow.Right);
        }
        if (GetArea(PingerCode.Left))
        {
            RotateView(Arrow.Left);
        }
    }

    #region Rotate
    private void RotateView(Arrow arrow)
    {
		weaponModel.RotateView(arrow);
        if (arrow == Arrow.Left) _arrow -= _rotateSpeed;
        if (arrow == Arrow.Right) _arrow += _rotateSpeed;
        _fps.rotation = Quaternion.Euler(_fps.rotation.eulerAngles.x, _arrow, 0);

        if (_viewMode == ViewType.TPS)
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
    #endregion

    #region ChangeView
    private void ChangeViewFps()
    {
        if (_viewMode == ViewType.FPS) return;
        _cam.transform.position = _fps.position;
        _cam.transform.rotation = _fps.rotation;

        _hand.SetActive(false);
        _weapon.SetActive(true);
        _player.SetActive(false);
        _mines.SetActive(false);
        _skill.SetActive(true);

        if (!_blur.enabled)
        {
            _background.gameObject.SetActive(false);
        }
        _viewMode = ViewType.FPS;
    }

    private void ChangeViewTps()
    {
        if (_viewMode == ViewType.TPS) return;
        _cam.transform.position = _tps.position;
        _cam.transform.rotation = _tps.rotation;

        _hand.SetActive(true);
        _weapon.SetActive(false);
        _player.SetActive(true);
        _skill.SetActive(false);
        _mines.SetActive(false);

        if (!_blur.enabled)
        {
            _background.gameObject.SetActive(true);
            _background.color = new Color(50 / 255f, 50 / 255f, 50 / 255f, 50f / 255f);
        }
        _viewMode = ViewType.TPS;
    }

    private void ChangeViweMine()
    {
        if (_viewMode == ViewType.MINE) return;
        _cam.transform.position = _mine.position;
        _cam.transform.rotation = _mine.rotation;

        _hand.SetActive(false);
        _weapon.SetActive(false);
        _player.SetActive(true);
        _skill.SetActive(false);
        _mines.SetActive(true);

        _shop.gameObject.SetActive(false);
        _mine.gameObject.SetActive(true);
        _viewChangeBtn.gameObject.SetActive(false);

        if (!_blur.enabled)
        {
            _background.gameObject.SetActive(true);
            _background.color = new Color(50 / 255f, 50 / 255f, 50 / 255f, 50f / 255f);
        }
        _viewMode = ViewType.MINE;
    }
    #endregion

    #region Shop
    public void OpenShop()
    {
        _shop.gameObject.SetActive(true);
        _footer.gameObject.SetActive(false);
        _mine.gameObject.SetActive(false);
        _viewChangeBtn.gameObject.SetActive(true);
        _shop.OpenShop();
    }

    public void CloseShop()
    {
        _shop.gameObject.SetActive(false);
        _footer.gameObject.SetActive(true);
        _mine.gameObject.SetActive(false);
        _viewChangeBtn.gameObject.SetActive(true);
    }
    #endregion

    #region Effect
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
        }
        _superBlur.enabled = true;
    }

    public IEnumerator SuperBlurOff()
    {
        while (_superBlur.iterations > 0)
        {
            _superBlur.iterations--;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        _superBlur.enabled = false;
    }

    private IEnumerator FadeOut()
    {
        _fade.gameObject.SetActive(true);
        while (_fadeGroup.alpha > 0)
        {
            _fadeGroup.alpha -= 0.01f;
            yield return null;
        }
        _fade.gameObject.SetActive(false);
    }
    #endregion

    #region TouchUI
    public void TouchDownRight()
    {
        AddPingerBuffer(PingerCode.Right);
    }

    public void TouchUpRight() {
        RemovePingerBuffer(PingerCode.Right);
    }

    public void TouchDownLeft()
    {
        AddPingerBuffer(PingerCode.Left);
    }

    public void TouchUpLeft()
    {
        RemovePingerBuffer(PingerCode.Left);
    }

    public void TouchViewChange()
    {
        if (_viewMode == ViewType.FPS)
        {
            ChangeViewTps();
            ChangeHandAngle();
        }
        else if (_viewMode == ViewType.TPS)
        {
            ChangeViewFps();
        }
    }

    public void TouchViewMine()
    {
        ChangeViweMine();
    }

    public void TouchMineCalcle()
    {
        ChangeViewFps();
        OpenShop();
    }

    public void TouchExitShop()
    {
        _shop.ExitShop();
        CloseShop();
    }
    #endregion
}
