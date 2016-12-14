using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

    [Header("Bar Image")]
    public Image _bar;

    [Header("Bar Attribute")]
    public float _barWidth;
    public float _barHeight;
    public float _barSpeed;

    protected float nowWidth
    {
        get
        {
            return _bar.rectTransform.sizeDelta.x;
        }
    }

    protected void RenderBar(float width)
    {
        width = Mathf.Min(width, _barWidth);
        width = Mathf.Max(width, 0);

        Vector2 moveBar = new Vector2(width, _barHeight);        
        _bar.rectTransform.sizeDelta = moveBar;
    }

    protected float NumberToWidth(float number, float max)
    {
        return number / max * _barWidth;
    }
}
