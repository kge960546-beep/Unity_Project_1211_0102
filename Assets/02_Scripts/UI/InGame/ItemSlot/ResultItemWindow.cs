using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ResultItemWindow : MonoBehaviour
{
    public Transform slotRoot;
    public GameObject slotIcon;

    private List<Slot> slots;   
   
    public void OpenResult(List<ItemData> itemDatas)
    {
        //슬롯 리스트 없으면 만들기
        if(slots == null)
        {
            slots = new List<Slot>();

            int slotCnt = slotRoot.childCount;

            for (int i = 0; i < slotCnt; i++)
            {
                var slot = slotRoot.GetChild(i).GetComponent<Slot>();

                if (slot != null)
                {
                    slots.Add(slot);                    
                }
            }
        }
        
        // 모든 슬롯 초기화
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }

        Debug.Log($"[결과창] 전달받은 아이템 개수: {itemDatas.Count}개");

        //등급별 이름별 정렬 Linq 이용
        List<ItemData> itemOpposite = itemDatas.OrderByDescending(x => x.itemClass).ThenBy(x => x.itemName).ToList();

        for (int i = 0; i < itemOpposite.Count; i++)
        {
            if(i < slots.Count)
            {
                slots[i].SetItem(itemOpposite[i]);
            }
            else
            {
                Debug.Log("슬롯이 부족함");
                break;
            }
        }
        gameObject.SetActive(true);
    }   
}
