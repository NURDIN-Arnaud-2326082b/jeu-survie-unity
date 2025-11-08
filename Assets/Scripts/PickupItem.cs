using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //distance permettant au joueur de ramasser un item
    [SerializeField]
    private float pickupRange = 2.6f;

    //Référence au script comportment de ramassage
    public PickupBehavior playerPickupBehavior;

    //LayerMask pour ne détecter que les items
    [SerializeField]
    private LayerMask layerMask;

    //Référence au texte d'instruction de ramassage
    [SerializeField]
    private GameObject pickupText;

    //Compteur pour afficher le texte une seule fois
    private int cpt;

    // Start is called before the first frame update
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
            Debug.Log("Raycast hit: " + hit.transform.name);
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
