using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoLobby : MonoBehaviour
{
    TimeService timeService;
    private void Start()
    {
        if(timeService == null)
            timeService = GameManager.Instance.GetService<TimeService>();
    }
    public void GoHomeLoad()
    {
        timeService.ResumeGame();        
        SceneTransitionManager.Instance.LoadScene("Lobby");
    }
}
