using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;   

    private TimeService timeService;
    private float survialTime;
    
    private void Awake()
    {
        if(GameManager.Instance != null)
        {
            timeService = GameManager.Instance.GetService<TimeService>();
        }        

        survialTime = 0;
    }
    private void Start()
    {       
        StartCoroutine(StartTimer());
    }
    IEnumerator StartTimer()
    {
        int minute;
        int second;

        while (true) 
        {
            survialTime = timeService.accumulatedDeltaTime;

            minute = (int)survialTime / 60;
            second = (int)survialTime % 60;

            //TODO: 최적화 가능
            timeText.SetText("{0:00}:{1:00}", minute,second);

            yield return null;
        }
    }
}










/*
 void Start()
    {
        timeService.ResetTime();
        survialTime = 0.0f;
    }
   
    void Update()
    {        
        int totalSeconds = Mathf.FloorToInt(timeService.accumulatedDeltaTime);
        TimeUpdateText(totalSeconds);
    }
    private void TimeUpdateText(int totalSeconds)
    {
        int minute = totalSeconds / 60;
        int second = totalSeconds % 60;
        timeText.text = $"{minute:00}:{second:00}";
    }
 
 
 */
