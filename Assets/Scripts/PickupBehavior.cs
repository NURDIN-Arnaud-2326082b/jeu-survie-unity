using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    //Référence au script de déplacement du joueur
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour; 

    //Référence à l'animator du joueur
    [SerializeField]
    private Animator playerAnimator;

    //Référence à l'inventaire du joueur
    [SerializeField]
    private Inventory inventory;

    //Référence à l'item courant ramassé
    private Item currentItem;

    //Fonction de ramassage d'un item
    public void DoPickup(Item item)
    {
        //vérifier si l'inventaire est plein
        if (inventory.IsFull())
        {
            Debug.Log("Inventory full");
            return;
        }

        currentItem = item;
        //jouer animation de ramassage
        playerAnimator.SetTrigger("Pickup");
        //bloquer déplacement du joueur pendant l'animation
        playerMoveBehaviour.canMove = false;

        //message pour débuguer
        Debug.Log("Pickup action performed");
    }

    //Méthode pour ajouter l'item à l'inventaire (appelée par un event dans l'animation)
    public void AddItemToInventory()
    {
        //Ajouter objets ramassés à l'inventaire
        inventory.AddItem(currentItem.itemData);
        //détruire l'objet ramassé
        Destroy(currentItem.gameObject);
        //vider la référence à l'item courant
        currentItem = null;
    }
   
    //Méthode pour réactiver le déplacement du joueur (appelée par un event dans l'animation)
   public void ReEnablePlayerMovement()
   {
        //réactiver le déplacement du joueur
        playerMoveBehaviour.canMove = true;
   }
}
