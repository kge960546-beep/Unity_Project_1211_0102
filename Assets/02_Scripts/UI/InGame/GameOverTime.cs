using TMPro;
using UnityEngine;

public class GameOverTime : MonoBehaviour
{
    [Header("최고기록 UI 연결")]
    [SerializeField] private TextMeshProUGUI gameOverTimeText;
    [SerializeField] private TextMeshProUGUI stageBestTime;

    [Header("외부 연결")]
    [SerializeField] private TimeService timeService;

    private string saveKey = "Stage1_BestTime";

    void Start()
    {
        if(gameOverTimeText == null)
            gameOverTimeText = GetComponent<TextMeshProUGUI>();
        if(timeService == null)
            timeService = GameManager.Instance.GetService<TimeService>();

        GetGameOverTime();
    }
    public void GetGameOverTime()
    {
        float currentTime = 0;

        if (gameOverTimeText == null) return;
        if(GameManager.Instance != null)
        {
            currentTime = timeService.accumulatedDeltaTime;
        }        

        float bestTime = PlayerPrefs.GetFloat(saveKey, 0);

        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat(saveKey, bestTime); 
            PlayerPrefs.Save();
        }

        if(stageBestTime != null)
        {
            int bestmin = (int)bestTime / 60;
            int bestsec = (int)bestTime % 60;

            stageBestTime.text = string.Format($"최고 {bestmin:00}:{bestsec:00}");
        }
    }
}