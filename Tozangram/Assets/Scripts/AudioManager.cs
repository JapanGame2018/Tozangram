using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] AudioClip[] audioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("SE") || PlayerPrefs.HasKey("BGM"))
        {
            StaticValue.seValue = PlayerPrefs.GetFloat("SE");
            StaticValue.bgmValue = PlayerPrefs.GetFloat("BGM");
        }

        audioSource.volume = StaticValue.bgmValue;
    }

    public void PlaySE(int index)
    {
        audioSource.PlayOneShot(audioClip[index], StaticValue.seValue);
    }
}

public class StaticValue
{
    public static float seValue = 0.5f;
    public static float bgmValue = 0.5f;
}
