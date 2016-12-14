using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineStateWindow : MonoBehaviour {

    public Text _level;
    public Text _power;
    public Text _delay;
    public Text _durability;

    public Image _durabilityBar;

    public void RenderState(int level, int power, float delay, int maxDurability, int nowDurability)
    {
        _level.text = level.ToString();
        _power.text = power.ToString();
        _delay.text = delay + "/s";
        _durability.text = nowDurability + " / " + maxDurability;

        _durabilityBar.rectTransform.sizeDelta = new Vector2((float)nowDurability / (float)maxDurability * 300f, 50f);
    }
}
