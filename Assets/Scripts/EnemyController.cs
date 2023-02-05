using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemySettings Settings;
    public Transform FirePoint;
    private TimeTracker timeTracker;

    private bool detectedPlayer;
    private bool facingLeft;
    private Rigidbody rb;
    private float nextFire;
    private HealthController healthController;
    private SquirrelGraphics graphics;

    void Start()
    {
        this.timeTracker = new TimeTracker(Settings.CheckDistanceTime);
        facingLeft = true;
        nextFire = 0.0f;
        FlipFacing();
        PlayerController.PlayerJumpListeners += () => PlayerJumped();

        healthController = GetComponent<HealthController>();
        healthController.CurrentHealth = Settings.MaxHealth;
        healthController.MaxHealth = Settings.MaxHealth;
        healthController.OnHit += OnHit;
        healthController.OnDeath += OnDeath;
        graphics = GetComponentInChildren<SquirrelGraphics>();
        graphics.Idle();
    }

    private void OnDestroy()
    {
        PlayerController.PlayerJumpListeners -= () => PlayerJumped();
        healthController.OnHit -= OnHit;
        healthController.OnDeath -= OnDeath;
    }

    private void OnHit(int CurrentHealth, int MaxHealth)
    {
        Debug.Log(string.Format("Enemy hit: {0}/{1}", CurrentHealth, MaxHealth));
    }
    private void OnDeath()
    {
        Debug.Log("Enemy Dead");
    }

    private void FlipFacing()
    {
        var currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingLeft = !facingLeft;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = PlayerController.Instance.transform.position;
        var currentY = new Vector3(0, transform.position.y, 0);
        var playerY = new Vector3(0, playerPosition.y, 0);
        if (timeTracker.HasTimePassed())
        {
            var yDistance = Vector3.Distance(currentY, playerY);
            var allDistance = Vector3.Distance(transform.position, playerPosition);
            detectedPlayer = yDistance <= Settings.DistanceYThreshold && allDistance <= Settings.DistanceThreshold;
            timeTracker.RestartTimer(Settings.CheckDistanceTime);
        }

        Vector3 direction = (playerPosition - transform.position).normalized;
        if(detectedPlayer)
        {
            if (direction.x > 0 && facingLeft || direction.x < 0 && !facingLeft)
            {
                FlipFacing();
            }

            Fire();
            var allDistance = Vector3.Distance(transform.position, playerPosition);
            if (allDistance >= Settings.FollowDistanceThreshold)
            {
                graphics.Run();
                var pivotPoint = PlayerController.Instance.pivotPoint;
                var position = new Vector3(pivotPoint.position.x, transform.position.y, pivotPoint.position.z);
                transform.RotateAround(position, Vector3.up, (facingLeft ? 1 : -1) * Settings.Speed * Time.deltaTime);
            } else
            {
                graphics.Idle();
            }
        }
    }

    void Fire()
    {
        var pivotPoint = PlayerController.Instance.pivotPoint;
        if (Time.time > nextFire)
        {
            graphics.Fire();
            nextFire = Time.time + Settings.FireRate;
            var bulletObject = Instantiate(Settings.BulletPrefab, FirePoint.position, FirePoint.rotation);
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.PivotPoint = pivotPoint;
            bullet.Forward = facingLeft;
        }
    }

    private void PlayerJumped()
    {
        if(detectedPlayer)
        {
            if(TryHit(Settings.PercentChanceToJump))
            {
                rb.AddForce(Vector3.up * Settings.JumpForce, ForceMode.Impulse);
            }
            
        }
    }

    public bool TryHit(float chance)
    {
        var rand = UnityEngine.Random.Range(0f, 100f);
        return rand <= chance;
    }
}
