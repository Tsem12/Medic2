using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private GameObject SfxButton;
    [SerializeField] private GameObject MusicButton;
    [SerializeField] private AllReferences _refs;

    private GameData gameData;

    private Color Greycolor = Color.grey;
    private Color Whitecolor = Color.white;


    private void Awake()
    {
        gameData = SaveSystem.Load();

    }
    private void Start()
    {
        if(SfxButton != null)
        {
            SfxButton.GetComponent<Image>().color = gameData.isSfxMute? Greycolor : Whitecolor;
            MusicButton.GetComponent<Image>().color = gameData.isMusicMute ? Greycolor : Whitecolor;
        }
        
    }

    public void ToggleSfx()
    {
        gameData.isSfxMute = !gameData.isSfxMute;
        SaveSystem.save(gameData);

        SfxButton.GetComponent<Image>().color = gameData.isSfxMute? Greycolor : Whitecolor;
    }

    public void ToggleMusic()
    {
        gameData.isMusicMute = !gameData.isMusicMute;
        SaveSystem.save(gameData);

        MusicButton.GetComponent<Image>().color = gameData.isMusicMute ? Greycolor : Whitecolor;
        if (gameData.isMusicMute)
        {
            _refs.audioManager.Stop("Menu");
        }
        else
        {
            _refs.audioManager.Play("Menu");
        }
    }

    //private void Start()
    //{
    //    SetMusicVolume();

    //}

    //public void SetMusicVolume() { 

    //    gameData.musicVolume = !gameData.musicVolume;
    //    if (gameData.musicVolume)
    //    {
    //        myMixer.SetFloat("Music", 80f);

    //    }
    //    else
    //    {
    //        myMixer.SetFloat("Music", -80f);
    //    }
    //    SaveSystem.save(gameData);
    //}


    //public void SetSfxVolume()
    //{

    //    gameData.musicVolume = !gameData.musicVolume;
    //    if (gameData.musicVolume)
    //    {
    //        myMixer.SetFloat("Sfx", 80f);

    //    }
    //    else
    //    {
    //        myMixer.SetFloat("Sfx", -80f);

    //    }
    //}
}
