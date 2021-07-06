using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestroy : MonoBehaviour
{
    public static DonDestroy instance;
    public int ModeSelect;

    void Awake()
    {
        instance = this;

        var obj = FindObjectsOfType<DonDestroy>();
        if(obj.Length==1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
