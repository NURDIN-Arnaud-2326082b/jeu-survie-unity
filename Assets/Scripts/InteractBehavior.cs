using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InteractBehavior : MonoBehaviour
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

    [SerializeField]
    private GameObject pickaxeVisual;

    //Référence à l'item courant ramassé
    private Item currentItem;

    //Référence à l'objet récolté courant
    private Harvestable currentHarvestable;

    private Vector3 newItemOffset = new Vector3(0, 0.5f, 0);

    private Tool currentTool;

    [SerializeField]
    private GameObject axeVisual;

    private bool isBusy = false;

    //Fonction de ramassage d'un item
    public void DoPickup(Item item)
    {
        //éviter les actions multiples en même temps
        if (isBusy) return;
        isBusy = true;

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
        pickaxeVisual.SetActive(false);
        //réactiver le déplacement du joueur
        playerMoveBehaviour.canMove = true;
        enableToolFromEnum(currentTool, false);
        isBusy = false;
   }

   public void DoHarvest(Harvestable harvestable)
   {
        //éviter les actions multiples en même temps
        if (isBusy) return;
        isBusy = true;

        currentTool = harvestable.requiredTool;
        enableToolFromEnum(currentTool);
        currentHarvestable = harvestable;
         //jouer animation de récolte
        playerAnimator.SetTrigger("Harvest");
        //bloquer déplacement du joueur pendant l'animation
        playerMoveBehaviour.canMove = false;

        //message pour débuguer
        Debug.Log("Harvest action performed");
   }

    //coroutine pour gérer la récolte (appelée par l'event harvestable dans l'animation)
   IEnumerator BreakHarvestable()
   {
        Harvestable tmpHarvestable = currentHarvestable;
        //désactiver le layer de l'objet récolté pour éviter les collectes multiples avant la disparition de l'objet
        tmpHarvestable.gameObject.layer = LayerMask.NameToLayer("Default");
        if (tmpHarvestable.isDisableKinematics)
        {
            tmpHarvestable.GetComponent<Rigidbody>().isKinematic = false;
            tmpHarvestable.GetComponent<Rigidbody>().AddForce(transform.forward*800, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(tmpHarvestable.destroyDelay);

        for(int i = 0; i < tmpHarvestable.haverstableItems.Length; i++)
        {
            Resource resource = tmpHarvestable.haverstableItems[i];
            
            if (Random.Range(0, 101) < resource.dropChance)
            {
                GameObject droppedItem = Instantiate(resource.itemdata.prefab);
                droppedItem.transform.position = tmpHarvestable.transform.position + newItemOffset;
            }
            
        }
        //détruire l'objet récolté
        Destroy(tmpHarvestable.gameObject);
        //réactiver le déplacement du joueur
        playerMoveBehaviour.canMove = true;
   }

    public void enableToolFromEnum(Tool tool, bool enable = true)
    {
          switch (tool)
          {
                case Tool.Axe:
                 axeVisual.SetActive(enable);
                 break;
                case Tool.Pickaxe:
                 pickaxeVisual.SetActive(enable);
                 break;
                default:
                 pickaxeVisual.SetActive(false);
                 axeVisual.SetActive(false);
                 break;
          }
    }
}
