using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinTMP;
    
    void Update()
    {
        coinTMP.text = "" + PrefsController.Instance.GetCoinCount();
    }
}
