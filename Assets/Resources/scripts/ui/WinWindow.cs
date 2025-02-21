using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinWindow : UIWindow
{
    [SerializeField] private TextMeshProUGUI coinTMP;


    public void SetWinAmount(int amount)
    {
        coinTMP.text = "" + amount;
    }
}
