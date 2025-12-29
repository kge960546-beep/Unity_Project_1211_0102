using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class MergeRecipe
{
    [Header("선택 조건")]
    public EquipmentSO.EquipmentClassType inputType;    
    public int inputStep;

    [Header("재료 조건")]
    public int needCount;
    public EquipmentSO.EquipmentClassType needType;
    public int needStep;
    public bool isSameId;

    [Header("결과")]
    public EquipmentSO.EquipmentClassType resultType;
    public int resultStep;
}
public class MergeController : MonoBehaviour
{
    [Header("선택")]
    [SerializeField] private EquipmentItem baseItem;
    [SerializeField] private List<EquipmentItem> ingredients = new List<EquipmentItem>();

    [Header("규칙 리스트")]
    [SerializeField] private List<MergeRecipe> recipes = new List<MergeRecipe>();

    private void Awake()
    {
        if (recipes.Count == 0) InitializeDefaultRecipes();
    }

    //병합대상 아이템 지정후 기존 재료는 초기화
    public void SetBase(EquipmentItem item)
    {
        baseItem = item;
        ingredients.Clear();
    }

    //재료를 선택/ 해제 하는 기능
    public bool ToggleIngredient(EquipmentItem item)
    {
        if (item == null || item == baseItem || baseItem == null || baseItem.Data == null) return false;
        
        if(ingredients.Contains(item))
        {
            ingredients.Remove(item);
            return true;
        }

        MergeRecipe recipe = null;
        for(int i = 0; i < recipes.Count; i++)
        {
            MergeRecipe r = recipes[i];
            if(r.inputType == baseItem.ClassType && r.inputStep == baseItem.Step)
            {
                recipe = r;
                break;
            }
        }
        if (recipe == null) return false;
        if (ingredients.Count >= recipe.needCount) return false;

        if (!IsValidIngredient(item, recipe)) return false;

        ingredients.Add(item);
        return true;
    }
   

    //병합을 할 수 있는지 판단
    public bool CanMerge()
    {
        if (baseItem == null || baseItem.Data == null) return false;

        var recipe = recipes.FirstOrDefault(r => r.inputType == baseItem.ClassType && r.inputStep == baseItem.Step);
        if (recipe == null) return false;

        if (ingredients.Count != recipe.needCount) return false;

        return ingredients.All(mat => IsValidIngredient(mat, recipe));
    }

    //병합 최종 체크하고 병합 실행
    public bool IsMarge()
    {
        if (baseItem == null || baseItem.Data == null) return false;

        MergeRecipe recipe = recipes.FirstOrDefault(r => r.inputType == baseItem.ClassType && r.inputStep == baseItem.Step);

        if (recipe == null)
        {
#if UNITY_EDITOR
            Debug.Log("합성 단계가 아님");
#endif
            return false;
        }
        
        if(ingredients.Count != recipe.needCount)
        {
#if UNITY_EDITOR
            Debug.Log($"재료가 {recipe.needCount}개 필요함");
#endif
            return false;
        }
        bool isValid = ingredients.All(mat => IsValidIngredient(mat, recipe));

        if(!isValid)
        {
#if UNITY_EDITOR
            Debug.Log("조건에 맞지 않는 재료가 있음");
#endif
            return false;
        }
        Debug.Log("[Merge] IsMarge SUCCESS");
        ProcessMergeSuccess(recipe);
        return isValid;
    }

    //재료가 몇개 필요한지 판단
    public int GetIngredientNeedCount()
    {
        if (baseItem == null || baseItem.Data == null)
        {
            return 0; 
        }

#if UNITY_EDITOR
        Debug.Log($"[Merge] baseItem.Type={baseItem.ClassType}, baseItem.Step={baseItem.Step}");
#endif

        //var recipe = recipes.FirstOrDefault(r => r.type == baseItem.Type && r.inputStep == baseItem.Step);
        MergeRecipe recipe = null;
        foreach (MergeRecipe r in recipes)
        {
            if (r.inputType == baseItem.ClassType && r.inputStep == baseItem.Step)
            {
                recipe = r;
                break;
            }
        }

        if (recipe == null)
        {
#if UNITY_EDITOR
            Debug.Log("[Merge] recipe NOT FOUND");
#endif
            return 0;
        }
#if UNITY_EDITOR
        Debug.Log($"[Merge] recipe FOUND, needCount={recipe.needCount}, needType={recipe.needType}, needStep={recipe.needStep}, sameId={recipe.isSameId}");
#endif
        return recipe.needCount;
    }

    //재료가 조건에 맞는지 판단
    public bool IsValidIngredient(EquipmentItem item, MergeRecipe recipe)
    {
        if (item == null)
        {
            Debug.Log("[Merge] mat null");
            return false;
        }

        if (item.Data == null) { Debug.Log("[Merge] mat.Data null"); return false; }
        if (baseItem == null || baseItem.Data == null) { Debug.Log("[Merge] base null/data null"); return false; }


        if (item.ClassType != recipe.needType)
        {
            Debug.Log($"[Merge] FAIL Type mat={item.ClassType} need={recipe.needType}");
            return false;
        }

        if (item.Step != recipe.needStep)
        {
            Debug.Log($"[Merge] FAIL Step mat={item.Step} need={recipe.needStep}");
            return false;
        }

        if (item.Data.partType != baseItem.Data.partType)
        {
            Debug.Log($"[Merge] FAIL partType mat={item.Data.partType} base={baseItem.Data.partType}");
            return false;
        }

        if(recipe.isSameId)
        {
            if (item.Data == null || baseItem.Data == null) return false;

            int itemId = item.Data.equipmentID;
            int baseId = baseItem.Data.equipmentID;

            if (itemId != baseId)
            {
                Debug.Log($"[Merge] FAIL equipmentID mat={itemId} base={baseId}");
                return false;
            }
        }

        return true;
    }
   
    //병합성공시 실행
    public bool ProcessMergeSuccess(MergeRecipe recipe)
    {
        if (baseItem == null || baseItem.Data == null) return false;

        var inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null) return false;
        
        foreach (var mat in ingredients)
        {
            if (mat == null) continue;
            inventoryManager.RemoveUid(mat.inventoryUid);
            Destroy(mat.gameObject);
        }

        //var baseData = inventoryManager.FindUid(baseItem.inventoryUid);
        //if (baseData != null)
        //{
        //    baseData.classType = recipe.resultType;
        //    baseData.step = recipe.resultStep;
        //}

        bool updated = inventoryManager.UpdateUid(baseItem.inventoryUid, recipe.resultType, recipe.resultStep);
        
        if(!updated)
        {
            Debug.Log($"실패한 uid = {baseItem.inventoryUid}");
            return false; 
        }

        baseItem.Initialize(baseItem.Data, recipe.resultType, recipe.resultStep);

        ingredients.Clear();
        baseItem = null;
        return true;        
    }   

    //병합 규칙
    private void InitializeDefaultRecipes()
    {
        recipes.Clear();

        // 0) 초록까지: 같은 부위면 종류(장비ID) 상관없음(= step2 승급도 sameId false)
        AddTierRecipes(
            type: EquipmentSO.EquipmentClassType.Normal,
            nextType: EquipmentSO.EquipmentClassType.Good,
            sameIdOnStep2ToNext: false
        );

        AddTierRecipes(
            type: EquipmentSO.EquipmentClassType.Good,
            nextType: EquipmentSO.EquipmentClassType.Better,
            sameIdOnStep2ToNext: false
        );

        // 1) 파랑부터: step2 -> 다음 등급에서만 종류(장비ID) 같아야 함
        AddTierRecipes(
            type: EquipmentSO.EquipmentClassType.Better,
            nextType: EquipmentSO.EquipmentClassType.Excellent,
            sameIdOnStep2ToNext: true
        );

        AddTierRecipes(
            type: EquipmentSO.EquipmentClassType.Excellent,
            nextType: EquipmentSO.EquipmentClassType.Epic,
            sameIdOnStep2ToNext: true
        );

        AddTierRecipes(
            type: EquipmentSO.EquipmentClassType.Epic,
            nextType: EquipmentSO.EquipmentClassType.Legend,
            sameIdOnStep2ToNext: true
        );

        // 2) 빨강(마지막 티어): 내부 단계만(0->1, 1->2). step2 승급은 없음.
        AddFinalTierRecipes(EquipmentSO.EquipmentClassType.Legend);
    }

    private void AddTierRecipes(EquipmentSO.EquipmentClassType type,
                                EquipmentSO.EquipmentClassType nextType, bool sameIdOnStep2ToNext)
    {
        //재료 1개 같은 부위면 종류 무관
        AddRecipe(type, 0, 1, type, 0, type, 1, sameId: false);

        //재료 2개 같은 부위면 종류 무관
        AddRecipe(type, 1, 2, type, 1, type, 2, sameId: false);

        // step2 -> nextType step0 : 재료 1개
        // - Normal ~ Good: sameId=false
        // - Better 이상: sameId=true
        AddRecipe(type, 2, 1, type, 2, nextType, 0, sameId: sameIdOnStep2ToNext);
    }
    private void AddFinalTierRecipes(EquipmentSO.EquipmentClassType type)
    {
        AddRecipe(type, 0, 1, type, 0, type, 1, sameId: false);
        AddRecipe(type, 1, 2, type, 1, type, 2, sameId: false);        
    }

    //병합 레시피 추가메서드
    private void AddRecipe(EquipmentSO.EquipmentClassType inType, int inStep, int count,
                           EquipmentSO.EquipmentClassType needType, int needStep,
                           EquipmentSO.EquipmentClassType outType, int outStep, bool sameId)
    {
        recipes.Add(new MergeRecipe()
        {
            inputType = inType,
            inputStep = inStep,
            needCount = count,
            needType = needType,
            needStep = needStep,
            isSameId = sameId,
            resultType = outType,
            resultStep = outStep
        });
    }
}