using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Debug.Log("Add speed picked up by player " + player.PlayerID);
            player.AddSpeed(5);
            /*// Spawn animation
            _sfx.PlaySound();
            _anim.SetTrigger("collected");*/
            Destroy(gameObject);
        }
    }
}
