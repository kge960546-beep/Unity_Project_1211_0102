using System.Collections;
using TMPro;
using UnityEngine;

public class TimeTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    private TimeService timeService;
    public float survialTime;
    
    
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
    public float GetBestTIme()
    {
        return survialTime;
    }
}