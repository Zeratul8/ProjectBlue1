using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Pooling : MonoBehaviour
{
    // 풀에 담길 요소
    public Object element;

    private List<Object> pooling_list = new List<Object>();
    public int maxSize = 5;

    public void Init()
    {
        for(int i = 0; i < maxSize; i++)
        {
            pooling_list.Add(element);
        }
    }
    // 사용 가능한 오브젝트 얻기
    public Object Get_Element()
    { 
        Object _element = pooling_list[^1]; // 마지막 요소 가져오기
        pooling_list.Remove(_element);
        return _element;
    }
    // 사용이 끝난 오브젝트 풀로 반환
    public void Returned_Element(Object element)
    {
        pooling_list.Add(element);
    }
    // 더 이상 필요하지 않을 때 풀 파괴
    public void Destroy_Pool()
    {
        Destroy(gameObject);
    }
}
