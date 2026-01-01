using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSliderBar : MonoBehaviour
{
    [Header("PlayerHP")]
    public int playerMaxHp;
    public int playerCurrentHp;

    public PlayerHp playerHp;
    public Slider playerHpSlider;
    public PlayerDataSO playerDataSo;

    void Start()
    {
        if (playerHpSlider == null)
        {
            Debug.Log("비어있습니다");
            return;
        }
        playerMaxHp = playerDataSo.playerMaxHp;
        playerCurrentHp = playerMaxHp;

        playerHp = FindAnyObjectByType<PlayerHp>();

        UpdatePlayerHp(playerCurrentHp, playerDataSo.playerMaxHp);
    }
    private void OnEnable()
    {
        if (playerHp == null)
            playerHp = FindAnyObjectByType<PlayerHp>();

        playerHp.SubscribePlayerTakeDamageEvent(UpdatePlayerHp);
    }
    private void OnDisable()
    {
        if (playerHp == null) return;
        playerHp.UnsubscribePlayerTakeDamageEvent(UpdatePlayerHp);
    }   
    #region playerHpSlider
    public void UpdatePlayerHp(int currentHp, int maxHp)
    {
        if (playerHpSlider == null) return;
        if (maxHp <= 0) return;

        playerMaxHp = maxHp;
        playerCurrentHp = currentHp;

        float targetSliderValue = (float)currentHp / maxHp;
        playerHpSlider.value = Mathf.Clamp01(targetSliderValue);
    }
    #endregion
}
