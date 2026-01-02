using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    public Slider slider;
    private BossData bossData;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Bind(BossData data)
    {
        if (data == null)
        {
            Debug.LogError("BossHPUI.Bind() : BossData is NULL");
            return;
        }

        bossData = data;
        slider.maxValue = bossData.maxHp;
        slider.value = bossData.currentHp;

        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (bossData == null)
            return;

        slider.value = bossData.currentHp;

        if(bossData.currentHp <= 0)
            gameObject.SetActive(false);
    }
}
