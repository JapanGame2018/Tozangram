using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    [HideInInspector] public AudioSource seSource;
    [HideInInspector] public AudioSource bgmSource;
    [SerializeField] AudioClip[] audioClip;

    private void Awake()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        seSource = audioSources[0];
        bgmSource = audioSources[1];
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("SE") || PlayerPrefs.HasKey("BGM"))
        {
            StaticValue.seValue = PlayerPrefs.GetFloat("SE");
            StaticValue.bgmValue = PlayerPrefs.GetFloat("BGM");
        }

        seSource.volume = StaticValue.seValue;
        bgmSource.volume = StaticValue.bgmValue;
    }

    public void PlaySE(int index)
    {
        seSource.PlayOneShot(audioClip[index]);
    }
}

public class StaticValue
{
    public static float seValue = 0.5f;
    public static float bgmValue = 0.5f;
}
