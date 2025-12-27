using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MergeUIWindow : MonoBehaviour
{
    [SerializeField] private MergeController mergeController;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Button actionMergeButton;

    [SerializeField] private Transform upgradeSlotParent;
    [SerializeField] private Transform ingredientSlotParent;
    [SerializeField] private Transform resultSlotParent;

    private EquipmentItem selectedItem;
    private List<EquipmentItem> ingredientItems = new List<EquipmentItem>();    

    void Start()
    {
        ClearTopSlots();

        if(actionMergeButton != null)
        {
            actionMergeButton.interactable = false;

            actionMergeButton.onClick.AddListener(() =>
            {
                if(mergeController.IsMarge())
                {
                    ClearTopSlots();
                    FindObjectOfType<MergeInventory>().RefreshMergeUI();
                }
            });
        }
    }
    //하단 장비창에서 선택메서드
    public void OnItemSelected(EquipmentItem item)
    {
        if(selectedItem == null)
        {
            SetBase(item);
        }
        else
        {
            if (item == selectedItem) return;

            ToggleIngredient(item);
        }
    }
    //업그래이드할 장비등록
    private void SetBase(EquipmentItem item)
    {
        mergeController.SetBase(item);
        selectedItem = item;

        foreach(Transform child in upgradeSlotParent)
        {
            Destroy(child.gameObject);
        }

        SpawnIcon(item, upgradeSlotParent, true);

        UpdateResult();
    }
    //병합하면 어떻게 되는지 예시 창 로직
    void UpdateResult()
    {
        // 기존 결과 아이콘 삭제
        foreach (Transform child in resultSlotParent)
        {
            Destroy(child.gameObject);
        }

        if (selectedItem == null) return;

        //예상 결과 계산      
        EquipmentSO.EquipmentClassType nextType = selectedItem.Type;
        int nextStep = selectedItem.Step;

        if (selectedItem.Step >= 2)
        {
            nextType = selectedItem.Type + 1;
            nextStep = 0;
        }
        else
        {
            nextStep = selectedItem.Step + 1;
        }

        //결과 장비 생성
        GameObject resultObj = Instantiate(itemPrefab, resultSlotParent);
        EquipmentItem resultUI = resultObj.GetComponent<EquipmentItem>();
        resultUI.Initialize(selectedItem.Data, nextType, nextStep);

        //결과예시창 클릭 막기
        Button btn = resultObj.GetComponent<Button>();
        if (btn != null) btn.interactable = false;
    }
    //업그래이드할 장비의 재료 등록
    private void ToggleIngredient(EquipmentItem item)
    {
        if (ingredientItems.Contains(item))
        {
            ingredientItems.Remove(item);
            mergeController.ToggleMaterial(item);
        }        
        else
        {
            ingredientItems.Add(item);
            mergeController.ToggleMaterial(item);
        }
        
        RemoveIngredientUI();        
        CheckMergeButton();
    }
    void RemoveIngredientUI()
    {
        foreach (Transform child in ingredientSlotParent)
        {
            Destroy(child.gameObject);
        }

        
        foreach (var item in ingredientItems)
        {            
            SpawnIcon(item, ingredientSlotParent, false);
        }
    }    
    //업그래이드될 아이템 생성
    void SpawnIcon(EquipmentItem item, Transform parent, bool isBaseItem)
    {
        GameObject movement = Instantiate(itemPrefab, parent);
        EquipmentItem uiItem = movement.GetComponent<EquipmentItem>();

        uiItem.Initialize(item.Data, item.Type, item.Step);

        uiItem.SetOnClickAction(() =>
        {
            if (isBaseItem) RemoveBase();
            else ToggleIngredient(item);
        });
    }
    //등록 장비 취소할때
    void RemoveBase()
    {
        ingredientItems.Clear();
        RemoveIngredientUI(); // 재료 UI 지우기
        // mergeController.ClearIngredients(); // (컨트롤러에 재료 비우기 함수가 있다면 호출 권장)

        mergeController.SetBase(null);
        foreach (Transform child in upgradeSlotParent) Destroy(child.gameObject);
        foreach (Transform child in resultSlotParent) Destroy(child.gameObject); // 결과창도 지우기

        selectedItem = null;
        CheckMergeButton();
    }
    void CheckMergeButton()
    {
        if (actionMergeButton != null)
            actionMergeButton.interactable = mergeController.IsMarge();
    }
   
    //슬롯 초기화
    void ClearTopSlots()
    {
        RemoveBase();
    }
    //다시 로비로가기
    public void CloseMergeScene()
    {
        SceneManager.UnloadSceneAsync("MergeScene");
    }
}