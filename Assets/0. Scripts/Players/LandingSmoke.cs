using UnityEngine;

public class LandingSmoke : MonoBehaviour
{
    public GameObject smokeEffect;
    void Start()
    {
        GetComponentInParent<PlayerController>().OnGrounded += SpawnLandingSmokes;
    }
    
    
    // The player object has already been deactivated therefore couldn't be found
    // private void OnDisable()
    // {
    //     GetComponentInParent<PlayerController>().OnGrounded -= SpawnLandingSmokes;
    // }

    void SpawnLandingSmokes()
    {
        Instantiate(smokeEffect, transform.position, Quaternion.identity);
    }
}
