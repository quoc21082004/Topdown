using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemOptions : ItemOptions
{
    public GameObject SethotkeyWindow;
    public GameObject discardWindow;
    public Button Usebtn;
    public Button Hotkeybtn;
    public Button Discardbtn;
    public Button Backbtn;
    private void OnEnable()
    {
        Backbtn.onClick.AddListener(() =>
        {
            OnBackButton();
        });

        if (InventoryUI.selectedItem == null)
        {
            Usebtn.interactable = false;
            Hotkeybtn.interactable = false;
            Discardbtn.interactable = false;
        }
        else if (InventoryUI.selectedItem != null) 
        {
            Usebtn.interactable = InventoryUI.selectedItem.GetType().IsSubclassOf(typeof(Consumable));
            Hotkeybtn.interactable = InventoryUI.selectedItem.GetType().IsSubclassOf(typeof(Consumable));
            Discardbtn.interactable = true;
        }
        Usebtn.Select();
        Usebtn.OnSelect(null);
    }
    public void UseItem()
    {
        Consumable item = (Consumable)InventoryUI.selectedItem;
        if (!ItemHotKeyManager.instance.IsItemOnCoolDown(item))
        {
            ItemHotKeyManager.instance.UseItem(item);
            gameObject.SetActive(false);
            selectSlotbtn.Select();
            selectSlotbtn.OnSelect(null);
        }
        else if (ItemHotKeyManager.instance.IsItemOnCoolDown(item))
        {
            float remainingCD = ItemHotKeyManager.instance.GetRemainnigCD(item);
            Debug.Log("Remaining CD :" + remainingCD);
        }
    }
    public void OnDiscardItem()
    {
        discardWindow.gameObject.SetActive(true);
        discardWindow.gameObject.GetComponent<AmtConfirmWindow>().InitAmt(1);
        this.gameObject.SetActive(false);
    }
    public void OnSelectHotKeyItem()
    {
        SethotkeyWindow.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
