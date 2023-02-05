using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/BulletSettings")]
public class BulletSettings : ScriptableObject
{
    public float DropSpeed;
    public float Speed;
    public float DestroyTime;
    public GameObject ExplosionEffect;
}
