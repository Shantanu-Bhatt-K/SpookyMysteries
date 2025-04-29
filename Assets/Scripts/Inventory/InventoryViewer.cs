using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class InventoryViewer
{
    private InputActionReference viewAction;
    private GameObject viewModel;
    private GameObject iScreen;
    private float iScreenRadius;
    private float iScreenHeight;
    private int iSize;
    private Item[] iStorage;
    private List<ItemContainer> iContainers = new List<ItemContainer>();
    private ItemContainer baseContainer;
    public InventoryViewer(InputActionReference _viewAction, GameObject _iScreen, Item[] _storage, float _screenRadius, float _screenHeight, ItemContainer _baseContainer, GameObject _viewModel)
    {
        this.viewAction = _viewAction;
        this.iScreen = _iScreen;
        _viewAction.action.performed += OnView;
        this.iSize = _storage.Length;
        this.iStorage = _storage;
        this.iScreenRadius = _screenRadius;
        this.iScreenHeight = _screenHeight;
        this.baseContainer = _baseContainer;
        this.viewModel = _viewModel;
        InitScreen();
    }

    private void OnView(InputAction.CallbackContext ctx)
    {
        bool active = !viewModel.activeSelf;
        viewModel.SetActive(active);
        PlayerManager.Instance.SetMovementEnabled(!active);
        GameManager.Instance.SetBlur(active);
    }

    private void InitScreen()
    {
        for(int i = 0; i < iStorage.Length; i++)
        {
            iContainers.Add(GameObject.Instantiate(baseContainer));
            iContainers[i].transform.parent = viewModel.transform;
            iContainers[i].transform.localPosition = GetContainerPosition(i);
        }

        for(int i = 0; i < iStorage.Length; i++)
            if (iStorage[i] != null)
            {
                GameObject.Instantiate(iStorage[i], iContainers[i].transform, false);
            }
    }

    private Vector3 GetContainerPosition(int index)
    {
        float _angle = (2 * Mathf.PI * index / iSize) - (3 * Mathf.PI / 4);
        return new Vector3(iScreenRadius * Mathf.Cos(_angle), iScreenHeight, iScreenRadius * Mathf.Sin(_angle));
    }

}
