using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGame : MonoBehaviour
{
    [SerializeField] private TimeService timeService;
    [SerializeField] private GameObject pausePanel;

    private void Awake()
    {
        if(timeService == null)
        {
            if (GameManager.Instance != null)
            {
                timeService = GameManager.Instance.GetService<TimeService>();
            }
            else
            {
                timeService = FindAnyObjectByType<TimeService>();
            }
        }

        if (pausePanel == null)
        {
            Debug.Log("오브젝트가 없습니다");
            return;
        }
    }
    public void ContinuePlay()
    {
        timeService.ResumeGame();
        pausePanel.SetActive(false);
    }
}