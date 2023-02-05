using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBeer : MonoBehaviour
{
    public GameObject Credits;
    public GameObject PlayAgain;

    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.EnableRootBeer(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.DisableRootBeer();
        }
    }

    internal void TakeRootBeer()
    {
        Debug.Log("Root Beer Taken");
        Destroy(gameObject);
        PlayerController.Instance.EndGame();
        Credits.SetActive(true);
        PlayAgain.SetActive(true);
    }
}
