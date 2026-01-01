using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveWarningUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    [SerializeField] private GameObject bossWarningUI;
    //[SerializeField] private GameObject normalWarning;

    public float duration = 3f;

    private float minAlpha = 0.4f;
    private float maxAlpha = 1.0f;
    private float speed = 1.2f; //±ôºýÀÓ ¼Óµµ

    private void Awake()
    {

        if(canvasGroup == null) 
            canvasGroup = bossWarningUI.AddComponent<CanvasGroup>();
    }
    public void ShowBossWarning()
    {
        StartCoroutine(ShowBossWarningRoutine());
    }
    public IEnumerator ShowBossWarningRoutine()
    {
        canvasGroup.alpha = 1f;

        float time = 0f;

        while (time < duration)
        {
            float ping = Mathf.PingPong(time * 2f, 1f);
            canvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, ping);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
    //public IEnumerator ShowNormalWarningRoutine()
    //{
        //canvasGroup.alpha = 1f;

        //float time = 0f;

        //while (time < duration)
        //{
            //float ping = Mathf.PingPong(time * 2f, 1f);
            //canvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, ping);

            //time += Time.unscaledDeltaTime;
            //yield return null;
        //}

        //canvasGroup.alpha = 0f;
    //}
}
