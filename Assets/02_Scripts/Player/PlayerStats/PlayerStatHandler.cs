using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerBaseDataSO;

    [SerializeField] PlayerHp playerHp;
    [SerializeField] private bool isLobby = false;

    [SerializeField] private EquipmentInventory eqInventory;
    [SerializeField] private InventoryManager inventoryManager;    

    [SerializeField] private TextMeshProUGUI finalAtkText;
    [SerializeField] private TextMeshProUGUI finalHpText;

    [SerializeField] private PlayerStat maxHpStat;
    [SerializeField] private PlayerStat attackStat;    

    private Coroutine refreshCo;

    private void Awake()
    {
        if(playerHp == null) playerHp = GetComponent<PlayerHp>();
        
        if(maxHpStat == null || attackStat == null)
        {
            var stats = GetComponents<PlayerStat>();
            
            for(int i = 0; i < stats.Length; i++)
            {
                var st = stats[i];
                if (st == null) continue;

                if (st.StatType == PlayerStatType.Hp) maxHpStat = st;
                else if(st.StatType == PlayerStatType.Attack) attackStat = st;
            }
        }
        if (inventoryManager == null) inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null) inventoryManager = FindAnyObjectByType<InventoryManager>();

        if (eqInventory == null) eqInventory = FindAnyObjectByType<EquipmentInventory>();
    }
    private void Start()
    {
        PlayerStatsUpdate();
    }
    private void OnEnable()
    {
        if (eqInventory != null)
            eqInventory.SubscribeOnEquipmentChange(PlayerStatsUpdate);

        if (inventoryManager != null)
            inventoryManager.SubscribeOnInventoryChanged(PlayerStatsUpdate);

        if (refreshCo != null) StopCoroutine(refreshCo);
        refreshCo = StartCoroutine(RefreshLater());
    }
    private void OnDisable()
    {
        if (eqInventory != null)
            eqInventory.UnsubscribeOnEquipmentChange(PlayerStatsUpdate);

        if (inventoryManager != null)
            inventoryManager.UnsubscribeOnInventoryChanged(PlayerStatsUpdate);
    }   
    IEnumerator RefreshLater()
    {
        yield return new WaitForEndOfFrame();
        PlayerStatsUpdate();
    }

    //장착 상태를 보고 계산후 UI 출력
    public void PlayerStatsUpdate()
    {
        if (playerBaseDataSO == null && playerHp == null)
            playerBaseDataSO = playerHp.playerBaseData;

        if (playerBaseDataSO == null) return;
        if (maxHpStat == null || attackStat == null) return;
        if (isLobby && (inventoryManager == null || eqInventory == null)) return;

        maxHpStat.baseValue = playerBaseDataSO.playerMaxHp;
        attackStat.baseValue = playerBaseDataSO.playerAttack;

        maxHpStat.isRecalculate = false;
        attackStat.isRecalculate = false;

        maxHpStat.RemoveAllModifier(this);
        attackStat.RemoveAllModifier(this);

        PartsInstallation(EquipmentSO.EquipmentPart.Weapon);
        PartsInstallation(EquipmentSO.EquipmentPart.Armor);
        PartsInstallation(EquipmentSO.EquipmentPart.Necklace);
        PartsInstallation(EquipmentSO.EquipmentPart.Belt);
        PartsInstallation(EquipmentSO.EquipmentPart.Gloves);
        PartsInstallation(EquipmentSO.EquipmentPart.Shoes);

        maxHpStat.isRecalculate = true;
        attackStat.isRecalculate = true;

        int finalHp = Mathf.RoundToInt(maxHpStat.CalculatedValue);
        int finalAtk = Mathf.RoundToInt(attackStat.CalculatedValue);

        maxHpStat.UpdateStats();
        attackStat.UpdateStats();

        if (finalHpText != null)
            finalHpText.text = $"{maxHpStat.CalculatedValue}";

        if(finalAtkText != null) 
            finalAtkText.text = $"{attackStat.CalculatedValue}";

        if (isLobby) return;        

        if (playerHp == null)
            playerHp = FindAnyObjectByType<PlayerHp>();

        if (playerHp != null)
        {
            Debug.Log($"[PlayerStatsUpdate] finalHp={finalHp}, isLobby={isLobby}, playerHp={(playerHp != null ? playerHp.name : "NULL")}");
            playerHp.ReflectMaxHp(finalHp, isMaintainProportion: true, isnotify: true);
        }
    }


    //파츠별 스탯 계산해서 PlayerStat에 반영
    public void PartsInstallation(EquipmentSO.EquipmentPart part)
    {
        string uid = inventoryManager.GetSaveEquipmentUid(part);

        if (string.IsNullOrEmpty(uid)) return;

        InventoryItemData data = inventoryManager.FindUid(uid);

        if (data == null || data.scriptableObjectData == null) return;
        var so = data.scriptableObjectData;        

        so.GetFinalStats(data.classType, data.step, out int finalHp, out int finalAttack);
        switch(so.partType)
        {
            case (EquipmentSO.EquipmentPart.Armor):
            case (EquipmentSO.EquipmentPart.Belt):
            case (EquipmentSO.EquipmentPart.Shoes):
                if (finalHp != 0)
                    maxHpStat.AddModifier(new StatModifier(finalHp, StatModType.Flat, this));
                break;
            case (EquipmentSO.EquipmentPart.Weapon):               
            case (EquipmentSO.EquipmentPart.Necklace):               
            case (EquipmentSO.EquipmentPart.Gloves):
                if(finalAttack != 0)
                    attackStat.AddModifier(new StatModifier(finalAttack, StatModType.Flat, this));
                break;                                  
        }      
    }
}
