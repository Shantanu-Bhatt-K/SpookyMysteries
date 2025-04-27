using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{

    public InputActionReference moveAction;


    private Vector3 moveAmount = Vector3.zero;
    private Rigidbody rb;

    private Matrix4x4 skewedMatrix;
    [SerializeField]
    private float walkSpeed=1f;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector2 _input = moveAction.action.ReadValue<Vector2>();
        Debug.Log(moveAmount);
        skewedMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        moveAmount = skewedMatrix.MultiplyPoint(new Vector3(_input.x, 0, _input.y));
    }


    public void Jump()
    {
        Debug.Log("Jump");
    }


    private void FixedUpdate()
    {
        rb.AddForce(moveAmount * walkSpeed);
    }
}
