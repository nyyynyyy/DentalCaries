using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : Bar {

    [Header("Text")]
    public Text _textLevel;
    public Text _textExp;
    public Text _textTicket;

    private int _prevLevel;
    private int _nowLevel;

    private int _prevExp;
    private int _nowExp;
    private int _maxExp;

    private int _prevTicket;
    private int _nowTicket;

    void Start()
    {
        base.RenderBar(NumberToWidth(0, 1));
    }
    
    public void RenderBar()
    {
        FindUserData();
        RenderText();

        float targetWidth = NumberToWidth(_nowExp, _maxExp);

        base.RenderBar(targetWidth);
    }

    private void RenderText()
    {
        string renderExp = _prevExp + " / " + _maxExp;
        _textExp.text = renderExp;
        _textLevel.text = _prevLevel.ToString();
        _textTicket.text = _prevTicket.ToString();
    }

    private void FindUserData()
    {
        _prevExp = _nowExp = UserManager.instance.GetUserData(UserKey.Exp);
        _maxExp = 1000;
        _prevLevel = _nowLevel = UserManager.instance.GetUserData(UserKey.Level);
        _prevTicket = _nowTicket = UserManager.instance.GetUserData(UserKey.Ticket);
    }

    public IEnumerator BarDecrease(int gameExp)
    {
        CalcPrev(gameExp);
        RenderText();

        float initWidth = NumberToWidth(_prevExp, _maxExp);

        base.RenderBar(initWidth);

        int goalExp = _prevExp + gameExp;

        while (goalExp > _prevExp)
        {
            _prevExp += (int)_barSpeed;
            if(_prevExp > _maxExp)
            {
                _prevLevel++;
                _prevExp -= _maxExp;
                _prevTicket++;

                goalExp -= _maxExp;
            }
            float targetWidth = NumberToWidth(_prevExp, _maxExp);
            base.RenderBar(targetWidth);
            RenderText();

            yield return null;
        }

        base.RenderBar(NumberToWidth(_nowExp, _maxExp));
    }

    private void CalcPrev(int gameExp)
    {
        FindUserData();

        int totalExp = _nowLevel * _maxExp + _nowExp - gameExp;

        _prevLevel = totalExp / _maxExp;
        _prevExp = totalExp % _maxExp;
        _prevTicket = _nowTicket - (_nowLevel - _prevLevel);
    }
}
