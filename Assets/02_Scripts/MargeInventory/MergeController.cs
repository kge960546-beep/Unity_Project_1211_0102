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
            if(r.inputType == baseItem.Type && r.inputStep == baseItem.Step)
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

        var recipe = recipes.FirstOrDefault(r => r.inputType == baseItem.Type && r.inputStep == baseItem.Step);
        if (recipe == null) return false;

        if (ingredients.Count != recipe.needCount) return false;

        return ingredients.All(mat => IsValidIngredient(mat, recipe));
    }

    //병합 최종 체크하고 병합 실행
    public bool IsMarge()
    {
        if (baseItem == null || baseItem.Data == null) return false;

        MergeRecipe recipe = recipes.FirstOrDefault(r => r.inputType == baseItem.Type && r.inputStep == baseItem.Step);

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
        Debug.Log($"[Merge] baseItem.Type={baseItem.Type}, baseItem.Step={baseItem.Step}");
#endif

        //var recipe = recipes.FirstOrDefault(r => r.type == baseItem.Type && r.inputStep == baseItem.Step);
        MergeRecipe recipe = null;
        foreach (MergeRecipe r in recipes)
        {
            if (r.inputType == baseItem.Type && r.inputStep == baseItem.Step)
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


        if (item.Type != recipe.needType)
        {
            Debug.Log($"[Merge] FAIL Type mat={item.Type} need={recipe.needType}");
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
        
        var baseData = inventoryManager.FindUid(baseItem.inventoryUid);
        if (baseData != null)
        {
            baseData.classType = recipe.resultType;
            baseData.step = recipe.resultStep;
        }
        
        baseItem.Initialize(baseItem.Data, recipe.resultType, recipe.resultStep);

        ingredients.Clear();
        baseItem = null;
        return true;        
    }   

    //병합 규칙
    private void InitializeDefaultRecipes()
    {
        //Better(파랑등급) 까지는 같은 종류면 상관x
        AddRecipe(EquipmentSO.EquipmentClassType.Normal,
                  0, 2, EquipmentSO.EquipmentClassType.Normal, 0, EquipmentSO.EquipmentClassType.Good, 0, true);
        AddRecipe(EquipmentSO.EquipmentClassType.Good,
                  0, 2, EquipmentSO.EquipmentClassType.Good, 0, EquipmentSO.EquipmentClassType.Better, 0, true);

        AddRecipe(EquipmentSO.EquipmentClassType.Better,
                  0, 2, EquipmentSO.EquipmentClassType.Better, 0, EquipmentSO.EquipmentClassType.Excellent, 0, true);

        AddRecipe(EquipmentSO.EquipmentClassType.Excellent,
                  0, 1, EquipmentSO.EquipmentClassType.Excellent, 0, EquipmentSO.EquipmentClassType.Excellent, 1, false);
        
        AddRecipe(EquipmentSO.EquipmentClassType.Excellent,
                  1, 2, EquipmentSO.EquipmentClassType.Excellent, 0, EquipmentSO.EquipmentClassType.Excellent, 2, false);

        AddRecipe(EquipmentSO.EquipmentClassType.Excellent,
                  2, 1, EquipmentSO.EquipmentClassType.Excellent, 2, EquipmentSO.EquipmentClassType.Epic, 0, true);
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