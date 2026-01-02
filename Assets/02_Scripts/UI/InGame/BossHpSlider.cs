using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BossHpSlider : MonoBehaviour
{
    [SerializeField] private EnemyHp bossHp;
    [SerializeField] private Slider bossHpSlider;
    [SerializeField] private BossData bossData;
    [SerializeField] private TextMeshProUGUI bossName;


    int maxHp;
    int currentHp;

    private void Awake()
    {
        if (bossHpSlider == null)
        {
            bossHpSlider = GetComponent<Slider>();
        }

        if(bossData != null)
        {
            maxHp = bossData.maxHp;
            currentHp = maxHp;
            UpdateBossHp(currentHp, maxHp);            
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

        bossHp= hp;

        if (bossHp != null)
            bossHp.SubscribeEnemyTakeDamageEvent(UpdateBossHp);

        if(bossHpSlider != null)
            bossHpSlider.value = 1f;

        if (bossName != null)
            bossName.text = bossData.bossName;
    }
    public void UpdateBossHp(int bossCurrentHp, int bossMaxHp)
    {
        if (bossHpSlider == null) return;
        if (bossMaxHp <= 0) return;

        float targetSliderValue = (float)bossCurrentHp / bossMaxHp;
        bossHpSlider.value = Mathf.Clamp01(targetSliderValue);

        if(bossCurrentHp <=0)
            gameObject.SetActive(false);
    }
}