using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 2f;

    private void Update()
    {
        transform.Rotate(Vector3.forward, 360 * Speed * Time.deltaTime);
    }
}
