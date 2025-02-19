using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeed : MonoBehaviour
{
    [SerializeField] private float amount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Debug.Log("Add speed picked up by player " + player.PlayerID);
            player.AddSpeed(amount);
            /*// Spawn animation
            _sfx.PlaySound();
            _anim.SetTrigger("collected");*/
            Destroy(gameObject);
        }
    }
}
