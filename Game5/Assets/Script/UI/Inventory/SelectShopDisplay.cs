﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SelectShopDisplay : SelectedItemDisplay
{ 
    public TextMeshProUGUI itemPriceGold_txt;
    private void OnEnable()
    {
        UpdateUI();
    }
    public override void UpdateUI()
    {
        if (InventoryUI.selectedItem == null)
        {
            itemTitle_txt.text = "";
            itemDescription_txt.text = "";
            itemPriceGold_txt.text = "";
        }
        else if (InventoryUI.selectedItem != null)
        {
            itemTitle_txt.text = "" + InventoryUI.selectedItem.name;
            itemDescription_txt.text = "" + InventoryUI.selectedItem.itemDescription;
            itemPriceGold_txt.text = "" + InventoryUI.selectedItem.buyPrice + "  <sprite=3>";
        }
    }
}
