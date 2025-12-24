using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BossHpSlider : MonoBehaviour
{
    [Header("PlayerHP")]
    public int BossMaxHp;
    public int BossCurrentHp;

    [SerializeField] private EnemyHp bossHp;
    [SerializeField] private Slider bossHpSlider;
    [SerializeField] private EnemyData bossDataSo;
    [SerializeField] private TextMeshProUGUI bossName;

    private void Awake()
    {
        if (bossHpSlider == null)
        {
            bossHpSlider = GetComponent<Slider>();
        }

        if(bossDataSo != null)
        {
            BossMaxHp = bossDataSo.maxHp;
            BossCurrentHp = BossMaxHp;
            UpdateBossHp(BossCurrentHp, bossDataSo.maxHp);            
        }
    }
    private void OnEnable()
    {
        if (bossHp != null)
            bossHp.SubscribeEnemyTakeDamageEvent(UpdateBossHp);
    }
    private void OnDisable()
    {
        if (bossHp != null)
            bossHp.UnsubscribeEnemyTakeDamageEvent(UpdateBossHp);
    }    
    public void BindingBoss(EnemyHp hp)
    {
        if(bossHp != null)
            bossHp.UnsubscribeEnemyTakeDamageEvent(UpdateBossHp);

        bossHp = hp;

        if (bossHp != null)
            bossHp.SubscribeEnemyTakeDamageEvent(UpdateBossHp);

        if(bossHpSlider != null)
            bossHpSlider.value = 1f;

        if (bossName != null)
            bossName.text = "°õ";
    }
    public void UpdateBossHp(int currentHp, int maxHp)
    {
        if (bossHpSlider == null) return;
        if (maxHp <= 0) return;

        BossMaxHp = maxHp;
        BossCurrentHp = currentHp;

        float targetSliderValue = (float)currentHp / maxHp;
        bossHpSlider.value = Mathf.Clamp01(targetSliderValue);
    }
}