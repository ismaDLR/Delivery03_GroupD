using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // NOTE: Inventory UI slots support drag&drop,
    // implementing the Unity provided interfaces by events system

    public static Action<int> OnSell;
    public static Action<int> OnBuy;
    public static Action<int> OnChangeHealth;

    public Image Image;
    public TextMeshProUGUI AmountText;
    public Image Selected;

    private Canvas _canvas;
    private GraphicRaycaster _raycaster;
    private Transform _parent;
    public ItemSlot _slot;
    private ItemBase _item;
    private InventoryUI _inventory;

    public void Initialize(ItemSlot slot, InventoryUI inventory)
    {
        Image.sprite = slot.Item.ImageUI;
        Image.SetNativeSize();

        AmountText.text = slot.Amount.ToString();
        AmountText.enabled = slot.Amount > 1;

        _slot = slot;
        _item = slot.Item;
        _inventory = inventory;

        slot.Selected = false;
        Selected.gameObject.SetActive(slot.Selected);
    }

    public void OnClickItem()
    {
        RaycastHit2D hitData = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (hitData)
        {
            if (hitData.collider.gameObject == gameObject)
            {
                Selected.gameObject.SetActive(true);
                _slot.SelectedItem();
            }
            else
            {
                if (hitData.collider.tag == "Sell" && _slot.Selected)
                {
                    if (transform.parent.gameObject.tag == "Player")
                    {
                        GameObject inventoryShop = GameObject.FindGameObjectWithTag("Shop");

                        if (inventoryShop.GetComponentInChildren<ShopCoins>().GetCoins() - _slot.Item.Cost >= 0)
                        {
                            _inventory.Inventory.RemoveItem(_slot.Item);
                            inventoryShop.GetComponent<InventoryUI>().Inventory.AddItem(_slot.Item);
                            OnSell?.Invoke(_slot.Item.Cost);
                        }
                    }
                }
                else if (hitData.collider.tag == "Buy" && _slot.Selected)
                {
                    if (transform.parent.gameObject.tag == "Shop")
                    {
                        GameObject inventoryPlayer = GameObject.FindGameObjectWithTag("Player");

                        if (inventoryPlayer.GetComponentInChildren<PlayerCoins>().GetCoins() - _slot.Item.Cost >= 0)
                        {
                            _inventory.Inventory.RemoveItem(_slot.Item);
                            inventoryPlayer.GetComponent<InventoryUI>().Inventory.AddItem(_slot.Item);
                            OnBuy?.Invoke(_slot.Item.Cost);
                        }
                    }
                }
                else if (hitData.collider.tag == "Use" && _slot.Selected)
                {
                    if (transform.parent.gameObject.tag == "Player")
                    {
                        if (_item is ConsumableItem)
                        {
                            ConsumableItem consumableItem = (_item as ConsumableItem);
                            _inventory.Inventory.RemoveItem(_slot.Item);
                            OnChangeHealth?.Invoke(consumableItem.LifeRestore);
                        }
                    }
                }
                else
                {
                    Selected.gameObject.SetActive(false);
                    _slot.UnSelectedItem();
                }
            }
            
        }
        else
        {
            Selected.gameObject.SetActive(false);
            _slot.UnSelectedItem();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parent = transform.parent;

        // Start moving object from the beginning!
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        
        // We need a few references from UI
        if (!_canvas)
        {
            _canvas = GetComponentInParent<Canvas>();
            _raycaster = _canvas.GetComponent<GraphicRaycaster>();
        }
        
        // Change parent of our item to the canvas
        transform.SetParent(_canvas.transform, true);
        
        // And set it as last child to be rendered on top of UI
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Moving object around screen using mouse delta
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Find scene objects colliding with mouse point on end dragging
        RaycastHit2D[] hitData = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition));

        for (int i = 0; i < hitData.Length; i++)
        {
            if (hitData[i])
            {
                Debug.Log("Drop over object: " + hitData[i].collider.gameObject.name);
                GameObject inventoryPlayer = GameObject.FindGameObjectWithTag("Player");
                GameObject inventoryShop = GameObject.FindGameObjectWithTag("Shop");

                if (hitData[i].collider.gameObject.tag == "Shop" && _parent.gameObject != hitData[i].collider.gameObject)
                {
                    if (inventoryShop.GetComponentInChildren<ShopCoins>().GetCoins() - _slot.Item.Cost >= 0)
                    {
                        _parent = hitData[i].collider.gameObject.transform;
                        _inventory.Inventory.RemoveItem(_slot.Item);
                        inventoryShop.GetComponent<InventoryUI>().Inventory.AddItem(_slot.Item);
                        OnSell?.Invoke(_slot.Item.Cost);
                    }
                }
                else if (hitData[i].collider.gameObject.tag == "Player" && _parent.gameObject != hitData[i].collider.gameObject)
                {
                    if (inventoryPlayer.GetComponentInChildren<PlayerCoins>().GetCoins() - _slot.Item.Cost >= 0)
                    {
                        _parent = hitData[i].collider.gameObject.transform;
                        _inventory.Inventory.RemoveItem(_slot.Item);
                        inventoryPlayer.GetComponent<InventoryUI>().Inventory.AddItem(_slot.Item);
                        OnBuy?.Invoke(_slot.Item.Cost);
                    }
                }
                /*
                var consumer = hitData.collider.GetComponent<IConsume>();
                bool consumable = _item is ConsumableItem;

                if ((consumer != null) && consumable)
                {
                    (_item as ConsumableItem).Use(consumer);
                    _inventory.UseItem(_item);
                }
                */
            }
        }

        Selected.gameObject.SetActive(false);
        _slot.UnSelectedItem();

        // Changing parent back to slot
        transform.SetParent(_parent.transform);

        // And centering item position
        transform.localPosition = Vector3.zero;
    }
}
