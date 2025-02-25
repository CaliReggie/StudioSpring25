using System.Collections;
using UnityEngine;

public enum EPickupBuff
{
    Speed,
    Jump,
    Attack
}
public class Pickup : MonoBehaviour
{
    [SerializeField] private EPickupBuff pickupBuff;
    
    [SerializeField] private int amount;
    
    [SerializeField] private GameObject pickupSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerLevel player = collision.gameObject.GetComponent<PlayerLevel>();
            
            if (player == null)
            {
                return;
            }
            
            switch (pickupBuff)
            {
                case EPickupBuff.Speed:
                    player.AddRunXP(amount);
                    break;
                case EPickupBuff.Jump:
                    player.AddFlyXP(amount);
                    break;
                case EPickupBuff.Attack:
                    player.AddAtkXP(amount);
                    break;
            }
            
            if (pickupSound != null)
            {
                Instantiate(pickupSound, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
    }
}
