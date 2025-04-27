using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField]
    private InputActionReference moveAction;
    

   

    [Header("Movement Variables")]
    [SerializeField]
    private float moveSpeed = 50f;

   
    
    
    private Rigidbody rb;

    //Classes
    private PlayerMotor pMotor;
    private PlayerInventory pInventory;
    private void Awake()
    {
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
}
