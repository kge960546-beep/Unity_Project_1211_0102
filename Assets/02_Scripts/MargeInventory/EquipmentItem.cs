using UnityEngine;

public class EquipmentItem : MonoBehaviour
{
    [SerializeField] private EquipmentSO data;
    [SerializeField] private EquipmentSO.EquipmentClassType type;    
    [Range(0, 2)] public int step;

    public EquipmentSO Data => data;
    public int EquipmentId { get { return data != null ? data.equipmentID : -1; } }
    public EquipmentSO.EquipmentClassType Type => type;    
    public int Step => step;
   
    public void Initialize(EquipmentSO newData, EquipmentSO.EquipmentClassType newtype,int newStep)
    {
        data = newData;
        type = newtype;
        step = Mathf.Clamp(newStep, 0, 2);

        //TODO: 아이콘, 능력치 갱신 로직 구현해야함
    }
}
