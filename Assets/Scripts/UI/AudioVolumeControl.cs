using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeControl : MonoBehaviour
{
    [Header("Volume UI")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("VolumeMaster", 100f));
        SetMusicVolume(PlayerPrefs.GetFloat("VolumeMusic", 100f));
        SetSFXVolume(PlayerPrefs.GetFloat("VolumeSFX", 100f));
    }

    private void SetMusicVolume(float volume)
    {
        if (volume < 1)
        {
            volume = 0.01f;
        }

        musicVolumeSlider.value = volume;
        musicVolumeText.text = $"{volume:0}%";
        PlayerPrefs.SetFloat("VolumeMusic", volume);
        mixer.SetFloat("VolumeMusic", Mathf.Log10(volume / 100) * 20f);
    }

    private void SetSFXVolume(float volume)
    {
        if (volume < 1)
        {
            volume = 0.01f;
        }

        sfxVolumeSlider.value = volume;
        sfxVolumeText.text = $"{volume:0}%";
        PlayerPrefs.SetFloat("VolumeSFX", volume);
        mixer.SetFloat("VolumeSFX", Mathf.Log10(volume / 100) * 20f);
    }
    
    private void SetMasterVolume(float volume)
    {
        if (volume < 1)
        {
            volume = 0.01f;
        }

        masterVolumeSlider.value = volume;
        masterVolumeText.text = $"{volume:0}%";
        PlayerPrefs.SetFloat("VolumeMaster", volume);
        mixer.SetFloat("VolumeMaster", Mathf.Log10(volume / 100) * 20f);
    }
    
    public void SetMusicVolumeFromSlider() => SetMusicVolume(musicVolumeSlider.value);
    
    public void SetSFXVolumeFromSlider() => SetSFXVolume(sfxVolumeSlider.value);
    
    public void SetMasterVolumeFromSlider() => SetMasterVolume(masterVolumeSlider.value);
}
