using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pooling : MonoBehaviour
{
    public static Pooling instance;

    public GameObject prefab;

    private List<GameObject> objs = new List<GameObject>();
    private int maxSize = 5;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }
    public void Init()
    {
        for(int i = 0; i < maxSize; i++)
        {
            GameObject obj = Instantiate(prefab, gameObject.transform);
            obj.SetActive(false);
            objs.Add(obj);
        }
    }
    // 사용 가능한 오브젝트 얻기
    public GameObject Get_Item()
    {
        foreach(GameObject obj in objs)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
    // 사용이 끝난 오브젝트 풀로 반환
    public void Return_Item(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(false);
    }
}
