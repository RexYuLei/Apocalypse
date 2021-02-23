using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class Singleton_Mono<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T m_instance = null;
    public bool DontDestroy;

    public static T Instance
    {
        get { return m_instance; }
    }

    protected virtual void Awake()
    {
        if(m_instance==null)
        {
            m_instance = this as T;
        }
       else
        {
            Destroy(this.gameObject);
        }
        if(DontDestroy)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}