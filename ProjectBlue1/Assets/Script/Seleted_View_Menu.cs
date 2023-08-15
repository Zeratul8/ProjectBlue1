using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleted_View_Menu : MonoBehaviour
{
    public GameObject[] obj = new GameObject[2]; // 캔버스의 ElementView 자식 오브젝트 ( Menus, IMG_MenuBackground ) 오브젝트 삽입
    private GameObject[] view_obj; // IMG_MenuBackground의 자식 오브젝트들을 삽입할 배열
    private int index; // 선택한 메뉴 번호

    void Awake()
    {
        view_obj = new GameObject[obj[1].transform.childCount]; // IMG_MenuBackground의 자식 개수만큼만 배열 생성

        // IMG_MenuBackground의 자식 오브젝트 삽입
        int idx = 0;
        foreach (Transform t in obj[1].transform)
        {
            view_obj[idx] = t.gameObject;
            idx++;
        }

        for (int i = 0; i < view_obj.Length; i++)
        {
            if (view_obj[i].name != "Status")
            {
                view_obj[i].SetActive(false);
            }
        }
    }

    public void Seleted_Menu(int index) // 선택한 메뉴만 보이기
    {
        for(int i = 0; i < view_obj.Length; i++)
        {
           view_obj[i].SetActive(false);
        }
        view_obj[index].SetActive(true);
    }
}
