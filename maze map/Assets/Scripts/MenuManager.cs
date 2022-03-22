using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;//다른 class에서도 호출가능

    [SerializeField] Menu[] menus;//SerializedField를 사용하면 우리는 public처럼 쓸 수 있지만  public이 아니여서 외부에서는 못만짐.

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)//string을 받아서 해당이름 가진 메뉴를 여는 스크립트
            {
                menus[i].Open();//오픈 메뉴(스트링)에 있는 for문이 오픈 메뉴(메뉴)에도 똑같이 있어서 중복을 피하고자 코드 수정.  
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}