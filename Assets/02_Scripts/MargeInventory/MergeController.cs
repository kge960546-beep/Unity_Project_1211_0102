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
    [SerializeField] private string baseUid;
    [SerializeField] private List<string> ingredientUids = new List<string>();    

    [Header("규칙 리스트")]
    [SerializeField] private List<MergeRecipe> recipes = new List<MergeRecipe>();

    private void Awake()
    {
        if (recipes.Count == 0) InitializeDefaultRecipes();
    }

    //병합대상 아이템 지정후 기존 재료는 초기화
    public void SetBase(EquipmentItem item)
    {
        baseUid = (item != null) ? item.inventoryUid : null;
        ingredientUids.Clear();
    }

    //재료를 선택/ 해제 하는 기능
    public bool ToggleIngredient(EquipmentItem item)
    {
        if (item == null) return false;
        
        string uid = item.inventoryUid;
        if (string.IsNullOrEmpty(uid)) return false;
        if (string.IsNullOrEmpty(baseUid)) return false;
        if(uid == baseUid) return false;

        if(ingredientUids.Contains(uid))
        {
            ingredientUids.Remove(uid);
            return true;
        }
        
        if(!TryGetBaseData(out var baseData)) return false;

        MergeRecipe recipe = recipes.FirstOrDefault(rec => rec.inputType == baseData.classType && rec.inputStep == baseData.step);

        if (recipe == null) return false;
        if (ingredientUids.Count >= recipe.needCount) return false;
        if (!IsValidIngredientUid(uid, baseData, recipe)) return false;

        ingredientUids.Add(uid);
        return true;
    }   
    public bool TryGetBaseData(out InventoryItemData baseData)
    {
        baseData = null;
        if (InventoryManager.Instance == null) return false;
        if (string.IsNullOrEmpty(baseUid)) return false;

        baseData = InventoryManager.Instance.FindUid(baseUid);

        return baseData != null && baseData.scriptableObjectData != null;
    }


    //병합을 할 수 있는지 판단
    public bool CanMerge()
    {
        if (!TryGetBaseData(out var baseData)) return false;

        var recipe = recipes.FirstOrDefault(recipe => recipe.inputType == baseData.classType && recipe.inputStep == baseData.step);
        if (recipe == null) return false;

        if (ingredientUids.Count != recipe.needCount) return false;

        for (int i = 0; i < ingredientUids.Count; i++)
        {
            if (!IsValidIngredientUid(ingredientUids[i], baseData, recipe)) return false;
        }

        return true;
    }

    //병합 최종 체크하고 병합 실행
    public bool IsMarge()
    {
        if (!TryGetBaseData(out var baseData)) return false;
        

        MergeRecipe recipe = recipes.FirstOrDefault(recipe => recipe.inputType == baseData.classType && recipe.inputStep == baseData.step);

        if (recipe == null)
        {
            return false;
        }
        
        if(ingredientUids.Count != recipe.needCount)
        {
            return false;
        }
        
        for(int i = 0; i< ingredientUids.Count; i++)
        {
            if (!IsValidIngredientUid(ingredientUids[i], baseData, recipe)) return false;
        }

        return ProcessMergeSuccess(recipe);
    }

    //재료가 몇개 필요한지 판단
    public int GetIngredentNeedCount()
    {
        if (!TryGetBaseData(out var baseData)) return 0;

        //var recipe = recipes.FirstOrDefault(r => r.type == baseItem.Type && r.inputStep == baseItem.Step);
        MergeRecipe recipe = null;
        foreach (MergeRecipe rec in recipes)
        {
            if (rec.inputType == baseData.classType && rec.inputStep == baseData.step)
            {
                recipe = rec;
                break;
            }
        }        

        return (recipe != null)? recipe.needCount : 0;
    }

    //재료가 조건에 맞는지 판단
    public bool IsValidIngredientUid(string uid, InventoryItemData baseData, MergeRecipe recipe)
    {
        if (string.IsNullOrEmpty(uid)) return false;
        if(InventoryManager.Instance == null) return false;

        var mat = InventoryManager.Instance.FindUid(uid);
        if (mat == null) return false;
        if (mat.scriptableObjectData == null) return false;
        if (baseData == null || baseData.scriptableObjectData == null) return false;        

        if(mat.classType != recipe.needType) return false;
        if (mat.step != recipe.needStep) return false;

        if(mat.scriptableObjectData.partType != baseData.scriptableObjectData.partType) return false;
       
        if(recipe.isSameId)
        {
            if (mat.scriptableObjectData.equipmentID != baseData.scriptableObjectData.equipmentID)
                return false;
        }          

        return true;
    }
   
    //병합성공시 실행
    public bool ProcessMergeSuccess(MergeRecipe recipe)
    {
        if (InventoryManager.Instance == null) return false;
        if(string.IsNullOrEmpty(baseUid)) return false;

        if(InventoryManager.Instance.FindUid(baseUid) == null) return false;
        

        for(int i = 0; i<ingredientUids.Count; i++)
        {
            string matUid = ingredientUids[i];
            if(string.IsNullOrEmpty(matUid)) continue;
            if (matUid == baseUid) continue;

            InventoryManager.Instance.RemoveUid(matUid, false);
        }

        bool updated = InventoryManager.Instance.UpdateUid(baseUid, recipe.resultType, recipe.resultStep);

        if (!updated) return false;

        ingredientUids.Clear();
        baseUid = null;
        return true;        
    }
    public string GetBaseUid() => baseUid;
    public IReadOnlyList<string> GetIngredentUids() => ingredientUids;
    public bool IsIngredientSelected(string uid) => ingredientUids.Contains(uid);
    //병합 규칙
    private void InitializeDefaultRecipes()
    {
        recipes.Clear();

        AddSimpleMerge( EquipmentSO.EquipmentClassType.Normal, EquipmentSO.EquipmentClassType.Good );

        AddSimpleMerge(EquipmentSO.EquipmentClassType.Good, EquipmentSO.EquipmentClassType.Better);

        AddSimpleMerge(EquipmentSO.EquipmentClassType.Better, EquipmentSO.EquipmentClassType.Excellent);

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
        //현재티어(0) 는 재료(1)개필요하고 재료의 티어는(0)이어야한다 결과는 티어(1)이다, 종류는 (false)상관없다
        AddRecipe(type, 0, 1, type, 0, type, 1, sameId: false);

        //현재티어(1) 는 재료(2)개필요하고 재료의 티어는(0)이어야한다 결과는 티어(2)이다, 종류는 (false)상관없다
        AddRecipe(type, 1, 2, type, 0, type, 2, sameId: false);

        //현재티어(2) 는 재료(1)개필요하고 재료의 티어는(2)이어야한다 결과는 다음등급티어(0)이다, 종류는 (true)같아야한다.
        AddRecipe(type, 2, 1, type, 2, nextType, 0, sameId: sameIdOnStep2ToNext);
    }
    private void AddFinalTierRecipes(EquipmentSO.EquipmentClassType type)
    {
        AddRecipe(type, 0, 1, type, 0, type, 1, sameId: false);
        AddRecipe(type, 1, 2, type, 0, type, 2, sameId: false);
    }
    private void AddSimpleMerge(EquipmentSO.EquipmentClassType inType, EquipmentSO.EquipmentClassType outType)
    {
        //0등급 재료 0등급2개 = 0등급 다음단계 등급 필요타입 같은종류 장비 무관
        AddRecipe(inType, 0, 2, inType, 0, outType, 0, sameId: false);
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
    public bool ResultPreview(EquipmentSO.EquipmentClassType inType, int inStep, out EquipmentSO.EquipmentClassType outType, out int outStep)
    {
        outType = inType;
        outStep = inStep;

        
        var recipe = recipes.FirstOrDefault(r => r.inputType == inType && r.inputStep == inStep);

        if(recipe == null) return false;

        outType = recipe.resultType;
        outStep = recipe.resultStep;
        return true;
    }
}