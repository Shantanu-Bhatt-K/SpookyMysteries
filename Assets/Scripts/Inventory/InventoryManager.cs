using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Storage Fields")]
    [SerializeField]
    private int iSize = 10;
    [SerializeField]
    private Item[] storage;
    [HideInInspector]
    public static InventoryManager Instance;
    [Header("Inventory Viewer Fields")]
    [SerializeField]
    private float vRotateSpeed;
    [Header("Actions")]
    [SerializeField]
    private InputActionReference iViewAction;
    [SerializeField] private InputActionReference iDragModifier;
    [SerializeField] public InputActionReference iDragAction;
    [SerializeField] public InputActionReference iPickAction;

    [Header("InventoryViewer")]
    [SerializeField]
    private float iScreenRadius = 5f;
    private float iScreenHeight = 2f;

    [Header("Prefabs")]
    [SerializeField]
    private ItemContainer iContainer;

    [Header("Attachments")]
    [SerializeField]
    private GameObject viewModel;
    [SerializeField]
    private Camera mainCamera;

    private GameObject iScreen;
    [HideInInspector]
    public InventoryStorage iStorage;
    private InventoryViewer iViewer;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        iScreen = this.gameObject;
        iStorage = new InventoryStorage(storage);
        iViewer = new InventoryViewer(iViewAction,iPickAction, iDragAction, iDragModifier, iScreen,iStorage.GetInventory(),iScreenRadius, iScreenHeight, iContainer, viewModel, vRotateSpeed, mainCamera);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        iViewer.OnUpdate();
    }


}
