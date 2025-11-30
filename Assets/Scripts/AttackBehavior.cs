using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Equipment equipmentManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && equipmentManager.equippedWeaponItem != null)
        {
            //Lancer l'animation d'attaque
            playerAnimator.SetTrigger("Attack");
            Debug.Log("Attack action performed");
        }
    }
}
