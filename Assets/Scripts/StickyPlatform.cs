using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    [SerializeField] private Transform Parent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision Enter");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is on the platform");
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(Parent);
        }

    }
}
