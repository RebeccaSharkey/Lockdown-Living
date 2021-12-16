using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Event : MonoBehaviour
{
    public static SCR_Event eventSystem;

    private void Awake()
    {
        if (eventSystem)
        {
            Destroy(gameObject);
        }
        else
        {
            eventSystem = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
