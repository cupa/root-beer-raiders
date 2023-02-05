using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTrigger : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject ZoomCamera;

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            MainCamera.SetActive(false);
            ZoomCamera.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            ZoomCamera.SetActive(false);
            MainCamera.SetActive(true);
        }
    }
}
