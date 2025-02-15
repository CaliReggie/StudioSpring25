using UnityEngine;

public class RunningSmoke : MonoBehaviour
{
    private Scripts.Collision _col;
    private SpriteRenderer _rend;
    private Animator _anim;

    private bool _isGrounded;
    private PlayerController _pc;
    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponentInParent<Scripts.Collision>();
        _anim = GetComponent<Animator>();
        _rend = GetComponent<SpriteRenderer>();
        _pc = GetComponentInParent<PlayerController>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        // what a hack
        if (_col.onGround && Input.GetKey(KeyCode.LeftShift) && _pc.hDirection != 0)
        {
            _anim.enabled = true;
            _rend.enabled = true;
        }
        else
        {
            _anim.enabled = false;
            _rend.enabled = false;
        }
    }
}
