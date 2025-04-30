using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class InventoryViewer
{
    private GameObject viewModel;
    private GameObject iScreen;
    private float iScreenRadius;
    private float iScreenHeight;
    private int iSize;
    private Item[] iStorage;
    private List<ItemContainer> iContainers = new List<ItemContainer>();
    private ItemContainer baseContainer;
    private float rotateSpeed;
    private bool isDragging = false;
    private Camera cam;

    
    private bool isPicking = false;
    private GameObject pickedItem;
    private int oldIndex;
    public InventoryViewer(InputActionReference _viewAction,InputActionReference _pickAction, InputActionReference _dragAction, InputActionReference _dragModifier, GameObject _iScreen, Item[] _storage, float _screenRadius, float _screenHeight, ItemContainer _baseContainer, GameObject _viewModel, float _rotateSpeed, Camera _cam)
    {
        this.cam = _cam;
        this.iScreen = _iScreen;
        _viewAction.action.performed += OnView;
        _dragModifier.action.started += ctx =>
        {
            isDragging = true;
            Debug.Log("Drag started");
        };
        _dragModifier.action.canceled += ctx =>
        {
            isDragging = false;
            Debug.Log("Drag canceled");
        };
        _dragAction.action.performed += OnDrag;
        _pickAction.action.started += OnPickStart;
        _pickAction.action.canceled += OnPickEnd;
        this.iSize = _storage.Length;
        this.iStorage = _storage;
        this.iScreenRadius = _screenRadius;
        this.iScreenHeight = _screenHeight;
        this.baseContainer = _baseContainer;
        this.viewModel = _viewModel;
        InitScreen();
        this.rotateSpeed = _rotateSpeed;
    }

    private void OnView(InputAction.CallbackContext ctx)
    {
        bool active = !viewModel.activeSelf;
        viewModel.SetActive(active);
        PlayerManager.Instance.SetMovementEnabled(!active);
        GameManager.Instance.SetBlur(active);
    }

    private void OnDrag(InputAction.CallbackContext ctx)
    {
        if (!isDragging) return;

        float dragDelta = -ctx.ReadValue<float>();

        
        viewModel.transform.Rotate(Vector3.up, dragDelta * rotateSpeed, Space.World);
        for(int i = 0; i <iContainers.Count; i++)
        {
            iContainers[i].transform.rotation = Quaternion.identity;
        }
    }

    private void OnPickStart(InputAction.CallbackContext ctx)
    {
        if(isDragging) return;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {

            if (hit.collider.CompareTag("Item"))
            {
                
                pickedItem = hit.collider.gameObject;
                isPicking = true;
                oldIndex = iContainers.IndexOf(hit.collider.transform.parent.gameObject.GetComponent<ItemContainer>());
            }
        }
    }
    public void OnUpdate()
    {
        if (isPicking)
        {
            Vector3 screenPosition = Mouse.current.position.ReadValue();
            screenPosition.z = 100;
            pickedItem.transform.position = cam.ScreenToWorldPoint(screenPosition);
        }
       
    }
    private void OnPickEnd(InputAction.CallbackContext ctx)
    {
        if(!isPicking) return;
        isPicking = false;
        Vector3 screenPosition = Mouse.current.position.ReadValue();
        Ray ray  = cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.collider.name);
            if (hit.collider.CompareTag("ItemContainer"))
            {
                Debug.Log("hit a Container");
               
                InventoryManager.Instance.iStorage.MoveItem(oldIndex, iContainers.IndexOf(hit.collider.GetComponent<ItemContainer>()), (check) =>
                {
                    if(check)
                    {
                        pickedItem.transform.SetParent(hit.collider.transform);
                    }
                });
            }
        }
        pickedItem.transform.localPosition = Vector3.zero;
        
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
