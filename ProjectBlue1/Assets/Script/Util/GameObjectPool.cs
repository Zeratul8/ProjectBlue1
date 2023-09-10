using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
{
    public delegate T Func();
    //[] 싱글톤에서 해당 스크립트를 미리 메모리에 잡아놓고 거기에 있는 메서드가 필요할때마다 메모리에 접근해서 실행하는 것처럼 delegate에서도 메모리를 미리 잡아놓고 필요할때마다 불러와서 쓴다.
    Queue<T> m_objPool = new Queue<T>();
    int m_count = 0;
    Func m_createFunc;
    //풀에 오브젝트 생성해서 집어넣기
    //count는 풀에 들어갈 오브젝트 갯수, createFunc는 오브젝트 SetActive == true일때 초기 설정
    public GameObjectPool(int count, Func createFunc)
    {
        m_count = count;
        m_createFunc = createFunc;
        Allocate();
    }
    void Allocate()
    {
        for(int i = 0; i< m_count; i++)
        {
            m_objPool.Enqueue(m_createFunc());
        }
    }
    
    public T Get()
    {
        if (m_objPool.Count > 0)
            return m_objPool.Dequeue();
        else
            return m_createFunc();
    }
    public void Set(T obj)
    {
        m_objPool.Enqueue(obj);
    }

}
