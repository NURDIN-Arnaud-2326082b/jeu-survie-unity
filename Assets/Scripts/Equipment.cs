using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Equipment : MonoBehaviour
{

    [SerializeField]
    private PlayerStats playerStats;

    [Header("Equipment Panel Reference")]

    //Référence à la bibliothèque d'équipements
    [SerializeField]
    EquipmentLibrary equipmentLibrary;

    //Référence à l'image du slot tête
    [SerializeField]
    private UnityEngine.UI.Image headSlotImage;

    //Référence à l'image du slot plastron
    [SerializeField]
    private UnityEngine.UI.Image chestSlotImage;

    //Référence à l'image du slot jambes
    [SerializeField]
    private UnityEngine.UI.Image legsSlotImage;

    //Référence à l'image du slot pieds
    [SerializeField]
    private UnityEngine.UI.Image feetSlotImage;

    //Référence à l'image du slot mains
    [SerializeField]
    private UnityEngine.UI.Image handsSlotImage;

    //Référence à l'image du slot arme
    [SerializeField]
    private UnityEngine.UI.Image weaponSlotImage;

    //Références a l'équipement porté sur la tête
    [HideInInspector]
    public ItemData equippedHeadItem;
    //Références a l'équipement porté sur le plastron
    [HideInInspector]   
    public ItemData equippedChestItem;
    //Références a l'équipement porté sur les jambes
    [HideInInspector]
    public ItemData equippedLegsItem;
    //Références a l'équipement porté sur les pieds
    [HideInInspector]
    public ItemData equippedFeetItem;
    //Références a l'équipement porté sur les mains
    public ItemData equippedHandsItem;
    //Référence à l'arme portée
    [HideInInspector]
    public ItemData equippedWeaponItem;
    //Référence aux slots de l'équipement
    [SerializeField]
    public Transform equipmentSlotsParent;

    //Référence au panneau d'action d'équipement
    [Header("Equipment Action Panel Reference")]
    [SerializeField]
    public GameObject actionEquipmentPanel;

    //bouton de l'action de déséquiper
    [SerializeField]
    private GameObject unequipEquipmentButton;

    //bouton de laction de drop
    [SerializeField]
    private GameObject dropEquipmentButton;

    //bouton de laction de destruction
    [SerializeField]
    private GameObject destroyEquipmentButton;

    //méthode pour désactiver l'équipement précédemment équipé
    private void DisablePreviousEquipmentEquiped(ItemData itemToDisable)
    {
        //vérifier si un équipement est déjà porté
        if (itemToDisable == null)
        {
            return;
        }
        //chercher l'équipement dans la bibliothèque
        EquipmentLibraryItem equipmentToDisable = equipmentLibrary.content.Where(e => e.itemData == itemToDisable).First();
        if (equipmentToDisable != null)
        {
            //Réactiver les éléments conflictuels et activer le prefab de l'équipement
            for (int i = 0; i < equipmentToDisable.ElementToDisable.Length; i++)
            {
                equipmentToDisable.ElementToDisable[i].SetActive(true);
            }
            equipmentToDisable.equipmentPrefab.SetActive(false);
        }
        playerStats.currentArmorPoints -= itemToDisable.armorPoints;
        //ajouter l'équipement à l'inventaire pour éviter la perte
        Inventory.instance.AddItem(itemToDisable);
    }

    public void OpenEquipmentActionPanel(ItemData item, UnityEngine.Vector3 slotPosition)
    {
        Inventory.instance.selectedItem = item;
        //vérifier si le slot est vide
        if (item == null)
        {
            CloseEquipmentActionPanel();
            return;
        }
        //configurer les boutons d'action en fonction du type d'item
        unequipEquipmentButton.SetActive(true);
        dropEquipmentButton.SetActive(true);
        destroyEquipmentButton.SetActive(true);
        actionEquipmentPanel.transform.position = slotPosition;
        actionEquipmentPanel.SetActive(true);
    }

    public void CloseEquipmentActionPanel()
    {
        actionEquipmentPanel.SetActive(false);
    }

    //méthode pour le bouton déséquiper
    public void UnequipEquipmentActionButton()
    {
        EquipmentLibraryItem equipmentToUnequip = equipmentLibrary.content.Where(e => e.itemData == Inventory.instance.selectedItem).First();
        if (equipmentToUnequip != null)
        {
            //Réactiver les éléments conflictuels et désactiver le prefab de l'équipement
            for (int i = 0; i < equipmentToUnequip.ElementToDisable.Length; i++)
            {
                equipmentToUnequip.ElementToDisable[i].SetActive(true);
            }
            equipmentToUnequip.equipmentPrefab.SetActive(false);
            //Mettre à jour l'interface des équipements
            switch (Inventory.instance.selectedItem.equipmentType)
            {
                case EquipmentType.Head:
                    headSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    equippedHeadItem = null;
                    break;
                case EquipmentType.Chest:
                    chestSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    equippedChestItem = null;
                    break;
                case EquipmentType.Hands:
                    handsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    equippedHandsItem = null;
                    break;
                case EquipmentType.Legs:
                    legsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    equippedLegsItem = null;
                    break;
                case EquipmentType.Feet:
                    feetSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    equippedFeetItem = null;
                    break;
                case EquipmentType.Weapon:
                    weaponSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    equippedWeaponItem = null;
                    break;
            }
            playerStats.currentArmorPoints -= Inventory.instance.selectedItem.armorPoints;
            //Ajouter l'équipement à l'inventaire pour éviter la perte
            Inventory.instance.AddItem(Inventory.instance.selectedItem);
            Inventory.instance.RefreshContent();
        }
        else
        {
            Debug.LogError("No equipment prefab found for item: " + Inventory.instance.selectedItem.name);
        }
        CloseEquipmentActionPanel();
        Inventory.instance.selectedItem = null;
    }

    //méthode pour le bouton drop item
    public void DropEquipmentActionButton()
    {
        EquipmentLibraryItem equipmentToDrop = equipmentLibrary.content.Where(e => e.itemData == Inventory.instance.selectedItem).First();
        if (equipmentToDrop != null)
        {
            //Réactiver les éléments conflictuels et désactiver le prefab de l'équipement
            for (int i = 0; i < equipmentToDrop.ElementToDisable.Length; i++)
            {
                equipmentToDrop.ElementToDisable[i].SetActive(true);
            }
            equipmentToDrop.equipmentPrefab.SetActive(false);
            //instancier l'équipement à dropper au point de drop
            GameObject instantiatedItem = Instantiate(Inventory.instance.selectedItem.prefab);
            instantiatedItem.GetComponent<Item>().itemData = Inventory.instance.selectedItem;
            instantiatedItem.layer = LayerMask.NameToLayer("Item");
            instantiatedItem.transform.position = Inventory.instance.dropPoint.position;
            //chercher l'équipement porté et le retirer
            switch (Inventory.instance.selectedItem.equipmentType)
            {
                case EquipmentType.Head:
                    headSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    //retirer l'équipement porté
                    equippedHeadItem = null;
                    break;
                case EquipmentType.Chest:
                    chestSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    //retirer l'équipement porté
                    equippedChestItem = null;
                    break;
                case EquipmentType.Hands:
                    handsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    //retirer l'équipement porté
                    equippedHandsItem = null;
                    break;
                case EquipmentType.Legs:
                    legsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    //retirer l'équipement porté
                    equippedLegsItem = null;
                    break;
                case EquipmentType.Feet:
                    feetSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    //retirer l'équipement porté
                    equippedFeetItem = null;
                    break;
                case EquipmentType.Weapon:
                    weaponSlotImage.sprite = Inventory.instance.emptySlotVisual;
                    //retirer l'équipement porté
                    equippedWeaponItem = null;
                    break;
            }
            playerStats.currentArmorPoints -= Inventory.instance.selectedItem.armorPoints;
            Inventory.instance.RefreshContent();
        }
        else
        {
            Debug.LogError("No equipment prefab found for item: " + Inventory.instance.selectedItem.name);
        }
        CloseEquipmentActionPanel();
        Inventory.instance.selectedItem = null;
    }

    //méthode pour le bouton détruire équipement
    public void DestroyEquipmentActionButton()
    {
        EquipmentLibraryItem equipmentToUnequip = equipmentLibrary.content.Where(e => e.itemData == Inventory.instance.selectedItem).First();
        if (equipmentToUnequip != null)
        {
            //Réactiver les éléments conflictuels et désactiver le prefab de l'équipement
            for (int i = 0; i < equipmentToUnequip.ElementToDisable.Length; i++)
            {
                equipmentToUnequip.ElementToDisable[i].SetActive(true);
            }
            equipmentToUnequip.equipmentPrefab.SetActive(false);
            //chercher l'équipement porté et le détruire
        switch (Inventory.instance.selectedItem.equipmentType)
        {
            case EquipmentType.Head:
                headSlotImage.sprite = Inventory.instance.emptySlotVisual;
                //détruire l'équipement porté
                equippedHeadItem = null;
                break;
            case EquipmentType.Chest:
                chestSlotImage.sprite = Inventory.instance.emptySlotVisual;
                //détruire l'équipement porté
                equippedChestItem = null;
                break;
            case EquipmentType.Hands:
                handsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                //détruire l'équipement porté
                equippedHandsItem = null;
                break;
            case EquipmentType.Legs:
                legsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                //détruire l'équipement porté
                equippedLegsItem = null;
                break;
            case EquipmentType.Feet:
                feetSlotImage.sprite = Inventory.instance.emptySlotVisual;
                //détruire l'équipement porté
                equippedFeetItem = null;
                break;
            case EquipmentType.Weapon:
                weaponSlotImage.sprite = Inventory.instance.emptySlotVisual;
                //détruire l'équipement porté
                equippedWeaponItem = null;
                break;
        }
            Inventory.instance.RefreshContent();
            playerStats.currentArmorPoints -= Inventory.instance.selectedItem.armorPoints;
        }
        else
        {
            Debug.LogError("No equipment prefab found for item: " + Inventory.instance.selectedItem.name);
        }

        CloseEquipmentActionPanel();
        Inventory.instance.selectedItem = null;
    }

    //méthode pour le bouton équiper
    public void EquipItemActionButton()
    {
        EquipmentLibraryItem equipmentToEquip = equipmentLibrary.content.Where(e => e.itemData == Inventory.instance.selectedItem).First();
        if (equipmentToEquip != null)
        {
            //Mettre à jour l'interface des équipements
            switch (Inventory.instance.selectedItem.equipmentType)
            {
                case EquipmentType.Head:
                    DisablePreviousEquipmentEquiped(equippedHeadItem);
                    headSlotImage.sprite = Inventory.instance.selectedItem.visual;
                    equippedHeadItem = Inventory.instance.selectedItem;
                    break;
                case EquipmentType.Chest:
                    DisablePreviousEquipmentEquiped(equippedChestItem);
                    chestSlotImage.sprite = Inventory.instance.selectedItem.visual;
                    equippedChestItem = Inventory.instance.selectedItem;
                    break;
                case EquipmentType.Hands:
                    DisablePreviousEquipmentEquiped(equippedHandsItem);
                    handsSlotImage.sprite = Inventory.instance.selectedItem.visual;
                    equippedHandsItem = Inventory.instance.selectedItem;
                    break;
                case EquipmentType.Legs:
                    DisablePreviousEquipmentEquiped(equippedLegsItem);
                    legsSlotImage.sprite = Inventory.instance.selectedItem.visual;
                    equippedLegsItem = Inventory.instance.selectedItem;
                    break;
                case EquipmentType.Feet:
                    DisablePreviousEquipmentEquiped(equippedFeetItem);
                    feetSlotImage.sprite = Inventory.instance.selectedItem.visual;
                    equippedFeetItem = Inventory.instance.selectedItem;
                    break;
                case EquipmentType.Weapon:
                    DisablePreviousEquipmentEquiped(equippedWeaponItem);
                    weaponSlotImage.sprite = Inventory.instance.selectedItem.visual;
                    equippedWeaponItem = Inventory.instance.selectedItem;
                    Debug.Log("Equipped weapon: " + equippedWeaponItem.name);
                    break;
            }
            //Désactiver les éléments conflictuels et activer le prefab de l'équipement
            for (int i = 0; i < equipmentToEquip.ElementToDisable.Length; i++)
            {
                equipmentToEquip.ElementToDisable[i].SetActive(false);
            }
            equipmentToEquip.equipmentPrefab.SetActive(true);
            //Retirer l'item de l'inventaire pour éviter la duplication
            Inventory.instance.RemoveItem(Inventory.instance.selectedItem);
            playerStats.currentArmorPoints += Inventory.instance.selectedItem.armorPoints;
            Inventory.instance.RefreshContent();
        }
        else
        {
            Debug.LogError("No equipment prefab found for item: " + Inventory.instance.selectedItem.name);
        }
        Inventory.instance.selectedItem = null;
    }
}
