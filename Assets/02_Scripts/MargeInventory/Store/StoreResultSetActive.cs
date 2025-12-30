using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreResultSetActive : MonoBehaviour
{

    [SerializeField] private GameObject turnOnOff;
    [SerializeField] private GameObject resultPanel;

    public void ClickResultPanel()
    {
        turnOnOff.SetActive(false);
        resultPanel.SetActive(true);
    }
    public void ClickGoBack()
    {
        turnOnOff.SetActive(true);
        resultPanel.SetActive(false);
    }
   
}
