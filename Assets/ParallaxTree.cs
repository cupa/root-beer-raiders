using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTree : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform PivotPoint;
    public float RotateSpeed;
    private PlayerInputActions inputActions;
    

    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalInput = inputActions.Player.Move.ReadValue<Vector2>().x;
        transform.RotateAround(PivotPoint.position, Vector3.up, (horizontalInput) * RotateSpeed * Time.deltaTime);
    }
}
