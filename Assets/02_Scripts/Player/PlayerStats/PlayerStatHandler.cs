using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
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
        if (eqInventory == null) eqInventory = FindAnyObjectByType<EquipmentInventory>();
        if(inventoryManager == null) inventoryManager = FindAnyObjectByType<InventoryManager>();          
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
        if (maxHpStat == null || attackStat == null) return;
        if (inventoryManager == null || eqInventory == null) return;

        maxHpStat.RemoveAllModifier(this);
        attackStat.RemoveAllModifier(this);

        PartsInstallation(EquipmentSO.EquipmentPart.Weapon);
        PartsInstallation(EquipmentSO.EquipmentPart.Armor);
        PartsInstallation(EquipmentSO.EquipmentPart.Necklace);
        PartsInstallation(EquipmentSO.EquipmentPart.Belt);
        PartsInstallation(EquipmentSO.EquipmentPart.Gloves);
        PartsInstallation(EquipmentSO.EquipmentPart.Shoes);

        if(finalHpText != null)
            finalHpText.text = $"{maxHpStat.Value}";

        if(finalAtkText != null) 
            finalAtkText.text = $"{attackStat.Value}";

        if (isLobby) return;

        int finalHp = Mathf.RoundToInt(maxHpStat.Value);

        if (playerHp == null)
            playerHp = FindAnyObjectByType<PlayerHp>();

        if (playerHp != null)
            playerHp.ReflectMaxHp(finalHp, isMaintainProportion: true, isnotify: true);
    }


    //파츠별 스탯 계산해서 PlayerStat에 반영
    public void PartsInstallation(EquipmentSO.EquipmentPart part)
    {
        string uid = eqInventory.GetInventoryEquipmentUid(part);        

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
