using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour {

    [Header("Menu Buttons")]
    public Button[] _btnMenu;
    public GameObject _areasMenu;
    public GameObject[] _areaMenu;
    public Image _selectedLine;

    public Image _background;

    public Color _selectedColor;
    public Color _defaultColor;

    [Header("Menu 2")]
    public ExpBar _bar;

    [Header("Menu 3")]
    public Text _textLevel;
    public Text _textExp;
    public Text _textTicket;

    private float[] _posLine = new float[4] {-800f, -400f, 0, 400f};
    private float[] _posMenu = new float[4] {0, 1920f, 3840f, 5760f};

    private Menu _selectedMenu = Menu.menu1;

    private bool _isMoving = false;

    enum Menu
    {
        menu1, menu2, menu3, menu4,
    }

    void Start()
    {
        StartCoroutine("moveMenu", Menu.menu1);
    }

    private IEnumerator moveMenu(Menu menu)
    {
        _isMoving = true;

        float disLine = (_posLine[(int)menu] - _posLine[(int)_selectedMenu]) / 10f;
        float disMenu = (_posMenu[(int)menu] - _posMenu[(int)_selectedMenu]) / 10f;
        float disBack = ((int)menu - (int)_selectedMenu) * 640 / 10f;
        while (_selectedLine.transform.localPosition.x != _posLine[(int)menu])
        {
            _selectedLine.transform.localPosition += new Vector3(disLine, 0);
            _areasMenu.transform.localPosition -= new Vector3(disMenu, 0);
            _background.transform.localPosition -= new Vector3(disBack, 0);
            yield return new WaitForSeconds(0.01f);
        }
        rendMenuText(menu);
        _selectedMenu = menu;

        _isMoving = false;
    }

    private void rendMenuText(Menu menu)
    {
        for(int i = 0; i < _btnMenu.Length; i++)
        {
            _btnMenu[i].GetComponent<Text>().color = (i == (int)menu ? _selectedColor : _defaultColor);
        }
    }

	public void viewMenu1()
    {
        if (_isMoving) return;
        StartCoroutine("moveMenu", Menu.menu1);
    }

    public void viewMenu2()
    {
        if (_isMoving) return;
        StartCoroutine("moveMenu", Menu.menu2);
        _bar.RenderBar();
    }

    public void viewMenu3()
    {
        if (_isMoving) return;
        StartCoroutine("moveMenu", Menu.menu3);
        _textLevel.text = "Level\t" + UserManager.instance.GetUserData(UserKey.Level);
        _textExp.text = "Exp\t" + UserManager.instance.GetUserData(UserKey.Exp);
        _textTicket.text = "Ticket\t" +UserManager.instance.GetUserData(UserKey.Ticket);
    }

    public void viewMenu4()
    {
        if (_isMoving) return;
        StartCoroutine("moveMenu", Menu.menu4);
    }
}
