using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField]
    private InputActionReference moveAction;

    [HideInInspector]
    public static PlayerManager Instance;
   

    [Header("Movement Variables")]
    [SerializeField]
    private float moveSpeed = 50f;
    
    private Rigidbody rb;

    //Classes
    private PlayerMotor pMotor;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
        pMotor = new PlayerMotor(moveAction, rb, moveSpeed);
       
    }

    // Update is called once per frame
    void Update()
    {
        pMotor.OnUpdate();
    }

    private void FixedUpdate()
    {
        pMotor.OnFixedUpdate();
    }

    public void SetMovementEnabled(bool canMove)
    {
        if (pMotor != null)
            pMotor.canMove = canMove;
    }
}
