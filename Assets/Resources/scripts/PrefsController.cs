using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsController : MonoBehaviour
{
    private static PrefsController _instance;
    private static bool isCreated = false;
    
    public int GetAdsRewardBonusId()
    {
        return PlayerPrefs.GetInt("AdsRewardBonusId", 0);
    }
    public void SetAdsRewardBonusId(int id)
    {
        PlayerPrefs.SetInt("AdsRewardBonusId", id);
        PlayerPrefs.Save();
    }
    public void SetCompleteLevel(int completeLevel)
    {
        if (GetMaxLevel() < (completeLevel + 1))
        {
            PlayerPrefs.SetInt("MaxOpenLevel",completeLevel + 1);
            PlayerPrefs.Save();
        }
    }

    public bool IsSkinBuyed(int skinId)
    {
        return PlayerPrefs.GetInt("SkinBuy-" + skinId, 0) == 1;
    }

    public void BuySkin(int skinId)
    {
        PlayerPrefs.SetInt("SkinBuy-" + skinId, 1);
        PlayerPrefs.Save();
    }
    public int GetCurrentBlockSkin()
    {
        return PlayerPrefs.GetInt("CurrentBlockSkin", 0);
    }

    public void SetCurrentBlockSkin(int skinId)
    {
        PlayerPrefs.SetInt("CurrentBlockSkin",skinId);
        PlayerPrefs.Save();
    }
    
    public int GetCurrentLocation()
    {
        return PlayerPrefs.GetInt("CurrentLocation", 0);
    }

    public void SetCurrentLocation(int locationId)
    {
        PlayerPrefs.SetInt("CurrentLocation",locationId);
        PlayerPrefs.Save();
    }
    public int GetMaxCurrentLocationLevel(int locationId)
    {
        return PlayerPrefs.GetInt("CurrentLocationLevel-" + locationId, 0);
    }

    public void SetMaxCurrentLocationLevel(int locationId, int level)
    {
        PlayerPrefs.SetInt("CurrentLocationLevel-" + locationId,level);
        PlayerPrefs.Save();
    }

    public int GetMaxLevel()
    {
        return PlayerPrefs.GetInt("MaxOpenLevel", 0);
    }

    public int GetCoinCount()
    {
        return PlayerPrefs.GetInt("coinCount", 0);
    }
    public void SetCoinCount(int value)
    {
        PlayerPrefs.SetInt("coinCount",value);
        PlayerPrefs.Save();
    }


    public bool GetMusicValue()
    {
        return PlayerPrefs.GetInt("Music", 1) == 1;
    }

    public bool SwitchMusic()
    {
        if (GetMusicValue())
        {
            PlayerPrefs.SetInt("Music",0);
        }
        else
        {
            PlayerPrefs.SetInt("Music",1);
        }
        PlayerPrefs.Save();
        return GetMusicValue();
    }
    public bool GetVibrationValue()
    {
        return PlayerPrefs.GetInt("Vibration", 1) == 1;
    }

    public bool SwitchVibration()
    {
        if (GetVibrationValue())
        {
            PlayerPrefs.SetInt("Vibration",0);
        }
        else
        {
            PlayerPrefs.SetInt("Vibration",1);
        }
        PlayerPrefs.Save();
        return GetVibrationValue();
    }
    public static PrefsController Instance
    {
        get
        {
            if (_instance == null)
            {
                if (isCreated)
                {
                    Debug.Log("Create Instance !!!!!!");

                    return null;
                }
                _instance = FindObjectOfType<PrefsController>();

                if (_instance == null)
                {
                    isCreated = true;
                    Debug.Log("Create Instance Prefs");
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<PrefsController>();
                    singletonObject.name = typeof(PrefsController).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }
    
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static bool IsAlive()
    {
        return _instance != null;
    }
}
