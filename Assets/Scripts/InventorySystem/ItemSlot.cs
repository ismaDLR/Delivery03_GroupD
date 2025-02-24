using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlot
{
    public ItemBase Item;
    public int Amount;
    public bool Selected;

    public ItemSlot(ItemBase item)
    {
        this.Item = item;
        Amount = 1;
    }

    internal bool HasItem(ItemBase item)
    {
        return (item == Item);
    }

    internal bool CanHold(ItemBase item)
    {
        if (item.IsStackable) return (item == Item);

        return false;
    }

    internal void AddOne()
    {
        Amount++;
    }

    internal void RemoveOne()
    {
        Amount--;
    }

    internal void SelectedItem()
    {
        Selected = true;
    }

    internal void UnSelectedItem()
    {
        Selected = false;
    }

    public bool IsEmpty()
    {
        return Amount < 1;
    }
}


