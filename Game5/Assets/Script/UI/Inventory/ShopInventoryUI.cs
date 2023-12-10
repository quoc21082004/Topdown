using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoryUI : InventoryUI
{
    public List<ItemSO> shopItemList;

    protected override void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < shopItemList.Count)
            {
                slots[i].AddItem(shopItemList[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
        if (selectedItem != null && selectedItem.currentAmt < 0)
        {
            selectedItem = null;
        }
        selectItemDisplay.UpdateUI();
        gold_text.text = "" + inventory.Gold;
    }
}
