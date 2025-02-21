using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private List<LocationDate> _locationDates;
    [SerializeField] private Image nextButton;
    [SerializeField] private Image previsionButton;

    [SerializeField] private Image secondLocationBackground;
    [SerializeField] private Image locationBackground;
    [SerializeField] private Image playButton;
    [SerializeField] private TextMeshProUGUI playButtonTMP;
    [SerializeField] private TextMeshProUGUI titleTMP;
    [SerializeField] private TextMeshProUGUI modeTMP;
    
    private int currentLocation = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        MusicPlayer.Instance.PlayMenuMusic();
        LocationDate locationDate = _locationDates.Find(it => it.id == PrefsController.Instance.GetCurrentLocation());
        if (locationDate == null)
        {
            locationDate = _locationDates[0];
            currentLocation = 0;
        }
        else
        {
           currentLocation =  _locationDates.FindIndex(it => it == locationDate);
        }
        ReloadButton();
        ReloadUI();
        InitAnimation();
    }

    public void StartBattle()
    {
        PrefsController.Instance.SetCurrentLocation(_locationDates[currentLocation].id);
        SceneManager.LoadScene("GameScene");
    }

    public void NextLocation()
    {
        currentLocation = Mathf.Clamp(++currentLocation, 0, _locationDates.Count - 1);
        secondLocationBackground.sprite = locationBackground.sprite;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(secondLocationBackground.transform.DOLocalMove(new Vector3(0f, 0f), 0f));
        sequence.Join(locationBackground.transform.DOLocalMove(new Vector3(420f, 0f), 0f));

        sequence.Append(secondLocationBackground.transform.DOLocalMove(new Vector3(-420f, 0f), 1f));
        sequence.Join(locationBackground.transform.DOLocalMove(new Vector3(0f, 0f),1f));
        
        ReloadButton();
        ReloadUI();
    }

    public void PrevisionLocation()
    {
        currentLocation = Mathf.Clamp(--currentLocation, 0, _locationDates.Count - 1);
        
        secondLocationBackground.sprite = locationBackground.sprite;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(secondLocationBackground.transform.DOLocalMove(new Vector3(0f, 0f), 0f));
        sequence.Join(locationBackground.transform.DOLocalMove(new Vector3(-420f, 0f), 0f));

        sequence.Append(secondLocationBackground.transform.DOLocalMove(new Vector3(420f, 0f), 1f));
        sequence.Join(locationBackground.transform.DOLocalMove(new Vector3(0f, 0f),1f));
        
        ReloadButton();
        ReloadUI();
    }

    public void ReloadUI()
    {
        if (currentLocation < 0 || currentLocation >= _locationDates.Count)
        {
            return;
        }
        LocationDate locationDate = _locationDates[currentLocation];
        locationBackground.sprite = locationDate.background;
        titleTMP.text = locationDate.title;
        modeTMP.text = locationDate.modeTxt;

        int currentLevel = PrefsController.Instance.GetMaxCurrentLocationLevel(locationDate.id);
        if (currentLevel > locationDate.levelList.Count)
        {
            PrefsController.Instance.SetMaxCurrentLocationLevel(locationDate.id,locationDate.levelList.Count);
            playButtonTMP.text = "Level-" + (locationDate.levelList.Count + 1);
        }
        else
        {
            playButtonTMP.text = "Level-" + (currentLevel + 1);
        }
    }
    public void ReloadButton()
    {
        previsionButton.gameObject.SetActive(currentLocation > 0);
        nextButton.gameObject.SetActive(currentLocation < _locationDates.Count - 1);
    }

    private void InitAnimation()
    {
        Vector3 correctPlayButtonLocalPosition = playButton.transform.localPosition;
        Vector3 correctNextLocalPosition = nextButton.transform.localPosition;
        Vector3 correctPrevisionLocalPosition = previsionButton.transform.localPosition;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(playButton.transform.DOLocalMove(new Vector3(
            correctPlayButtonLocalPosition.x, correctPlayButtonLocalPosition.y - 100f,
            correctPlayButtonLocalPosition.z
        ), 0f));
        sequence.Join(nextButton.transform.DOLocalMove(new Vector3(
            correctPlayButtonLocalPosition.x, correctPlayButtonLocalPosition.y - 100f,
            correctPlayButtonLocalPosition.z
        ), 0f));
        sequence.Join(previsionButton.transform.DOLocalMove(new Vector3(
            correctPlayButtonLocalPosition.x, correctPlayButtonLocalPosition.y - 100f,
            correctPlayButtonLocalPosition.z
        ), 0f));
        
        sequence.Append(playButton.transform.DOLocalMove(new Vector3(
            correctPlayButtonLocalPosition.x, correctPlayButtonLocalPosition.y ,
            correctPlayButtonLocalPosition.z
        ), 0.5f));
        sequence.Join(nextButton.transform.DOLocalMove(new Vector3(
            correctPlayButtonLocalPosition.x, correctPlayButtonLocalPosition.y ,
            correctPlayButtonLocalPosition.z
        ), 0.5f));
        sequence.Join(previsionButton.transform.DOLocalMove(new Vector3(
            correctPlayButtonLocalPosition.x, correctPlayButtonLocalPosition.y ,
            correctPlayButtonLocalPosition.z
        ), 0.5f));
        
        sequence.Append(nextButton.transform.DOLocalMove(new Vector3(
            correctNextLocalPosition.x, correctNextLocalPosition.y ,
            correctNextLocalPosition.z
        ), 0.5f));
        sequence.Join(previsionButton.transform.DOLocalMove(new Vector3(
            correctPrevisionLocalPosition.x, correctPrevisionLocalPosition.y ,
            correctPrevisionLocalPosition.z
        ), 0.5f));
    }
}
