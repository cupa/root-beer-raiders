using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 100.0f;
    public Transform pivotPoint;
    public float jumpForce = 10f;
    private float gravity = -9.8f;
    private float vSpeed = 0f;

    private PlayerInputActions inputActions;
    private Rigidbody rb;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        var previouslyGrounded = isGrounded;
        CheckGround();


        var horizontalInput = inputActions.Player.Move.ReadValue<Vector2>().x;
        var position = new Vector3(pivotPoint.position.x, transform.position.y, pivotPoint.position.z);
        transform.RotateAround(position, Vector3.up, (horizontalInput * -1) * speed * Time.deltaTime);

        var jump = inputActions.Player.Jump.IsPressed();
        if (isGrounded && jump)
        {
            Debug.Log("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, groundDistance, groundMask);
    }
}
