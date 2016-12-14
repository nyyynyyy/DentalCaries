using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : Bar {

    public static EnemyHpBar instance;

    [Header("Object")]
    public GameObject _barObject;

    [Header("Text")]
    public Text _textHp;
    public Text _textName;

    private float _maxHp;
    private float _nowHp;

    private bool _isMoved;

    private Enemy _targetEnemy;

    private IEnumerator _decreasing;
    private IEnumerator _whileShowing;

    public Enemy target
    {
        get
        {
            return _targetEnemy;
        }
    }
    public bool isMoved
    {
        get
        {
            return _isMoved;
        }
    }

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : EnemyHpBar");
        }
        instance = this;
    }

    void Start()
    {
        _barObject.SetActive(false);

        //coroutine null prevent
        _whileShowing = WhileShowBar(3f);
        _decreasing = BarDecrease(null);
    }

    public void BarDecrease(Enemy enemy, float damage)
    {
        StopCoroutine(_whileShowing);

        _barObject.SetActive(true);

        if (_targetEnemy != enemy)
        {
            //Debug.Log("New Target");
            _targetEnemy = enemy;
            _textName.text = enemy.name;

            StopCoroutine(_decreasing);

            _isMoved = true;

            _maxHp = enemy.maxHp;
            _nowHp = Mathf.Min(enemy.nowHp + damage, enemy.maxHp);
            float initWidth = NumberToWidth(_nowHp, _maxHp);

            RenderBar(initWidth);
        }

        _decreasing = BarDecrease(enemy);
        _whileShowing = WhileShowBar(3f);

        StartCoroutine(_decreasing);
        StartCoroutine(_whileShowing);
    }

    private IEnumerator BarDecrease(Enemy enemy)
    {
        _maxHp = enemy.maxHp;
        _nowHp = enemy.nowHp;

        float targetWidth = NumberToWidth(_nowHp, _maxHp);

        _nowHp = enemy.nowHp;
        _maxHp = enemy.maxHp;

        string renderHp = _nowHp + " / " + _maxHp;
        _textHp.text = renderHp;

        while (targetWidth < nowWidth)
        {
            base.RenderBar(nowWidth - _barSpeed);

            if (enemy.state == State.Idle) _isMoved = false;

            yield return null;
        }

        base.RenderBar(targetWidth);

        yield return null;
    }

    private IEnumerator WhileShowBar(float second)
    {
        yield return new WaitForSeconds(second);
        _barObject.SetActive(false);
    }
}
