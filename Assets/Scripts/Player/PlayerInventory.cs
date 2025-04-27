using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerInventory
{
    private int inventorySize;
    private PlayerItem[] inventory;
    private InputActionReference showInventoryAction;
    private InputActionReference rotateInventoryAction;
    private GameObject inventoryScreen;
    private float inventoryRadius;
    private Dictionary<string, PlayerItem> itemDictionary = new Dictionary<string, PlayerItem>();
    private List<GameObject> inventoryModels = new List<GameObject>();
    private float rotateSpeed;
    private Camera cam;

    private float rotateAmount;
    private bool isSnapping = false;
    private Quaternion targetRotation;
    private float snapSpeed = 5f;
    public PlayerInventory(int _inventorySize, float _inventoryRotateSpeed, InputActionReference _showInventoryAction, InputActionReference _rotateInventoryAction, GameObject inventoryScreen, float _inventoryRadius,List<PlayerItem> _itemList, Camera _cam)
    {
        this.inventorySize = _inventorySize;
        this.rotateSpeed = _inventoryRotateSpeed;
        this.showInventoryAction = _showInventoryAction;
        this.rotateInventoryAction = _rotateInventoryAction;
        inventory = new PlayerItem[inventorySize];
        this.inventoryScreen = inventoryScreen;
        this.inventoryRadius = _inventoryRadius;
        this.cam = _cam;
        Debug.Log("is this working");
        for(int i = 0; i<_itemList.Count; i++)
        {
            itemDictionary.Add(_itemList[i].itemName, _itemList[i]);
        }
        for(int i = 0; i < inventorySize; i++)
        {
            GameObject _temp;
            if (inventory[i] == null)
            {
                _temp = GameObject.Instantiate(itemDictionary["Null"].itemModel, inventoryScreen.transform.position + new Vector3(inventoryRadius * Mathf.Cos(2 * Mathf.PI * i /inventorySize), 0, inventoryRadius * Mathf.Sin(2 * Mathf.PI * i / inventorySize)), Quaternion.identity);
            }
            else
            {
                _temp = GameObject.Instantiate(itemDictionary[inventory[i].itemName].itemModel, inventoryScreen.transform.position + new Vector3(inventoryRadius * Mathf.Cos(2 * Mathf.PI * i / inventorySize), 0, inventoryRadius * Mathf.Sin(2 * Mathf.PI * i / inventorySize)), Quaternion.identity);
            }
            _temp.transform.SetParent(inventoryScreen.transform);
            inventoryModels.Add( _temp );
        }
    }

    public PlayerItem[] GetInventory()
    {
        return inventory;
    }

    public void AddItem(PlayerItem _item, Action<bool> _callback)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = _item;
                _callback?.Invoke(true);
                return;
            }
        }
        _callback?.Invoke(false);
    }

    public void AddItemAt(PlayerItem _item, int _index, Action<bool> _callback)
    {
        if (_index < 0 || _index >= inventory.Length)
        {
            Debug.LogWarning("AddItemAt: Index out of bounds!");
            _callback?.Invoke(false);
        }
        else if (inventory[_index] == null)
        {
            inventory[_index] = _item;
            _callback?.Invoke(true);
        }
        else
        {
            _callback?.Invoke(false);
        }
    }

    public void RemoveItem(int _index, Action<bool> _callback)
    {
        if (_index < 0 || _index >= inventory.Length)
        {
            Debug.LogWarning("AddItemAt: Index out of bounds!");
            _callback?.Invoke(false);
        }
        else if (inventory[_index] == null)
        {
            Debug.LogWarning("Item doesnt exist");
            _callback?.Invoke(false);
        }
        else
        {
            inventory[_index] = null;
            _callback?.Invoke(true);
        }
    }

    public void MoveItem(int _initialIndex, int _finalIndex, Action<bool> _callback)
    {
        if (_initialIndex < 0 || _initialIndex >= inventory.Length || _finalIndex < 0 || _finalIndex >= inventory.Length)
        {
            Debug.LogWarning("MoveItem: Index out of bounds!");
            _callback?.Invoke(false);
            return;
        }

        if (inventory[_initialIndex] == null)
        {
            Debug.LogWarning("MoveItem: No item at initial index!");
            _callback?.Invoke(false);
            return;
        }

        if (inventory[_finalIndex] == null)
        {
            inventory[_finalIndex] = inventory[_initialIndex];
            inventory[_initialIndex] = null;
        }
        else
        {
            PlayerItem tempItem = inventory[_initialIndex];
            inventory[_initialIndex] = inventory[_finalIndex];
            inventory[_finalIndex] = tempItem;
        }
        _callback?.Invoke(true);
    }




    public void OnUpdate()
    {
        if (showInventoryAction.action.WasPressedThisFrame())
        {
            ShowInventory();
        }
        else if (showInventoryAction.action.WasReleasedThisFrame())
        {
            CloseInventory();
        }

        rotateAmount = rotateInventoryAction.action.ReadValue<float>();

        if (Mathf.Abs(rotateAmount) != 0)
        {
            isSnapping = false;
            inventoryScreen.transform.Rotate(Vector3.up, -rotateAmount * rotateSpeed * Time.deltaTime);
        }
        else if (!isSnapping)
        {
            // Start snapping when rotation input stops
            float currentY = inventoryScreen.transform.eulerAngles.y;

            // Find nearest snap angle
            float anglePerItem = 360f / inventorySize;
            float nearestSnapAngle = Mathf.Round(currentY / anglePerItem) * anglePerItem;

            targetRotation = Quaternion.Euler(0, nearestSnapAngle, 0);
            isSnapping = true;
        }
    }

    public void OnFixedUpdate()
    {
        if (isSnapping)
        {
            inventoryScreen.transform.rotation = Quaternion.Slerp(
                inventoryScreen.transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * snapSpeed
            );

            // Optional: stop snapping when close enough
            if (Quaternion.Angle(inventoryScreen.transform.rotation, targetRotation) < 0.5f)
            {
                inventoryScreen.transform.rotation = targetRotation;
                isSnapping = false;
            }
        }

        foreach (var model in inventoryModels)
        {
            model.transform.rotation = Quaternion.identity;
        }
    }





    private void ShowInventory()
    {
        inventoryScreen.SetActive(true);
    }

    private void CloseInventory()
    {
        inventoryScreen.SetActive(false);
    }
}
