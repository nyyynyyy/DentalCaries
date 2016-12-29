using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour {

    [Header("Canvas")]
    public CanvasGroup _canvas;

    [Header("Menu Buttons")]
    public Button[] _btnMenu;
    public GameObject[] _areaMenu;

    public Image _background;

    public Color _selectedColor;
    public Color _defaultColor;

    [Header("Side Area")]
    public GameObject _sideArea;

    [Header("Menu 2")]
    public ExpBar _bar;

    [Header("Menu 3")]
    public Text _textLevel;
    public Text _textExp;
    public Text _textTicket;

    private Menu _selectedMenu = Menu.menu1;

    private bool _isSelected = false;

    public enum Menu
    {
        menu1, menu2, menu3, menu4,
    }

    void Start()
    {
     //   StartCoroutine("moveMenu", Menu.menu1);
        _sideArea.SetActive(false);
    }

    private IEnumerator FadeMenu(Menu menu)
    {
        _canvas.alpha = 0;

        _areaMenu[(int)menu].SetActive(true);

        while(_canvas.alpha < 1)
        {
            _canvas.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        _sideArea.SetActive(true);

        // wait return menu
        while (_isSelected)
        {
            yield return null;
        }
        // wait return menu

        _isSelected = true;

        while (_canvas.alpha > 0)
        {
            _canvas.alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < _areaMenu.Length; i++)
        {
            _areaMenu[i].SetActive(false);
        }

        _isSelected = false;
    }

    private IEnumerator MoveButton(Menu menu)
    {
        float time = 20f;

        float dis = -_btnMenu[(int)menu].transform.position.x / time;

        for(int i = 0; i < time; i++)
        {
            _btnMenu[(int)menu].transform.position += new Vector3(dis, 0);
            for(int j = 0; j < _btnMenu.Length; j++)
            {
                if (j == (int)menu) continue;
                _btnMenu[j].GetComponent<CanvasGroup>().alpha -= 0.05f;
            }
            yield return new WaitForSeconds(0.01f);
        }

        // wait return menu
        while (_isSelected)
        {
            yield return null;
        }
        // wait return menu


        for (int i = 0; i < time; i++)
        {
            _btnMenu[(int)menu].transform.position -= new Vector3(dis, 0);
            for (int j = 0; j < _btnMenu.Length; j++)
            {
                if (j == (int)menu) continue;
                _btnMenu[j].GetComponent<CanvasGroup>().alpha += 0.05f;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void RendMenuText(Menu menu)
    {
        for(int i = 0; i < _btnMenu.Length; i++)
        {
            Color buttonColor = (i == (int)menu ? _selectedColor : _defaultColor);
            _btnMenu[i].GetComponent<Text>().color = buttonColor;
        }
    }

    public void ReturnMenu()
    {
        if (!_isSelected) return;
        _isSelected = false;
        _sideArea.SetActive(false);
    }

    public void ChangeMenu(int menu)
    {
        if (_isSelected) return;
        _isSelected = true;

        RendMenuText((Menu)menu);
        StartCoroutine(FadeMenu((Menu)menu));
        StartCoroutine(MoveButton((Menu)menu));

        switch (menu)
        {
            case 2:
                _bar.RenderBar();
                return;
            case 3:
                _textLevel.text = "Level\t" + UserManager.instance.GetUserData(UserKey.Level);
                _textExp.text = "Exp\t" + UserManager.instance.GetUserData(UserKey.Exp);
                _textTicket.text = "Ticket\t" + UserManager.instance.GetUserData(UserKey.Ticket);
                return;
        }
    }
}
