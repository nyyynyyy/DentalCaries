using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{

    public static HealthBar instance;

    public GameObject _me;
    public Image _bar;
    public Text _target;

    private bool isUsed;

    void Start()
    {
        if (instance)
        {
            Debug.LogError("Already loaded instance: HealthBar.cs");
            return;
        }
        instance = this;
    }

    public IEnumerator ViewUi(float maxHp, float nowHp, string name)
    {
        Debug.Log("VIEW");
        if (!isUsed) _me.SetActive(true);
        _target.text = name + " " + (nowHp / maxHp) * 100;
        _bar.rectTransform.sizeDelta = new Vector2(nowHp / maxHp * 1000, 100);
        yield return new WaitForSeconds(3);
        _me.SetActive(false);
    }
}
