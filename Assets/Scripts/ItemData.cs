using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/new Item")]
public class ItemData : ScriptableObject
{
    //Nom de l'item
    public string name;

    //Sprite de l'item
    public Sprite visual;

    //Prefab de l'item
    public GameObject prefab;

    //Description de l'item
    public string description;

    //Type de l'item
    public ItemType itemType;

    //Type d'équipement (si applicable)
    public EquipmentType equipmentType;
}

//Enum pour les types d'items
public enum ItemType
{
    Ressource,
    Equipment,
    Consumable
}

//Enum pour les types d'équipements
public enum EquipmentType
{
    Head,
    Chest,
    Hands,
    Legs,
    Feet
}