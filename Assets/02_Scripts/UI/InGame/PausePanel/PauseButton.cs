using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] TimeService timeService;
    
    void Start()
    {
        if(pausePanel == null)
            pausePanel = GetComponent<GameObject>();

        if(GameManager.Instance == null)
            timeService = FindAnyObjectByType<TimeService>();
    }
    public void ActivePausePanel()
    {
        pausePanel.SetActive(true);
        timeService.PauseGame();
    }
   
}
