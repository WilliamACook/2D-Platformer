using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/ItemData", order = 1)]
public class Item : ScriptableObject
{
    public ItemType type;
    public Sprite icon;
    public string displayName;
    public int maxStack;
}

[CreateAssetMenu(fileName = "ItemDataLibary", menuName = "ScriptableObject/ItemDataLibary", order = 2)]
public class ItemSOLibary : ScriptableObject
{
    private static ItemSOLibary instance;

    public static ItemSOLibary Instance
    {
        get
        {
            if(instance == null)
            {
                ItemSOLibary[] libaries = Resources.LoadAll<ItemSOLibary>("");
                if(libaries.Length > 0)
                {
                    instance = libaries[0];
                    if(libaries.Length > 1) { Debug.LogWarning("Muiltiple intances of ItemSOLibary asset found in Resources folder. First instance being used"); }
                }
                else
                {
                    instance = CreateInstance<ItemSOLibary>();

                    Debug.Log("No Instance of ItemSOLibary foind in Resource folder, New one made!");
                }
            }
            return instance;
        }
    }

    public ItemSOLibary[] itemDataObjects;
}

public enum ItemType
{
    NONE,
    POTION,
    COIN

}