using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Contenu de l'inventaire
    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    public void AddItem(ItemData item)
    {
        //ajout de l'objet à l'inventaire
        content.Add(item);
        //message pour débuguer
        Debug.Log("Item added to inventory: " + item.name);
    }
}
