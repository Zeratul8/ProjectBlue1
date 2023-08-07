using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pooling : MonoBehaviour
{
    public static Pooling instance;

    public GameObject prefab;

    private Queue<GameObject> objs = new Queue<GameObject>();
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
            objs.Enqueue(obj);
        }
    }
    public GameObject GetPoolItem()
    {
        GameObject obj = objs.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public void ReturnPoolItem(GameObject obj)
    {
        obj.SetActive(false);
        objs.Enqueue(obj);
    }
}
