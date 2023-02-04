using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float Speed;
    public float JumpForce;
    public GameObject BulletPrefab;
    public float FireRate;
    public float GroundDistance;
    public float SideDistance;
}
