using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerSettings Settings;

    public Transform pivotPoint;

    private PlayerInputActions inputActions;

    internal void EnableRootBeer(RootBeer rootBeer)
    {
        this.rootBeer = rootBeer;
    }

    internal void DisableRootBeer()
    {
        rootBeer = null;
    }

    private Rigidbody rb;

    public LayerMask groundMask;
    bool isGrounded;

    private float nextFire = 0.0f;
    private bool facingLeft;

    public Transform FirePoint;
    public Transform SideCollision;

    public static PlayerController Instance;

    public GameObject Pistol;
    public GameObject Rootbeer;

    public GameObject YouDied;
    public GameObject PlayAgain;

    internal void EndGame()
    {
        GameOver = true;
        Pistol.SetActive(false);
        Rootbeer.SetActive(true);
    }

    private bool isSided;

    public static event Action PlayerJumpListeners;

    private HealthController healthController;
    private RootBeer rootBeer;
    private bool hasRootbeer;
    private SquirrelGraphics graphics;
    public bool GameOver;

    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
        rb = GetComponent<Rigidbody>();
        inputActions.Player.Fire.performed += Fire;
        inputActions.Player.Jump.performed += Jump;
        inputActions.Player.Start.performed += RestartGame;
        facingLeft = true;
        isSided = false;
        Instance = this;
        hasRootbeer = false;
        GameOver = false;

        healthController = GetComponent<HealthController>();
        healthController.CurrentHealth = Settings.MaxHealth;
        healthController.MaxHealth = Settings.MaxHealth;
        healthController.OnHit += OnHit;
        healthController.OnDeath += OnDeath;
        graphics = GetComponentInChildren<SquirrelGraphics>();
        graphics.Idle();
    }

    private void RestartGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnHit(int CurrentHealth, int MaxHealth)
    {
        Debug.Log(string.Format("Player hit: {0}/{1}", CurrentHealth, MaxHealth));
    }
    private void OnDeath()
    {
        Debug.Log("Player Dead");
        graphics.Die();
        GameOver = true;
        YouDied.SetActive(true);
        PlayAgain.SetActive(true);
    }

    private void OnDestroy()
    {
        inputActions.Player.Fire.performed -= Fire;
        inputActions.Player.Jump.performed -= Jump;
        inputActions.Player.Start.performed -= RestartGame;
        healthController.OnHit -= OnHit;
        healthController.OnDeath -= OnDeath;
    }

    private void OnEnable()
    {
        inputActions?.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if (!GameOver)
        {
            var previouslyGrounded = isGrounded;
            CheckGround();
            //CheckSide();
            if (isSided)
            {
                Debug.Log("Sided");
            }
            var horizontalInput = inputActions.Player.Move.ReadValue<Vector2>().x;
            if (horizontalInput != 0)
            {
                graphics.Run();
            }
            else
            {
                graphics.Idle();
            }
            //if(!isSided || IsTurningAround(horizontalInput))
            //{
            RotateAround(horizontalInput);
            //}

            FlipDirection(horizontalInput);
        }
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
        if (IsTurningAround(horizontalInput))
        {
            var currentScale = transform.localScale;
            currentScale.x *= -1;
            transform.localScale = currentScale;
            facingLeft = !facingLeft;
        }
    }

    private bool IsTurningAround(float horizontalInput)
    {
        return horizontalInput > 0 && facingLeft || horizontalInput < 0 && !facingLeft;
    }

    void Fire(InputAction.CallbackContext ctx)
    {
        if(!GameOver)
        {
            if (rootBeer == null)
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
            else
            {
                rootBeer.TakeRootBeer();
                hasRootbeer = true;
            }
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, Settings.GroundDistance, groundMask);
    }

    private void CheckSide()
    {
        isSided = Physics.Raycast(SideCollision.position, facingLeft ? Vector3.left : Vector3.right, Settings.SideDistance, groundMask);
    }
}
