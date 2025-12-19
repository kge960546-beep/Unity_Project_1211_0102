using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    public TextMeshProUGUI killCount;

    private int currentKillCount = 0;
    void Start()
    {
        KillCountRenewal();
    }
    public void AddKillCount(int value)
    {
        currentKillCount += value;
        KillCountRenewal();        
    }
    public void KillCountRenewal()
    {
        killCount.text = currentKillCount.ToString();
    }
}