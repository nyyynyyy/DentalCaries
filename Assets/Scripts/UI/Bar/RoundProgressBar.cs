using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundProgressBar : Bar {

    public static RoundProgressBar instance;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : RoundProgressBar");
        }
        instance = this;
    }

    void Start ()
    {
        base.RenderBar(NumberToWidth(0, 1));
    }
	
	public IEnumerator BarLengthen()
    {
        float targetWidth = NumberToWidth(RoundManager.instance.leftPro, 1);

        while (targetWidth > nowWidth)
        {
            base.RenderBar(nowWidth + _barSpeed);

            yield return null;
        }

        base.RenderBar(targetWidth);
    }
}
