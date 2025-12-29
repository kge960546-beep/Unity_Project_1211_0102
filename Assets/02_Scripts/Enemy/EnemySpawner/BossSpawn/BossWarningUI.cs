using System.Collections;
using TMPro;
using UnityEngine;

public class BossWarningUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Awake()
    {
        if(canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    public IEnumerator ShowCountdown(int sec)
    {
        canvasGroup.alpha = 1f;

        for (int i = sec; i > 0; i--)
        {
            if(countdownText != null)
                countdownText.text = i.ToString();

            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
            countdownText.text = "BOSS APPROACHING";

        yield return new WaitForSeconds(1f);

        canvasGroup.alpha = 0f;
    }
}
