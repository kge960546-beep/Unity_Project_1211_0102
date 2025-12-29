using System.Collections.Generic;
using System.Linq;

public static class InventorySortComparer
{
    //등급별 정렬
    public static List<InventoryItemData> SortDescendingOrderByRank(List<InventoryItemData> sorted)
    {
        return sorted.OrderByDescending(itemData => itemData.classType).
            ThenByDescending(itemData => itemData.step).ToList();
    }

    //부위별 정렬
    public static List<InventoryItemData> SortByPart(List<InventoryItemData> sorted) 
    {
        return sorted.OrderBy(itemData => itemData.soData.partType).
            ThenByDescending(itemData => itemData.classType).
            ThenByDescending(itemData => itemData.step).ToList();        
    }
}