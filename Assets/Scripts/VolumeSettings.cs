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
        SetMusicVolume();

    }

    public void SetMusicVolume() { 
    
        gameData.musicVolume = !gameData.musicVolume;
        if (gameData.musicVolume)
        {
            myMixer.SetFloat("Music", 80f);

        }
        else
        {
            myMixer.SetFloat("Music", -80f);
        }
        SaveSystem.save(gameData);
    }


    public void SetSfxVolume()
    {

        gameData.musicVolume = !gameData.musicVolume;
        if (gameData.musicVolume)
        {
            myMixer.SetFloat("Sfx", 80f);

        }
        else
        {
            myMixer.SetFloat("Sfx", -80f);

        }
    }
}
