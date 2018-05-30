using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OptionValueManager : MonoBehaviour
{

    Slider seSlider;
    Slider bgmSlder;
    AudioManager am;

    private void Start()
    {
        seSlider = GameObject.Find("SESlider").GetComponent<Slider>();
        bgmSlder = GameObject.Find("BGMSlider").GetComponent<Slider>();
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (PlayerPrefs.HasKey("SE") || PlayerPrefs.HasKey("BGM"))
        {
            StaticValue.seValue = PlayerPrefs.GetFloat("SE");
            StaticValue.bgmValue = PlayerPrefs.GetFloat("BGM");
        }

        seSlider.value = StaticValue.seValue;
        bgmSlder.value = StaticValue.bgmValue;
    }

    public void changeSEValue(float value)
    {
        am.seSource.volume = value;
        StaticValue.seValue = value;
    }

    public void changeBGMValue(float value)
    {
        am.bgmSource.volume = value;
        StaticValue.bgmValue = value;
    }
}
