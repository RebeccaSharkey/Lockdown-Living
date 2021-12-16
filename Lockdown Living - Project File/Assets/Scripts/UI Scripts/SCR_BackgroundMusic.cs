using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_BackgroundMusic : MonoBehaviour
{
    public static SCR_BackgroundMusic backgroundMusic;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (backgroundMusic)
        {
            Destroy(gameObject);
        }
        else
        {
            backgroundMusic = this;
            DontDestroyOnLoad(gameObject);
            audioSource.Play();
        }
    }

}
