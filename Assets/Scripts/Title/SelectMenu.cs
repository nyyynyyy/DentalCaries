using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour {

    public Button[] btnMenu;
    public GameObject areasMenu;
    public GameObject[] areaMenu;
    public Image selectedLine;

    public Image background;

    public Color selectedColor;
    public Color defaultColor;

    private float[] posLine = new float[4] {-800f, -400f, 0, 400f};
    private float[] posMenu = new float[4] {0, 1920f, 3840f, 5760f};

    private Menu selectedMenu = Menu.menu1;

    private bool isMoving = false;

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
        isMoving = true;

        float disLine = (posLine[(int)menu] - posLine[(int)selectedMenu]) / 10f;
        float disMenu = (posMenu[(int)menu] - posMenu[(int)selectedMenu]) / 10f;
        float disBack = ((int)menu - (int)selectedMenu) * 640 / 10f;
        while (selectedLine.transform.localPosition.x != posLine[(int)menu])
        {
            selectedLine.transform.localPosition += new Vector3(disLine, 0);
            areasMenu.transform.localPosition -= new Vector3(disMenu, 0);
            background.transform.localPosition -= new Vector3(disBack, 0);
            yield return new WaitForSeconds(0.01f);
        }
        rendMenuText(menu);
        selectedMenu = menu;

        isMoving = false;
    }

    private void rendMenuText(Menu menu)
    {
        for(int i = 0; i < btnMenu.Length; i++)
        {
            btnMenu[i].GetComponent<Text>().color = i == (int)menu? selectedColor : defaultColor;
        }
    }

	public void viewMenu1()
    {
        if (isMoving) return;
        StartCoroutine("moveMenu", Menu.menu1);
    }

    public void viewMenu2()
    {
        if (isMoving) return;
        StartCoroutine("moveMenu", Menu.menu2);
    }

    public void viewMenu3()
    {
        if (isMoving) return;
        StartCoroutine("moveMenu", Menu.menu3);
    }

    public void viewMenu4()
    {
        if (isMoving) return;
        StartCoroutine("moveMenu", Menu.menu4);
    }
}
