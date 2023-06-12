using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;

    public GameData gameData;

    private void Awake()
    {
        gameData = SaveSystem.Load();

    }

    private void Start()
    {
        SetMusicVolume();
    }

    public void SetMusicVolume() { 
    
        gameData.musicVolume = !gameData.musicVolume;
        SaveSystem.save(gameData);
    }


    public void SetSfxVolume()
    {

        gameData.sfxVolume = !gameData.sfxVolume;
        SaveSystem.save(gameData);
    }
}
