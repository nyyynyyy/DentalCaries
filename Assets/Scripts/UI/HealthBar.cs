using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{

    public static HealthBar instance;

    public GameObject _me;
    public Image _bar;
    public Text _targetHp;
    public Text _targetName;

    private int isUsed = 0;

    void Start()
    {
        if (instance)
        {
            Debug.LogError("Already loaded instance: HealthBar.cs");
            return;
        }
        instance = this;

        _me.SetActive(false);
    }

    public void ViewUi(float maxHp, float nowHp, string name)
    {
      //  StopCoroutine("ViewTest");
        StartCoroutine(WhileTimeView(maxHp, nowHp, name));
    }

    private IEnumerator WhileTimeView(float maxHp, float nowHp, string name)
    {
        if (isUsed == 0) _me.SetActive(true);

        isUsed++;
        //Debug.Log("VIEW UI : " + isUsed);

        _targetHp.text = nowHp + "/" + maxHp;
        _targetName.text = name;
        _bar.rectTransform.sizeDelta = new Vector2(nowHp / maxHp * 1000, 100);

       // Debug.Log("CALC UI : " + isUsed + " : " + nowHp / maxHp);
        yield return new WaitForSeconds(3);

        isUsed--;

       // Debug.Log("CLOSE UI" + isUsed);

        if (isUsed == 0) _me.SetActive(false);
    }
}
