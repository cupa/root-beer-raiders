using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBulletOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.DestroyBullet();
        }
    }
}
