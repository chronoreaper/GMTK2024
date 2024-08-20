using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Inst { get; private set; }
    public AudioSource SFX;
    public AudioSource Music;
    // Start is called before the first frame update
    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(this);
        }
        else
        {
            Inst = this;
        }
        //DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        Music.clip = clip;
        Music.Play();
    }
}
