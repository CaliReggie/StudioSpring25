using UnityEngine;

public class JumpingSmokes : MonoBehaviour
{
    public GameObject smokeEffect;
    public int FrogID;
    private PlayerController _pc;
    void Start()
    {
        GetComponentInParent<PlayerController>().OnFirstJump += SpawnJumpingSmoke;
        _pc = GetComponentInParent<PlayerController>();
    }

    // private void OnDisable()
    // {
    //     GetComponentInParent<PlayerController>().OnGrounded -= SpawnJumpingSmoke;
    // }

    void SpawnJumpingSmoke()
    {
        if (_pc.activePlayerID != FrogID) return;
        Instantiate(smokeEffect, transform.position, Quaternion.identity);
    }
}
