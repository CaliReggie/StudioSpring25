using UnityEngine;

public class HealthSFX : MonoBehaviour
{
    [SerializeField] private PlaySoundOnce hitSFX;
    [SerializeField] private PlaySoundOnce deathSFX;

    private Health _health;
    void Start()
    {
        _health = GetComponentInParent<Health>();
        _health.OnDespawn += PlayDeathSFX;
        _health.OnTakeDamage += PlayHitSFX;
    }

    void PlayDeathSFX()
    {
        deathSFX.PlaySound();
    }

    void PlayHitSFX()
    {
        hitSFX.PlaySound();
    }
}
