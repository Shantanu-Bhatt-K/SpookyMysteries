using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject inventoryScreen;
    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField] 
    Transform spawnPoint;
    [Header("Attachments")]
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private InputActionReference showInventoryAction;
    [SerializeField]
    private InputActionReference rotateInventoryAction;
    [Header("Inventory Variables")]
    [SerializeField]
    private int inventorySize = 10;
    [SerializeField]
    private float inventoryRadius = 5f;
    [SerializeField]
    float inventoryRotateSpeed = 1f;
    [SerializeField]
    private List<PlayerItem> allItems;

    private PlayerInventory pInventory;
    private void Awake()
    {
        InitPlayer();
        pInventory = new PlayerInventory(inventorySize, inventoryRotateSpeed, showInventoryAction, rotateInventoryAction, inventoryScreen, inventoryRadius, allItems, mainCam);
    }

    private void Update()
    {
        pInventory.OnUpdate();
    }

    private void FixedUpdate()
    {
        pInventory.OnFixedUpdate();
    }
    public void InitPlayer()
    {
       Instantiate(playerManager, spawnPoint.position, Quaternion.identity);
    }
}
