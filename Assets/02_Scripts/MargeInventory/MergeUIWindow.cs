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
            actionMergeButton.onClick.AddListener(OnClickMergeButton);           
        }
        CheckMerge();
    }

    //중복방지
    private void OnDestroy()
    {
        if (actionMergeButton != null)
            actionMergeButton.onClick.RemoveListener(OnClickMergeButton);
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
    
    //버튼 클릭 이벤트 등록
    public void OnClickMergeButton()
    {
        if (mergeController.IsMarge())
        {
            ClearTopSlots();
            FindObjectOfType<MergeInventory>().RefreshMergeUI();
        }
    }
    //다시 로비로가기
    public void CloseMergeScene()
    {        
        SceneManager.UnloadSceneAsync("MergeScene");
       
    }

    //업그래이드할 장비등록
    void SetBase(EquipmentItem item)
    {
        mergeController.SetBase(item);
        selectedItem = item;

        foreach(Transform child in upgradeSlotParent)
        {
            if (child.GetComponent<EquipmentItem>() != null)
                Destroy(child.gameObject);
        }

        SpawnIcon(item, upgradeSlotParent, true);
        IngredientSlotBackGround();

        UpdateResult();
        CheckMerge();
    }

    //업그래이드할 장비의 재료 등록
    void ToggleIngredient(EquipmentItem item)
    {
        if (ingredientItems.Contains(item))
        {
            bool check = mergeController.ToggleIngredient(item);
            if (!check) return;

            ingredientItems.Remove(item);            
        }
        else
        {
            bool check = mergeController.ToggleIngredient(item);
            if (!check) return;

            ingredientItems.Add(item);
        }

        RemoveIngredientUI();
        CheckMerge();
    }

    //등록 장비 취소할때
    void RemoveBase()
    {
        ingredientItems.Clear();
        RemoveIngredientUI(); // 재료 UI 지우기
        // mergeController.ClearIngredients(); // (컨트롤러에 재료 비우기 함수가 있다면 호출 권장)

        mergeController.SetBase(null);
        foreach (Transform child in upgradeSlotParent)
        {
            if (child.GetComponent<EquipmentItem>() != null)
                Destroy(child.gameObject);
        }
        foreach (Transform child in resultSlotParent)
        {
            if (child.GetComponent<EquipmentItem>() != null)
                Destroy(child.gameObject);
        }

        selectedItem = null;
        IngredientSlotBackGround();
        CheckMerge();
    }

    //슬롯 초기화
    void ClearTopSlots()
    {
        RemoveBase();
    }

    //병합하면 어떻게 되는지 예시 창 로직
    void UpdateResult()
    {

        foreach (Transform child in resultSlotParent)
        {
            if (child.GetComponent<EquipmentItem>() != null)
                Destroy(child.gameObject);
        }

        if (selectedItem == null) return;

        //예상 결과 계산      
        EquipmentSO.EquipmentClassType nextType;
        int nextStep;

        if(!mergeController.ResultPreview(selectedItem.ClassType, selectedItem.Step, out nextType, out nextStep))
        {
            nextType = selectedItem.ClassType;
            nextStep = selectedItem.Step;
        }      

            //결과 장비 생성
            GameObject resultObj = Instantiate(itemPrefab);
        RectTransform resultRect = resultObj.GetComponent<RectTransform>();
        resultRect.SetParent(resultSlotParent, false);

        ForceRectToFill(resultRect);
        EnsureIgnoreLayout(resultObj);

        EquipmentItem resultUI = resultObj.GetComponent<EquipmentItem>();
        resultUI.Initialize(selectedItem.Data, nextType, nextStep);

        //결과예시창 클릭 막기
        Button btn = resultObj.GetComponent<Button>();
        if (btn != null)
        {
            btn.transition = Selectable.Transition.None;
            btn.interactable = false;
        }
    }
   
    //재료 슬롯 1개일때는 1개만 2개일때는 2개 보여지게하기
    void IngredientSlotBackGround()
    {
        if (ingredientSlotParent == null) return;

        int needCount = 0;
        if (selectedItem != null && mergeController != null)
            needCount = mergeController.GetIngredentNeedCount();

        for (int i = 0; i < ingredientSlotParent.childCount; i++)
        {
            ingredientSlotParent.GetChild(i).gameObject.SetActive(i < needCount);
        }
    }    

    //선택한 재료 다시 되돌리기
    void RemoveIngredientUI()
    {
        if(ingredientSlotParent.childCount == 0)
        {
            foreach (Transform child in ingredientSlotParent)
            {
                if (child.GetComponent<EquipmentItem>() != null)
                    Destroy(child.gameObject);
            }


            foreach (var item in ingredientItems)
            {
                SpawnIcon(item, ingredientSlotParent, false);
            }

            return;
        }

        for(int i = 0; i < ingredientSlotParent.childCount; i++)
        {
            Transform slot = ingredientSlotParent.GetChild(i);

            for (int j = slot.childCount - 1; j >= 0; j--)
            {
                Transform t = slot.GetChild(j);

                if(t.GetComponent<EquipmentItem>() != null)
                    Destroy(t.gameObject);
            }
        }

        for (int i = 0; i < ingredientItems.Count; i++)
        {
            if (i >= ingredientSlotParent.childCount) break;

            Transform slot = ingredientSlotParent.GetChild(i);
            SpawnIcon(ingredientItems[i], slot, false);
        }
    }

    //병합할 수 있는지 체크하고 버튼 클릭 활성화 시킬지
    void CheckMerge()
    {
        if (actionMergeButton == null || mergeController == null) return;

        actionMergeButton.interactable = mergeController.CanMerge();
    }

    //업그래이드될 아이템 생성
    void SpawnIcon(EquipmentItem item, Transform parent, bool isBaseItem)
    {
        GameObject movement = Instantiate(itemPrefab);
        RectTransform childRect = movement.GetComponent<RectTransform>();
        childRect.SetParent(parent, false);

        ForceRectToFill(childRect);
        EnsureIgnoreLayout(movement);

        EquipmentItem uiItem = movement.GetComponent<EquipmentItem>();
        uiItem.Initialize(item.Data, item.ClassType, item.Step);
        uiItem.BindInventory(item.inventoryUid);

        uiItem.SetOnClickAction(() =>
        {
            if (isBaseItem) RemoveBase();
            else ToggleIngredient(item);
        });

        Debug.Log($"[UI] SpawnIcon uid={uiItem.inventoryUid}");
    }
    #region RectTransform 슬롯에 이동시 크기 강제
    void ForceRectToFill(RectTransform rect)
    {
        if (rect == null) return;

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;

        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;
    }
    void EnsureIgnoreLayout(GameObject gameObject)
    {
        if (gameObject == null) return;

        var le = gameObject.GetComponent<LayoutElement>();
        if (le == null) le = gameObject.AddComponent<LayoutElement>();
        le.ignoreLayout = true;
        
        gameObject.transform.SetAsLastSibling();
    }
    #endregion
}