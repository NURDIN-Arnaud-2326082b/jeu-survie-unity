using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //distance permettant au joueur de ramasser un item
    [SerializeField]
    private float pickupRange = 2.6f;

    public PickupBehavior playerPickupBehavior;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject pickupText;

    private int cpt;

    void Start()
    {
        cpt = 0;
        pickupText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //On vérifie si le joueur est proche d'un item
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, layerMask))
        {
            //On vérifie que l'objet est bien tagué "Item"
            if (hit.transform.CompareTag("Item"))
            {
                if (cpt == 0)
                {
                    pickupText.SetActive(true);
                    cpt = 1;
                }
                //message pour débuguer
                Debug.Log("Item proche");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //On appelle la fonction de ramassage
                    playerPickupBehavior.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
        }
        else
        {
            pickupText.SetActive(false);  
        }   
    }
}
