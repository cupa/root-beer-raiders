using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public Transform pivotPoint;
    private PlayerInputActions inputActions;
    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
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
        var horizontalInput = inputActions.Player.Move.ReadValue<Vector2>().x;
        if (horizontalInput != 0)
        {
            Debug.Log(horizontalInput);
        }
        var position = new Vector3(pivotPoint.position.x, transform.position.y, pivotPoint.position.z);
        transform.RotateAround(position, Vector3.up, (horizontalInput * -1) * speed * Time.deltaTime);
    }
}
