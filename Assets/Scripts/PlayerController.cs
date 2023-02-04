using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerSettings Settings;

    public Transform pivotPoint;

    private PlayerInputActions inputActions;
    private Rigidbody rb;

    public LayerMask groundMask;
    bool isGrounded;

    private float nextFire = 0.0f;
    private bool facingLeft;

    public Transform FirePoint;

    public static PlayerController Instance;

    public static event Action PlayerJumpListeners;

    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
        rb = GetComponent<Rigidbody>();
        inputActions.Player.Fire.performed += Fire;
        inputActions.Player.Jump.performed += Jump;
        facingLeft = true;
        Instance = this;
    }

    private void OnDestroy()
    {
        inputActions.Player.Fire.performed -= Fire;
        inputActions.Player.Jump.performed -= Jump;
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
        RotateAround(horizontalInput);

        FlipDirection(horizontalInput);
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            PlayerJumpListeners?.Invoke();
            rb.AddForce(Vector3.up * Settings.JumpForce, ForceMode.Impulse);
        }
    }

    private void RotateAround(float horizontalInput)
    {
        var position = new Vector3(pivotPoint.position.x, transform.position.y, pivotPoint.position.z);
        transform.RotateAround(position, Vector3.up, (horizontalInput * -1) * Settings.Speed * Time.deltaTime);
    }

    private void FlipDirection(float horizontalInput)
    {
        if (horizontalInput > 0 && facingLeft || horizontalInput < 0 && !facingLeft)
        {
            var currentScale = transform.localScale;
            currentScale.x *= -1;
            transform.localScale = currentScale;
            facingLeft = !facingLeft;
        }
    }

    void Fire(InputAction.CallbackContext ctx)
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + Settings.FireRate;
            var bulletObject = Instantiate(Settings.BulletPrefab, FirePoint.position, FirePoint.rotation);
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.PivotPoint = pivotPoint;
            bullet.Forward = facingLeft;
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, Settings.GroundDistance, groundMask);
    }
}
