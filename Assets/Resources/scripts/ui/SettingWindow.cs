using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : UIWindow
{
    [SerializeField] private Sprite buttonOn;
    [SerializeField] private Sprite buttonOff;

    [SerializeField] private Image musicSwitcher;
    [SerializeField] private Image vibrationSwitcher;
    // Start is called before the first frame update
    void Start()
    {
        ReloadUI();
    }
    

    public void SwitchMusic()
    {
        PrefsController.Instance.SwitchMusic();
        ReloadUI();
        if (PrefsController.Instance.GetMusicValue())
        {
            MusicPlayer.Instance.PlayMusic();
        }
        else
        {
            MusicPlayer.Instance.StopMusic();
        }
    }

    public void SwitchVibration()
    {
        PrefsController.Instance.SwitchVibration();
        ReloadUI();
    }

    private void ReloadUI()
    {
        if (PrefsController.Instance.GetMusicValue())
        {
            musicSwitcher.sprite = buttonOn;
        }
        else
        {
            musicSwitcher.sprite = buttonOff;
        }

        if (PrefsController.Instance.GetVibrationValue())
        {
            vibrationSwitcher.sprite = buttonOn;
        }
        else
        {
            vibrationSwitcher.sprite = buttonOff;
        }
    }
}
