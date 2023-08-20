using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType {Default, Food, Weapon, Instrument}
public class ItemObject : ScriptableObject
{
    public string itemName; // назание объекта
    public int maxAnount; // количество обьектов.
    public string itimeDescription; 
}
