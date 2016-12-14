#define TEST

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public enum ViewType
{
    TPS,
    FPS,
    MINE,
}

public enum MineState
{
	None,
    UnSelected,
    JustSet,
    SelectedFullDurability,
    Selected,
    FullUpgradeFullDurability,
    FullUpgrade,
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

    [Header("SideArea")]
    public GameObject _side;

    [Header("Footer")]
    public GameObject _footer;
    public GameObject _viewChangeBtn;
    public GameObject _skill;

    [Header("Mine")]
    public GameObject _mines;
    public MineStateWindow _mineState;
    public GameObject _mineCancle;
    public GameObject _mineUndo;
    public GameObject _mineRemove;
    public GameObject _mineUpgrade;
    public GameObject _mineAdd;
    public GameObject _mineRepair;

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

    private Blur[] _blurs;
    private Blur _blur;
    private Blur _superBlur;

    private const float BAR_W = 1800f;
    private const float BAR_H = 5f;
    private const float UP_BAR = 9f;

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

        _blurs = _cam.GetComponents<Blur>();
        _blur = _blurs[0];
        _superBlur = _blurs[1];
    }

    void Start () {
        Debug.Log("VIEW MANAGER IS READY");
#if !TEST
        StartCoroutine(ScreenManager.instance.FadeOut());
#endif
        Init();

#if UNITY_EDITOR
         StartCoroutine(CheckKey());
#endif
    }

    void Update () {
        ReadyKey();
        TurnRotate();
    }

    private void Init()
    {
        _arrow = _fps.rotation.eulerAngles.y;
        ChangeViewFps();
    }

    private void ReadyKey()
    {
        if (GameManager.instance.pause) return;
        if (viewMode == ViewType.MINE) return;

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
    private void TurnRotate()
    {
        if (_viewMode != ViewType.TPS) return;
        if (!GetPinger()) return;
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject()) return;
#elif UNITY_ANDROID
        if (EventSystem.current.IsPointerOverGameObject(0)) return;
#endif

        Vector3 inputPos = PingerPosition(0);
        inputPos.z = 1f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(inputPos);

        //Debug.Log(worldPos);
        Quaternion temp = Quaternion.LookRotation(worldPos);
        _arrow = temp.eulerAngles.y;

        _player.transform.rotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.x, _arrow, 0);
        _fps.rotation = Quaternion.Euler(_fps.rotation.eulerAngles.x, _arrow, 0);
        ChangeHandAngle();
    }

    private void RotateView(Arrow arrow)
    {
		weaponModel.RotateView(arrow);
        if (arrow == Arrow.Left) _arrow -= _rotateSpeed;
        if (arrow == Arrow.Right) _arrow += _rotateSpeed;
        _fps.rotation = Quaternion.Euler(_fps.rotation.eulerAngles.x, _arrow, 0);
        _cam.transform.rotation = _fps.rotation;
    }

    private void ChangeHandAngle()
    {
        _hand.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -_arrow);
    }
#endregion

    #region ChangeView
    private void ChangeViewFps()
    {
        _cam.transform.position = _fps.position;
        _cam.transform.rotation = _fps.rotation;

        _hand.SetActive(false);
        _weapon.SetActive(true);
        _player.SetActive(false);
        _mines.SetActive(false);
        _skill.SetActive(true);
        _side.SetActive(true);

        if (!_blur.enabled)
        {
            _background.gameObject.SetActive(false);
        }
        _viewMode = ViewType.FPS;
    }

    private void ChangeViewTps()
    {
        if (_blur.enabled) return;
        _cam.transform.position = _tps.position;
        _cam.transform.rotation = _tps.rotation;

        _hand.SetActive(true);
        _weapon.SetActive(false);
        _player.SetActive(true);
        _skill.SetActive(false);
        _mines.SetActive(false);
        _side.SetActive(false);

        if (!_blur.enabled)
        {
            _background.gameObject.SetActive(true);
            _background.color = new Color(50 / 255f, 50 / 255f, 50 / 255f, 50f / 255f);
        }
        _viewMode = ViewType.TPS;
    }

    private void ChangeViweMine()
    {
        _cam.transform.position = _mine.position;
        _cam.transform.rotation = _mine.rotation;

        _hand.SetActive(false);
        _weapon.SetActive(false);
        _player.SetActive(true);
        _skill.SetActive(false);
        _mines.SetActive(true);
        _side.SetActive(false);

        _shop.gameObject.SetActive(false);
        _mine.gameObject.SetActive(true);
        _viewChangeBtn.gameObject.SetActive(false);

        //Unselected();

        _blur.enabled = false;

        _background.gameObject.SetActive(true);
        _background.color = new Color(50 / 255f, 50 / 255f, 50 / 255f, 50f / 255f);

        _viewMode = ViewType.MINE;

		SetMineBtn(MineState.None);
    }
    #endregion

    #region Mine
    public void SetMineBtn(MineState state)
    {
        switch (state) {
			case MineState.None:
				_mineAdd.SetActive(false);
				_mineUpgrade.SetActive(false);
				_mineRemove.SetActive(false);
				_mineRepair.SetActive(false);
				_mineUndo.SetActive(false);
				return;
            case MineState.UnSelected:
                _mineAdd.SetActive(true);
                _mineUpgrade.SetActive(false);
                _mineRemove.SetActive(false);
                _mineRepair.SetActive(false);
                _mineUndo.SetActive(false);
                return;
            case MineState.JustSet:
                _mineAdd.SetActive(false);
                _mineUpgrade.SetActive(true);
                _mineRemove.SetActive(false);
                _mineRepair.SetActive(false);
                _mineUndo.SetActive(true);
                return;
            case MineState.SelectedFullDurability:
                _mineAdd.SetActive(false);
                _mineUpgrade.SetActive(true);
                _mineRemove.SetActive(true);
                _mineRepair.SetActive(false);
                _mineUndo.SetActive(false);
                return;
            case MineState.Selected:
                _mineAdd.SetActive(false);
                _mineUpgrade.SetActive(true);
                _mineRemove.SetActive(true);
                _mineRepair.SetActive(true);
                _mineUndo.SetActive(false);
                return;
            case MineState.FullUpgrade:
                _mineAdd.SetActive(false);
                _mineUpgrade.SetActive(false);
                _mineRemove.SetActive(true);
                _mineRepair.SetActive(true);
                _mineUndo.SetActive(false);
                return;
            case MineState.FullUpgradeFullDurability:
                _mineAdd.SetActive(false);
                _mineUpgrade.SetActive(false);
                _mineRemove.SetActive(true);
                _mineRepair.SetActive(false);
                _mineUndo.SetActive(false);
                return;
        }
    }

    public void SetMineState(int level, int power, float delay, int maxDurability, int nowDurability)
    {
        _mineState.RenderState(level, power, delay, maxDurability, nowDurability);
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
        ChangeViewFps();
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
		StartCoroutine(BlurOn());
    }

    public void TouchExitShop()
    {
        _shop.ExitShop();
        CloseShop();
    }
#endregion
}
