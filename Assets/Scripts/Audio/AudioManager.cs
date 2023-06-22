using System;
using UnityEngine;
using UnityEngine.Audio;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Sequence = DG.Tweening.Sequence;

public class AudioManager : MonoBehaviour
{
    public LevelDataObject levelData;
    private float sfxVolume;
    private float musicVolume;
    private GameData gameData;
    private string _startMusic;
    [SerializeField] private bool _isMenu;

    public Sound[] sounds;

    public string StartMusic { get => _startMusic; set => _startMusic = value; }
    public AnimationCurve testConnerie;

    void Awake()
    {
        gameData = SaveSystem.Load();
        Debug.Log(gameData);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
        }
        UpdateSoundsVolume();
        Debug.Log($"{gameData.musicVolume} && {gameData.sfxVolume}");
    }

    private void Start()
    {
        if(_isMenu)
        {
            Play("Menu");
        }
    }
    //private void OnEnable()
    //{
    //    SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
    //    SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    //}

    //private void SceneManagerOnSceneUnloaded(Scene scene)
    //{
    //    StopAll();
    //    DOTween.KillAll();
    //}

    //private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    DOTween.KillAll();
    //    if(scene.name != "Menu")
    //    {
    //        Play(levelData.levels[levelData.currentSceneIndex].themeName);
    //        Debug.LogError(scene.name);
    //    }
    //    else
    //    {
    //        Play("Menu");
    //        Debug.Log("sdqsdsqdsq");
    //    }
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
    //    SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
    //}

    public void UpdateSoundsVolume()
    {
        foreach (Sound s in sounds)
        {
            switch (s.type)
            {
                case Sound.SoundType.Music:
                    s.source.volume = gameData.musicVolume == true ? s.volume : 0;
                    break;
                case Sound.SoundType.Sfx:
                    s.source.volume = gameData.sfxVolume == true ? s.volume : 0;
                    break;
            }
        }
    }
    public void UpdateSound(string name, float volume, float pitch, bool loop)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return;
        }

        s.source.volume = volume;
        s.source.pitch = pitch;
        s.source.loop = loop;
    }


    public Sound Findsound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return null;
        }

        return s;
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return;
        }
        s.source.Stop();
    }
    public void StopAll()
    {
        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return;
        }
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return;
        }

        s.source.UnPause();
    }

    public void FadeSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return;
        }
        float vol = s.source.volume;
        s.source.volume = 0;
        s.source.DOFade(vol, 2f).SetEase(testConnerie);
    }

    public void Fade(string name, string name2)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        Sound s2 = Array.Find(sounds, sound => sound.name == name2);

        if (s == null || s2 == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return;
        }
        float vol = s2.source.volume;

        s.source.DOFade(0f, 2f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            Stop(name);
            s.source.volume = s.volume;
        });
        Play(name2);
        s2.source.volume = 0f;
        s2.source.DOFade(vol, 2f).SetEase(Ease.InQuint).SetDelay(1f);
    }

    public void FadeToNextMusic(string currentSound, string nextSound)
    {
        Sound s = Array.Find(sounds, sound => sound.name == currentSound);
        Sound s2 = Array.Find(sounds, sound => sound.name == nextSound);

        if (s == null || s2 == null)
        {
            Debug.LogWarning("Sound: " + name + "not Found");
            return;
        }

        //s.source.volume = s.volume;
        //s2.source.volume = s2.volume;

        Play(nextSound);
        Pause(nextSound);
        float vol = s2.source.volume;
        s2.source.volume = 0f;
        Debug.LogWarning("qsqsdqsdqs");
        s.source.DOFade(0f, 1f).SetEase(Ease.OutQuint).OnComplete(() => {
            Stop(currentSound);
            //s.source.volume = s.volume;
            });
        s.source.DOFade(vol, 1f).SetEase(Ease.InQuint).SetDelay(0.5f).OnStart(() => UnPause(nextSound));
    }
}