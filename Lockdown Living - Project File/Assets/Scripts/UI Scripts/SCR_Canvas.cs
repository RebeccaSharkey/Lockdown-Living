using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Canvas : MonoBehaviour
{
    public static SCR_Canvas canvas;

    private void Awake()
    {
        if(canvas)
        {
            Destroy(gameObject);
        }
        else
        {
            canvas = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
