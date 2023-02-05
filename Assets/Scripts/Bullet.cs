using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform PivotPoint;
    public bool Forward;
    // Start is called before the first frame update

    public BulletSettings Settings;
    private TimeTracker timeTracker;

    void Start()
    {
        this.timeTracker = new TimeTracker(Settings.DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        var position = new Vector3(PivotPoint.position.x, transform.position.y, PivotPoint.position.z);
        transform.RotateAround(position, Forward ? Vector3.up : Vector3.down, Settings.Speed * Time.deltaTime);

        var currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + (Forward ? -1 : 1) * (Settings.DropSpeed * Time.deltaTime), currentPosition.z);
        transform.position = newPosition;

        if (timeTracker.HasTimePassed())
        {
            Instantiate(Settings.ExplosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(Settings.ExplosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
