using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : MonoBehaviour, IConsume
{
    public void Use(ConsumableItem item)
    {
        if (item is ItemPotion)
        {
            Debug.Log("Health potion consumed!");
        }

        if (item is ItemFood)
        {
            Debug.Log("Food consumed!");
        }
    }
}
