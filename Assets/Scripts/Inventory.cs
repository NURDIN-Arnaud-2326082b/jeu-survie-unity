using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Panel Reference")]
    //Contenu de l'inventaire
    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    //Référence aux slots de l'inventaire
    [SerializeField]
    private Transform inventorySlotsParent;

    //Référence au panel de l'inventaire
    [SerializeField]
    private GameObject inventoryPanel;

    //Taille maximale de l'inventaire
    const int inventorySize = 32;

    //Point de drop des items
    [SerializeField]
    private Transform dropPoint;
    //Item sélectionné pour les actions
    private ItemData selectedItem;

    //Référence au panneau d'action d'item
    [Header("Action Panel Reference")]
    [SerializeField]
    private GameObject actionPanel;

    //bouton de laction d'utilisation
    [SerializeField]
    private GameObject useItemButton;

    //bouton de laction d'équiper
    [SerializeField]
    private GameObject equipItemButton;

    //bouton de laction de drop
    [SerializeField]
    private GameObject dropItemButton;

    //bouton de laction de destruction
    [SerializeField]
    private GameObject destroyItemButton;

    //Sprite pour un slot vide
    [SerializeField]
    private Sprite emptySlotVisual;

    [Header("Equipment Panel Reference")]

    //Référence à l'image du slot tête
    [SerializeField]
    private UnityEngine.UI.Image headSlotImage;

    //Référence à l'image du slot plastron
    [SerializeField]
    private UnityEngine.UI.Image chestSlotImage;

    //Référence à l'image du slot jambes
    [SerializeField]
    private UnityEngine.UI.Image legsSlotImage;
    [SerializeField]

    //Référence à l'image du slot pieds
    private UnityEngine.UI.Image feetSlotImage;

    //Référence à l'image du slot mains
    [SerializeField]
    private UnityEngine.UI.Image handsSlotImage;

    //Références a l'équipement porté sur la tête
    private ItemData equippedHeadItem;
    //Références a l'équipement porté sur le plastron
    private ItemData equippedChestItem;
    //Références a l'équipement porté sur les jambes
    private ItemData equippedLegsItem;
    //Références a l'équipement porté sur les pieds
    private ItemData equippedFeetItem;
    //Références a l'équipement porté sur les mains
    private ItemData equippedHandsItem;
    //Référence aux slots de l'équipement
    [SerializeField]
    private Transform équipmentSlotsParent;

     //Référence au panneau d'action d'équipement
    [Header("Action Panel Reference")]
    [SerializeField]
    private GameObject actionEquipmentPanel;

    //bouton de l'action de déséquiper
    [SerializeField]
    private GameObject unequipEquipmentButton;

    //bouton de laction de drop
    [SerializeField]
    private GameObject dropEquipmentButton;

    //bouton de laction de destruction
    [SerializeField]
    private GameObject destroyEquipmentButton;

    

    //Référence à la bibliothèque d'équipements
    [SerializeField]
    EquipmentLibrary equipmentLibrary;

    //Singleton instance de l'inventaire
    public static Inventory instance;

    //Initialisation du singleton
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    public void Start()
    {
        //cacher l'inventaire au démarrage
        inventoryPanel.SetActive(false);
        //cacher le panneau d'action au démarrage
        actionPanel.SetActive(false);
        //cacher le panneau d'action équipement au démarrage
        actionEquipmentPanel.SetActive(false);

        //rafraîchir l'affichage de l'inventaire
        RefreshContent();
    }

    //Ajouter un item à l'inventaire
    public void AddItem(ItemData item)
    {
        //ajout de l'objet à l'inventaire
        content.Add(item);
        //rafraîchir l'affichage de l'inventaire
        RefreshContent();
        //message pour débuguer
        Debug.Log("Item added to inventory: " + item.name);
    }

    // Update is called once per frame
    public void Update()
    {
        //afficher/cacher l'inventaire avec la touche I
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanel.activeSelf)
            {
                CloseItemActionPanel();
                //cacher le tooltip si l'inventaire est fermé
                ToolTipSystem.instance.Hide();
                //cacher le panneau d'action équipement si l'inventaire est fermé
                CloseEquipmentActionPanel();
            }
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    //rafraîchir l'affichage de l'inventaire
    private void RefreshContent()
    {
        //vider tous les slots de l'inventaire
        for (int i = 0; i < inventorySlotsParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
        }
        //vider tous les slots de l'équipement
        for (int i = 0; i < équipmentSlotsParent.childCount; i++)
        {
            Slot currentSlot = équipmentSlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
        }
        //rafraîchir le contenu de l'inventaire en le remplissant
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = content[i];
            currentSlot.itemVisual.sprite = content[i].visual;
        }
        //rafraîchir le contenu de l'équipement en le remplissant
        if (equippedHeadItem != null)
        {
            Slot headSlot = équipmentSlotsParent.GetChild(0).GetComponent<Slot>();
            headSlot.item = equippedHeadItem;
            headSlot.itemVisual.sprite = equippedHeadItem.visual;
        }
        if (equippedChestItem != null)
        {
            Slot chestSlot = équipmentSlotsParent.GetChild(1).GetComponent<Slot>();
            chestSlot.item = equippedChestItem;
            chestSlot.itemVisual.sprite = equippedChestItem.visual;
        }
        if (equippedHandsItem != null)
        {
            Slot handsSlot = équipmentSlotsParent.GetChild(2).GetComponent<Slot>();
            handsSlot.item = equippedHandsItem;
            handsSlot.itemVisual.sprite = equippedHandsItem.visual;
        }
        if (equippedLegsItem != null)
        {
            Slot legsSlot = équipmentSlotsParent.GetChild(3).GetComponent<Slot>();
            legsSlot.item = equippedLegsItem;
            legsSlot.itemVisual.sprite = equippedLegsItem.visual;
        }
        if (equippedFeetItem != null)
        {
            Slot feetSlot = équipmentSlotsParent.GetChild(4).GetComponent<Slot>();
            feetSlot.item = equippedFeetItem;
            feetSlot.itemVisual.sprite = equippedFeetItem.visual;
        }
    }

    //méthode pour vérifier si l'inventaire est plein
    public bool IsFull()
    {
        //vérifier si l'inventaire est plein
        return content.Count >= inventorySize;
    }

    //méthode pour ouvrir le panneau d'action
    public void OpenItemActionPanel(ItemData item, UnityEngine.Vector3 slotPosition)
    {
        selectedItem = item;
        //vérifier si le slot est vide
        if (item == null)
        {
            CloseItemActionPanel();
            return;
        }
        //configurer les boutons d'action en fonction du type d'item
        switch (item.itemType)
        {
            case ItemType.Ressource:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(false);
                break;
            case ItemType.Equipment:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(true);
                break;
            case ItemType.Consumable:
                useItemButton.SetActive(true);
                equipItemButton.SetActive(false);
                break;
        }
        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }

    //méthode pour fermer le panneau d'action
    public void CloseItemActionPanel()
    {
        actionPanel.SetActive(false);
    }

    //méthode pour le bouton utiliser
    public void UseItemActionButton()
    {
        Debug.Log("Use item action");
        CloseItemActionPanel();
        selectedItem = null;
    }

    //méthode pour le bouton équiper
    public void EquipItemActionButton()
    {
        EquipmentLibraryItem equipmentToEquip = equipmentLibrary.content.Where(e => e.itemData == selectedItem).First();
        if (equipmentToEquip != null)
        {
            //Mettre à jour l'interface des équipements
            switch (selectedItem.equipmentType)
            {
                case EquipmentType.Head:
                    DisablePreviousEquipmentEquiped(equippedHeadItem);
                    headSlotImage.sprite = selectedItem.visual;
                    equippedHeadItem = selectedItem;
                    break;
                case EquipmentType.Chest:
                    DisablePreviousEquipmentEquiped(equippedChestItem);
                    chestSlotImage.sprite = selectedItem.visual;
                    equippedChestItem = selectedItem;
                    break;
                case EquipmentType.Hands:
                    DisablePreviousEquipmentEquiped(equippedHandsItem);
                    handsSlotImage.sprite = selectedItem.visual;
                    equippedHandsItem = selectedItem;
                    break;
                case EquipmentType.Legs:
                    DisablePreviousEquipmentEquiped(equippedLegsItem);
                    legsSlotImage.sprite = selectedItem.visual;
                    equippedLegsItem = selectedItem;
                    break;
                case EquipmentType.Feet:
                    DisablePreviousEquipmentEquiped(equippedFeetItem);
                    feetSlotImage.sprite = selectedItem.visual;
                    equippedFeetItem = selectedItem;
                    break;
            }
            //Désactiver les éléments conflictuels et activer le prefab de l'équipement
            for (int i = 0; i < equipmentToEquip.ElementToDisable.Length; i++)
            {
                equipmentToEquip.ElementToDisable[i].SetActive(false);
            }
            equipmentToEquip.equipmentPrefab.SetActive(true);
            //Retirer l'item de l'inventaire pour éviter la duplication
            content.Remove(selectedItem);
            RefreshContent();
        }
        else
        {
            Debug.LogError("No equipment prefab found for item: " + selectedItem.name);
        }
        
        CloseItemActionPanel();

        selectedItem = null;
    }

    //méthode pour le bouton drop item
    public void DropItemActionButton()
    {
        //instancier l'item à dropper au point de drop
        GameObject instantiatedItem = Instantiate(selectedItem.prefab);
        instantiatedItem.GetComponent<Item>().itemData = selectedItem;
        instantiatedItem.layer = LayerMask.NameToLayer("Item");
        instantiatedItem.transform.position = dropPoint.position;
        //retirer l'item de l'inventaire
        content.Remove(selectedItem);
        RefreshContent();
        CloseItemActionPanel();
        selectedItem = null;
    }

    //méthode pour le bouton détruire item
    public void DestroyItemActionButton()
    {
        //retirer l'item de l'inventaire
        content.Remove(selectedItem);
        RefreshContent();
        CloseItemActionPanel();
        selectedItem = null;
    }

    public void OpenEquipmentActionPanel(ItemData item, UnityEngine.Vector3 slotPosition)
    {
        selectedItem = item;
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
        EquipmentLibraryItem equipmentToUnequip = equipmentLibrary.content.Where(e => e.itemData == selectedItem).First();
        if (equipmentToUnequip != null)
        {
            //Réactiver les éléments conflictuels et activer le prefab de l'équipement
            for (int i = 0; i < equipmentToUnequip.ElementToDisable.Length; i++)
            {
                equipmentToUnequip.ElementToDisable[i].SetActive(true);
            }
            equipmentToUnequip.equipmentPrefab.SetActive(false);
            //Mettre à jour l'interface des équipements
            switch (selectedItem.equipmentType)
            {
                case EquipmentType.Head:
                    headSlotImage.sprite = emptySlotVisual;
                    equippedHeadItem = null;
                    break;
                case EquipmentType.Chest:
                    chestSlotImage.sprite = emptySlotVisual;
                    equippedChestItem = null;
                    break;
                case EquipmentType.Hands:
                    handsSlotImage.sprite = emptySlotVisual;
                    equippedHandsItem = null;
                    break;
                case EquipmentType.Legs:
                    legsSlotImage.sprite = emptySlotVisual;
                    equippedLegsItem = null;
                    break;
                case EquipmentType.Feet:
                    feetSlotImage.sprite = emptySlotVisual;
                    equippedFeetItem = null;
                    break;
            }
            //Ajouter l'équipement à l'inventaire pour éviter la perte
            AddItem(selectedItem);
            RefreshContent();
        }
        else
        {
            Debug.LogError("No equipment prefab found for item: " + selectedItem.name);
        }
        
        CloseEquipmentActionPanel();

        selectedItem = null;
    }

    //méthode pour le bouton drop item
    public void DropEquipmentActionButton()
    {
        //instancier l'équipement à dropper au point de drop
        GameObject instantiatedItem = Instantiate(selectedItem.prefab);
        instantiatedItem.GetComponent<Item>().itemData = selectedItem;
        instantiatedItem.layer = LayerMask.NameToLayer("Item");
        instantiatedItem.transform.position = dropPoint.position;
         //chercher l'équipement porté et le retirer
        switch (selectedItem.equipmentType)
        {
            case EquipmentType.Head:
                headSlotImage.sprite = emptySlotVisual;
                //retirer l'équipement porté
                equippedHeadItem = null;
                break;
            case EquipmentType.Chest:
                chestSlotImage.sprite = emptySlotVisual;
                //retirer l'équipement porté
                equippedChestItem = null;
                break;
            case EquipmentType.Hands:
                handsSlotImage.sprite = emptySlotVisual;
                //retirer l'équipement porté
                equippedHandsItem = null;
                break;
            case EquipmentType.Legs:
                legsSlotImage.sprite = emptySlotVisual;
                //retirer l'équipement porté
                equippedLegsItem = null;
                break;
            case EquipmentType.Feet:
                feetSlotImage.sprite = emptySlotVisual;
                //retirer l'équipement porté
                equippedFeetItem = null;
                break;
        }
        RefreshContent();
        CloseEquipmentActionPanel();
        selectedItem = null;
    }

    //méthode pour le bouton détruire équipement
    public void DestroyEquipmentActionButton()
    {
        //chercher l'équipement porté et le détruire
        switch (selectedItem.equipmentType)
        {
            case EquipmentType.Head:
                headSlotImage.sprite = emptySlotVisual;
                //détruire l'équipement porté
                equippedHeadItem = null;
                break;
            case EquipmentType.Chest:
                chestSlotImage.sprite = emptySlotVisual;
                //détruire l'équipement porté
                equippedChestItem = null;
                break;
            case EquipmentType.Hands:
                handsSlotImage.sprite = emptySlotVisual;
                //détruire l'équipement porté
                equippedHandsItem = null;
                break;
            case EquipmentType.Legs:
                legsSlotImage.sprite = emptySlotVisual;
                //détruire l'équipement porté
                equippedLegsItem = null;
                break;
            case EquipmentType.Feet:
                feetSlotImage.sprite = emptySlotVisual;
                //détruire l'équipement porté
                equippedFeetItem = null;
                break;
        }
        RefreshContent();
        CloseEquipmentActionPanel();
        selectedItem = null;
    }

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
        
        //ajouter l'équipement à l'inventaire pour éviter la perte
        AddItem(itemToDisable);
    }
   
}
