using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new()
{
    static T m_instance = null;
    public static T Instance { get { return m_instance; } private set { m_instance = value; } }
    static Singleton()
    {
        if (Instance == null)
            Instance = new T();
    }
}
