using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLocationDate", menuName = "Level/LocationDate")]
public class LocationDate : ScriptableObject
{
    public int id;
    public string title;
    public string modeTxt;
    public Sprite background;
    public int winAmount;
    public AudioClip gameAudio;
    public List<LevelDate> levelList;
    
    
}
