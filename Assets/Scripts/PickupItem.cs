using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //distance permettant au joueur de ramasser un item
    [SerializeField]
    private float pickupRange = 2.6f;

    public PickupBehavior playerPickupBehavior;

    // Update is called once per frame
    void Update()
    {
        //On vérifie si le joueur est proche d'un item
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
        {
            //On vérifie que l'objet est bien tagué "Item"
            if(hit.transform.CompareTag("Item"))
            {
                //message pour débuguer
                Debug.Log("Item proche");
                if(Input.GetKeyDown(KeyCode.E))
                {
                    //On appelle la fonction de ramassage
                    playerPickupBehavior.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
        }
    }
}
