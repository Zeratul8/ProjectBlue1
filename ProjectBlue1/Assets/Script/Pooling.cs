using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Pooling : MonoBehaviour
{
    // Ǯ�� ��� ���
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
    // ��� ������ ������Ʈ ���
    public Object Get_Element()
    { 
        Object _element = pooling_list[^1]; // ������ ��� ��������
        pooling_list.Remove(_element);
        return _element;
    }
    // ����� ���� ������Ʈ Ǯ�� ��ȯ
    public void Returned_Element(Object element)
    {
        pooling_list.Add(element);
    }
    // �� �̻� �ʿ����� ���� �� Ǯ �ı�
    public void Destroy_Pool()
    {
        Destroy(gameObject);
    }
}
