using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryStorage
{
    private Item[] iStorage;

    public InventoryStorage(int _iSize)
    {
        iStorage = new Item[_iSize];
    }
    public InventoryStorage(Item[] _iStorage)
    {
        this.iStorage = _iStorage;
    }

    public Item[] GetInventory()
    {
        return iStorage;
    }

    public void AddItem(Item _item, Action<bool> callback)
    {
        if(iStorage.Contains<Item>(_item))
        {
            callback?.Invoke(true);
            Debug.LogWarning("Item already Exists");
            return;
        }
        for(int i = 0; i < iStorage.Length; i++)
        {
            if (iStorage[i] == null)
            {
                iStorage[i] = _item;
                callback?.Invoke(true);
                return;
            }
        }
        callback?.Invoke(false);
    }

    public void RemoveItemAt(int _index, Action<bool> callback)
    {
        if(_index >= iStorage.Length || _index < 0)
        {
            callback?.Invoke(false);
            Debug.LogError("index out of bounds");
            return;
        }
        else
        {
            if (iStorage[_index] == null)
            {
                callback?.Invoke(true);
                Debug.LogWarning("index is empty");
                return;
            }
            else
            {
                iStorage[_index] = null;
                callback?.Invoke(true);
                Debug.Log("Index has been deleted");
                return;
            }
        }
    }

    public void MoveItem(int _initialIndex, int _finalIndex, Action<bool> callback)
    {
        if(_initialIndex < 0 || _initialIndex >= iStorage.Length)
        {
            callback?.Invoke(false);
            Debug.LogError("initial index out of bounds");
            return;
        }
        else if(_finalIndex < 0 || _finalIndex >= iStorage.Length)
        {
            callback?.Invoke(false);
            Debug.LogError("final index out of bounds");
            return;
        }
        else if (iStorage[_initialIndex] == null)
        {
            callback?.Invoke(false);
            Debug.LogError("initial index empty");
            return;
        }
        else
        {
            (iStorage[_initialIndex], iStorage[_finalIndex]) = (iStorage[_finalIndex], iStorage[_initialIndex]);
            callback?.Invoke(true);
            Debug.Log("Item moved succesfully");
            return;
        }
    }
}


    

