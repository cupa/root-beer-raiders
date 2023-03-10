using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EnemySettings")]
public class EnemySettings : ScriptableObject
{
    public float Speed;
    public float JumpForce;
    public GameObject BulletPrefab;
    public float FireRate;
    public float GroundDistance;
    public float DistanceThreshold;
    public float DistanceYThreshold;
    public float CheckDistanceTime;
    public float FollowDistanceThreshold;

    public float PercentChanceToJump;
    public float JumpDelay;

    public int MaxHealth;
}
