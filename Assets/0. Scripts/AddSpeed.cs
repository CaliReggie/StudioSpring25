using UnityEngine;

public class AddSpeed : MonoBehaviour
{
    [SerializeField] private int amount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerLevel player = collision.gameObject.GetComponent<PlayerLevel>();
            player.AddRunXP(amount);
            /*// Spawn animation
            _sfx.PlaySound();
            _anim.SetTrigger("collected");*/
            Destroy(gameObject);
        }
    }
}
