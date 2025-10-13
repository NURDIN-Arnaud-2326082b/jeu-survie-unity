using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
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

    public void Start()
    {
        //cacher l'inventaire au démarrage
        inventoryPanel.SetActive(false);

        //rafraîchir l'affichage de l'inventaire
        RefreshContent();
    }
    public void AddItem(ItemData item)
    {
        //ajout de l'objet à l'inventaire
        content.Add(item);
        //rafraîchir l'affichage de l'inventaire
        RefreshContent();
        //message pour débuguer
        Debug.Log("Item added to inventory: " + item.name);
    }

    public void Update()
    {
        //afficher/cacher l'inventaire avec la touche I
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    private void RefreshContent()
    {
        //rafraîchir le contenu de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            inventorySlotsParent.GetChild(i).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = content[i].visual;
        }
    }
    
    public bool IsFull()
    {
        //vérifier si l'inventaire est plein
        return content.Count >= inventorySize;
    }
}
