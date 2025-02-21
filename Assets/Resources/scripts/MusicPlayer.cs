using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    private static MusicPlayer instance; 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip menuAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

        _audioSource = GetComponent<AudioSource>();

        if (PrefsController.Instance.GetMusicValue())
        {
            PlayMusic();
        }
        else
        {
            StopMusic();
        }
    }

    public void PlayMenuMusic()
    {
        if (_audioSource != null)
        {
            AudioClip audioClip = menuAudio;

            _audioSource.Stop();
            _audioSource.clip = audioClip;

            PlayMusic();
        }
    }

    public void ChangeMusic(AudioClip audioClip)
    {
        if (audioClip != null && _audioSource != null)
        {
            _audioSource.Stop();
            _audioSource.clip = audioClip;

            PlayMusic();
        }
    }

    public void PlayMusic(float volume = 1f)
    {
        if (_audioSource == null) return;

        _audioSource.volume = volume;
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        if (!PrefsController.Instance.GetMusicValue())
        {
            StopMusic();
        }
    }

    public void StopMusic()
    {
        if (_audioSource == null) return;

        _audioSource.Pause();
    }
    public static MusicPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicPlayer>();

                if (instance == null)
                {
                    Debug.Log("Create Instance Prefs");
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<MusicPlayer>();
                    singletonObject.AddComponent<AudioSource>();
                    singletonObject.name = typeof(MusicPlayer).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }
}