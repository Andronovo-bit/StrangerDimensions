using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int _cherryCount = 0;
    [SerializeField] private Text m_cherryCountText;
    [SerializeField] private AudioSource m_cherryPickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter");
        if (other.gameObject.CompareTag("Cherry"))
        {
            Destroy(other.gameObject);
            m_cherryPickupSound.Play();
            _cherryCount++;
            Debug.Log("Cherry Count: " + _cherryCount);
            m_cherryCountText.text = "Cherries: " + _cherryCount;
        }
    }

}
