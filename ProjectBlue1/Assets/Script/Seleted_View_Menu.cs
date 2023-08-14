using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleted_View_Menu : MonoBehaviour
{
    public GameObject[] obj = new GameObject[2];
    public GameObject[] view_obj = new GameObject[4];
    public int index;

    void Awake()
    {
        for (int i = 0; i < view_obj.Length; i++)
        {
            view_obj[i] = obj[1].transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < view_obj.Length; i++)
        {
            if (i != 0) view_obj[i].SetActive(false);
        }
    }

    public void Seleted_Menu(int index)
    {
        for(int i = 0; i < view_obj.Length; i++)
        {
            if (i == index) view_obj[i].SetActive(true);
            else view_obj[i].SetActive(false);
        }
    }
}
