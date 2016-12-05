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
    public Image _targetMoveBar;
    public Text _targetHp;
    public Text _targetName;

    private string _targetId = "";

    private int isUsed = 0;

    private GameManager _gm;

    private const float BAR_W = 500f;
    private const float BAR_H = 35f;
    private const float MOVE_BAR_H = 5f;
    private const float DOWN_BAR = 15f;

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

    public void ViewEnemyBar(Enemy enemy, float damage)
    {
        StartCoroutine(WhileTimeScreen(enemy, damage));
        StartCoroutine(DisBar(enemy));
    }

    private IEnumerator UpdateBar()
    {
        while (true)
        {
            if (_gm.nowHp / _gm.maxHp * BAR_W < _playerBar.rectTransform.sizeDelta.x)
            {
                _playerBar.rectTransform.sizeDelta = new Vector2(_playerBar.rectTransform.sizeDelta.x - 5f, BAR_H + MOVE_BAR_H);
            }
            yield return null;
        }
    }

    private IEnumerator WhileTimeScreen(Enemy enemy, float damage)
    {
        if (isUsed == 0) _targetHpBar.SetActive(true);

        isUsed++;
        //Debug.Log("VIEW UI : " + isUsed);

        _targetHp.text = enemy.nowHp + " / " + enemy.maxHp;
        _targetName.text = enemy.name;

        float startHp = Mathf.Max((enemy.nowHp + damage), enemy.maxHp) / enemy.maxHp;
        float finishHp = enemy.nowHp / enemy.maxHp;

        if (!_targetId.Equals(enemy.transform.name)) // new enemy bar
        {
            _targetBar.rectTransform.sizeDelta = new Vector2(startHp * BAR_W, BAR_H);
            _targetId = enemy.transform.name;
        }

        while (finishHp * BAR_W < _targetBar.rectTransform.sizeDelta.x) {
            if (!_targetId.Equals(enemy.transform.name)) break;
            float barWeight = Mathf.Max(_targetBar.rectTransform.sizeDelta.x - DOWN_BAR, finishHp * BAR_W);
            _targetBar.rectTransform.sizeDelta = new Vector2(barWeight, BAR_H);
            yield return null;

            if (!RoundManager.instance.isPlayingRound) { // when finish Round
                isUsed = 0;
                _targetHpBar.SetActive(false);
                yield break;
            }
        }

        if (_targetId.Equals(enemy.transform.name))
        {
            _targetBar.rectTransform.sizeDelta = new Vector2(finishHp * BAR_W, BAR_H);
        }

        yield return new WaitForSeconds(3); // during 3 sec view screen

        isUsed--;

        if (isUsed == 0) _targetHpBar.SetActive(false);
    }

    private IEnumerator DisBar(Enemy enemy)
    {
        while (_targetBar.IsActive())
        {
            if (!_targetId.Equals(enemy.transform.name)) yield break;
            if (enemy.isDeath) yield break;
            _targetMoveBar.rectTransform.sizeDelta = new Vector2(enemy.leftDis * BAR_W, MOVE_BAR_H);
            yield return null;
        }
    }
}
