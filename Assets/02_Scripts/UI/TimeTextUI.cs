using System.Timers;
using TMPro;
using UnityEngine;

public class TimeTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    

    private float survialTime;
    
    private void Awake()
    {
        GameManager.Instance.GetService<TimeService>();
    }
    void Start()
    {
        survialTime = 0.0f;
    }
   
    void Update()
    {
        survialTime += Time.deltaTime;
        timeText.text = survialTime.ToString();
        
    }
}
