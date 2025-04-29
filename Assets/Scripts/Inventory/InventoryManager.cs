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
    public InventoryManager Instance;

    [Header("Actions")]
    [SerializeField]
    private InputActionReference iViewAction;
    [SerializeField] private InputActionReference iModifier;
    [SerializeField] public InputActionReference iDrag;

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

    private GameObject iScreen;
    private InventoryStorage iStorage;
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
        iViewer = new InventoryViewer(iViewAction, iScreen,iStorage.GetInventory(),iScreenRadius, iScreenHeight, iContainer, viewModel);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
