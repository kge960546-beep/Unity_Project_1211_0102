using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour
{
    [SerializeField] private Slider expBarSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    ExperienceService es;

    private void Awake()
    {
        es = GameManager.Instance.GetService<ExperienceService>();        
    }

    private void LateUpdate()
    {
        if (es == null) return;

        expBarSlider.normalizedValue = (float)es.exp / es.nextLevelExp;
        levelText.text = $"LV. {es.level}";
    }
}
