using UnityEngine;

public class AttackProjectile : Damage
{
    public bool disableAfterDamage;
    [SerializeField] private GameObject damageEffect;
    public override void DealDamage(GameObject collisionGameObject, Collision2D collider = null)
    {
        Health collidedHealth = collisionGameObject.GetComponent<Health>();
        if (collidedHealth != null)
        {
            if (collidedHealth.teamID != this.teamID)
            {
                collidedHealth.TakeDamage(damageAmount);
                // Spawn damage effect
                Instantiate(damageEffect, 
                    collider != null ? collider.GetContact(0).point: transform.position,
                    Quaternion.identity);
                if (disableAfterDamage)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
