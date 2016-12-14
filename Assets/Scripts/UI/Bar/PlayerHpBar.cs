using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : Bar {

    public static PlayerHpBar instance;

    [Header("Text")]
    public Text _textHp;

    private float _maxHp;
    private float _nowHp;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : PlayerHpBar");
        }
        instance = this;
    }

    void Start()
    {
        base.RenderBar(NumberToWidth(_nowHp, _maxHp));
    }

    public IEnumerator BarDecrease()
    {
        _nowHp = GameManager.instance.nowHp;
        _maxHp = GameManager.instance.maxHp;

        string renderHp = _nowHp + " / " + _maxHp;
        _textHp.text = renderHp;

        float targetWidth = NumberToWidth(_nowHp, _maxHp);

        while (targetWidth < nowWidth) {
            base.RenderBar(nowWidth - _barSpeed); 
            yield return null;
        }

        base.RenderBar(targetWidth);
    }
}
