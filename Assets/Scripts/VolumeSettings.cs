using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private GameObject SfxButton;
    [SerializeField] private GameObject MusicButton;

    public GameData gameData;

    private Color Greycolor = Color.grey;
    private Color Whitecolor = Color.white;


    private void Awake()
    {
        gameData = SaveSystem.Load();

    }

    private void Start()
    {
      //  SetMusicVolume();

    }

    public void SetMusicVolume() {
        gameData.musicVolume = !gameData.musicVolume;
        SaveSystem.save(gameData);
    }


    public void SetSfxVolume()
    {
        gameData.sfxVolume = !gameData.sfxVolume;
    }
}
