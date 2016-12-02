using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{

    public static HealthBar instance;

    [Header("Player")]
    public GameObject _playerHpBar;
    public Image _playerBar;

    [Header("Enemy")]
    public GameObject _targetHpBar;
    public Image _targetBar;
    public Text _targetHp;
    public Text _targetName;

    private string _targetId = "";

    private int isUsed = 0;

    private GameManager _gm;

    private const float BAR_W = 500f;
    private const float BAR_H = 50f;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Already loaded instance: HealthBar.cs");
            return;
        }
        instance = this;

        _targetHpBar.SetActive(false);
    }

    void Start()
    {
        _gm = GameManager.instance;
        StartCoroutine(UpdateBar());
    }

    public void ViewEnemyBar(float maxHp, float nowHp, float damage, string name, string id)
    {
      //  StopCoroutine("ViewTest");
        StartCoroutine(WhileTimeScreen(maxHp, nowHp, damage, name, id));
    }

    private IEnumerator UpdateBar()
    {
        while (true)
        {
            if (_gm.nowHp / _gm.maxHp * BAR_W < _playerBar.rectTransform.sizeDelta.x)
            {
                _playerBar.rectTransform.sizeDelta = new Vector2(_playerBar.rectTransform.sizeDelta.x - 5f, BAR_H);
            }
            yield return null;
        }
    }

    private IEnumerator WhileTimeScreen(float maxHp, float nowHp, float damage, string name, string id)
    {
        if (isUsed == 0) _targetHpBar.SetActive(true);

        isUsed++;
        //Debug.Log("VIEW UI : " + isUsed);

        _targetHp.text = nowHp + "/" + maxHp;
        _targetName.text = name;

        float startHp = (nowHp + damage) / maxHp;
        float finishHp =  nowHp / maxHp;

        if (!_targetId.Equals(id))
        {
            _targetBar.rectTransform.sizeDelta = new Vector2(startHp * BAR_W, BAR_H);
            _targetId = id;
        }

        while (finishHp * 500f < _targetBar.rectTransform.sizeDelta.x && _targetId.Equals(id)) {
            _targetBar.rectTransform.sizeDelta = new Vector2(_targetBar.rectTransform.sizeDelta.x - 5f, 50);
            yield return null;
        }

        if (_targetId.Equals(id))
        {
            _targetBar.rectTransform.sizeDelta = new Vector2(finishHp * BAR_W, BAR_H);
        }
        // Debug.Log("CALC UI : " + isUsed + " : " + nowHp / maxHp);
        yield return new WaitForSeconds(3);

        isUsed--;

       // Debug.Log("CLOSE UI" + isUsed);

        if (isUsed == 0) _targetHpBar.SetActive(false);
    }
}
