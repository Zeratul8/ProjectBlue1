using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleted_View_Menu : MonoBehaviour
{
    public GameObject[] obj = new GameObject[2]; // ĵ������ ElementView �ڽ� ������Ʈ ( Menus, IMG_MenuBackground ) ������Ʈ ����
    private GameObject[] view_obj; // IMG_MenuBackground�� �ڽ� ������Ʈ���� ������ �迭
    private int index; // ������ �޴� ��ȣ

    void Awake()
    {
        view_obj = new GameObject[obj[1].transform.childCount]; // IMG_MenuBackground�� �ڽ� ������ŭ�� �迭 ����

        // IMG_MenuBackground�� �ڽ� ������Ʈ ����
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

    public void Seleted_Menu(int index) // ������ �޴��� ���̱�
    {
        for(int i = 0; i < view_obj.Length; i++)
        {
           view_obj[i].SetActive(false);
        }
        view_obj[index].SetActive(true);
    }
}
