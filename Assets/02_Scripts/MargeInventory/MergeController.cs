using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class MergeRecipe
{
    [Header("선택 조건")]
    public EquipmentSO.EquipmentClassType type;    
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
    public void SetBase(EquipmentItem item)
    {
        if (item == null) return;

        baseItem = item;
        ingredients.Clear();
    }
    public void ToggleMaterial(EquipmentItem item)
    {
        if (item == null || item == baseItem) return;

        if (ingredients.Contains(item)) ingredients.Remove(item);
        else ingredients.Add(item);
    }
    public bool IsMarge()
    {
        if (baseItem == null || baseItem.Data == null) return false;

        MergeRecipe recipe = recipes.FirstOrDefault(r => r.type == baseItem.Type && r.inputStep == baseItem.Step);

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
        bool isValid = ingredients.All(mat => IsValidMaterial(mat, recipe));

        if(!isValid)
        {
#if UNITY_EDITOR
            Debug.Log("조건에 맞지 않는 재료가 있음");
#endif
            return false;
        }

        ProcessMergeSuccess(recipe);
        return isValid;
    }
    public bool IsValidMaterial(EquipmentItem item, MergeRecipe recipe)
    {
        if (item == null) return false;

        if (item.Type != recipe.needType) return false;
        if (item.Step != recipe.needStep) return false;
        
        if(item.Data.partType != baseItem.Data.partType) return false;

        if(recipe.isSameId)
        {
            if(item.EquipmentId != baseItem.EquipmentId) return false;
        }

        return true;
    }
    public bool ProcessMergeSuccess(MergeRecipe recipe)
    {
        GameObject itemPrefab = baseItem.Data.GetUpgradePrefab(recipe.resultType, recipe.resultStep);

        if(itemPrefab != null)
        {
            Transform slot = baseItem.transform.parent;
            GameObject newObject = Instantiate(itemPrefab, baseItem.transform.position, Quaternion.identity, slot);

            EquipmentItem newItem = newObject.GetComponent<EquipmentItem>();
            if(newItem != null)
            {
                newItem.Initialize(baseItem.Data, recipe.resultType, recipe.resultStep);
            }
        }

        foreach(var mat in ingredients)
        {
            if (mat != null) Destroy(mat.gameObject);
        }
        Destroy(baseItem.gameObject);

        baseItem = null;
        ingredients.Clear();

        return true;
    }   
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
    private void AddRecipe(EquipmentSO.EquipmentClassType inType, int inStep, int count,
                          EquipmentSO.EquipmentClassType needType, int needStep,
                          EquipmentSO.EquipmentClassType outType, int outStep, bool sameId)
    {
        recipes.Add(new MergeRecipe()
        {
            type = inType,
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
