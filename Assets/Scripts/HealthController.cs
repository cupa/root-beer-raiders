using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    public GameObject HealthBar;
    public GameObject Canvas;

    public event Action<int, int> OnHit;
    public event Action OnDeath;

    private GameObject HealthBarInstance;
    private Coroutine currentCoroutine;
    private Camera camera1;

    private void Start()
    {
        camera1 = Camera.main;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<AttackController>())
        {
            Hurt(1);
        }
    }

    private void Hurt(int amount)
    {
        CurrentHealth -= amount;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(CreateHealthBar(MaxHealth, CurrentHealth));
        OnHit?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Update()
    {
        if(HealthBarInstance != null)
        {
            var pos = transform.position;
            var position = new Vector3(pos.x, pos.y + .4f, pos.z);
            var screenPosition = camera1.WorldToScreenPoint(position);
            HealthBarInstance.transform.position = screenPosition;
        }
    }

    private IEnumerator CreateHealthBar(int maxHealth, int currentHealth)
    {
        if(HealthBarInstance != null)
        {
            Destroy(HealthBarInstance);
        }

        var pos = transform.position;
        var position = new Vector3(pos.x, pos.y + .4f, pos.z);
        var screenPosition = camera1.WorldToScreenPoint(position);
        HealthBarInstance = Instantiate(HealthBar, screenPosition, Quaternion.identity);
        HealthBarInstance.transform.SetParent(Canvas.transform);
        var slider = HealthBarInstance.GetComponent<Slider>();
        var newWidth = (float)currentHealth / (float)maxHealth;
        slider.value = newWidth;
        yield return new WaitForSeconds(2);
        Destroy(HealthBarInstance);
    }
}
