using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public GameData gameData;

    private void Awake()
    {
        gameData = SaveSysteme.Load();
        musicSlider.value = gameData.musicVolume;
        sfxSlider.value = gameData.sfxVolume;
    }

    private void Start()
    {
        SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        gameData.musicVolume = volume;
        SaveSysteme.save(gameData);
    }


    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
        gameData.sfxVolume = volume;
        SaveSysteme.save(gameData);
    }
}
