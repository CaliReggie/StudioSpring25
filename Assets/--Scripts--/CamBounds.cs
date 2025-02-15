using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBounds : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
