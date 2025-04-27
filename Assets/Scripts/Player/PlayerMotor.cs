using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor
{

    public InputActionReference moveAction;


    private Vector3 moveAmount = Vector3.zero;
    private Rigidbody rb;

    private Matrix4x4 skewedMatrix;
    private float moveSpeed;


    public PlayerMotor(InputActionReference _moveAction,Rigidbody _rb, float _moveSpeed)
    {
        this.moveAction = _moveAction;
        this.rb = _rb;
        this.moveSpeed = _moveSpeed;
    }


    public void OnUpdate()
    {
        Vector2 _input = moveAction.action.ReadValue<Vector2>();
        skewedMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        moveAmount = skewedMatrix.MultiplyPoint(new Vector3(_input.x, 0, _input.y));
    }


    public void OnFixedUpdate()
    {
        rb.AddForce(moveAmount * moveSpeed);
    }
}
