using UnityEngine;
using Collision = Scripts.Collision;

public class WalkingSFX : MonoBehaviour
{
    [SerializeField] private AudioSource walkingAudio;
    [SerializeField] private AudioSource runningAudio;
    [SerializeField] private int PinkID;
    private Scripts.Collision _sCol;
    private PlayerController _pc;
    
    void Start()
    {
        _sCol = GetComponentInParent<Collision>();
        _pc = GetComponentInParent<PlayerController>();
        if(walkingAudio == null) Debug.LogError("WalkingSFX: No walking audio source found");
        if(runningAudio == null) Debug.LogError("WalkingSFX: No running audio source found");
    }

    void FixedUpdate()
    {
        if (_sCol.onGround && _pc.hDirection != 0)
        {
            // Sprinting
            if (Input.GetKey(KeyCode.LeftShift) && _pc.activePlayerID == PinkID)
            {
                if (runningAudio.isPlaying) return;
                if (walkingAudio.isPlaying) walkingAudio.Stop();
                runningAudio.Play();
            }
            else
            {
                if(walkingAudio.isPlaying) return;
                if (runningAudio.isPlaying) runningAudio.Stop();
                walkingAudio.Play();
            }
        }
        else
        {
            if (walkingAudio.isPlaying) walkingAudio.Stop();
            if (runningAudio.isPlaying) runningAudio.Stop();
        }
    }
}
