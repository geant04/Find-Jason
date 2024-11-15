using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioClips : MonoBehaviour
{
    public static AudioClips Instance { get; private set; }

    public AudioClip charge;
    public AudioClip cameraReady;
    public AudioClip cameraClick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
