using UnityEngine;

namespace Scripts
{
    public class Collision : MonoBehaviour
    {
        // System
        private Collider2D _col;
    
        [Header("Condition Variables")]
        [SerializeField] private GameObject groundRayObj;
        [SerializeField] private LayerMask groundMask;

        [SerializeField] private GameObject collisionRayObj;

        [Header("Collision State")] 
        public bool onGround;
        public bool onRightWall;
        public bool onLeftWall;
        public bool onWall;
    
        private Vector2 groundRayPos;
        private Vector2 collisionRayPos;

        [Header("Offsets")]
        [SerializeField] private float extraHeightTest;
        [SerializeField] private Vector2 leftOffset;
        [SerializeField] private Vector2 rightOffset;
        [SerializeField] private float collisionRadius;
        
        
        private void Start()
        {
            _col = GetComponent<CapsuleCollider2D>();
        }

        private void FixedUpdate()
        {
            groundRayPos = groundRayObj.transform.position;
            collisionRayPos = collisionRayObj.transform.position;
            onGround = IsGrounded();
            onWall = IsOnWall();
        }

        //Copy pasted from Celeste movement video from Mix & Jam youtube.
        void OnDrawGizmos()
        {
            collisionRayPos = collisionRayObj.transform.position;
            Gizmos.color = Color.red;

            //var positions = new Vector2[] { rightOffset, leftOffset };
            
            Gizmos.DrawWireSphere(collisionRayPos + rightOffset, collisionRadius);
            Gizmos.DrawWireSphere(collisionRayPos + leftOffset, collisionRadius);
        }
        
        //// COLLISION ////
    
        bool IsGrounded()
        {
            // BoxCast at the downwards bottom of the player with the size of player hitbox and only detects the ground
            RaycastHit2D hitGround = Physics2D.BoxCast(groundRayPos, new Vector2(_col.bounds.size.x , extraHeightTest),
                0f, Vector2.down, 0, groundMask);
            // Check if the BoxCast hits then change the color accordingly
            Color rayColor = hitGround.collider ? Color.green : Color.red;
            // Draw the BoxCast in Left, Right, Bottom order.
            Vector2 rayCastLength = new Vector2(0, -extraHeightTest);

            Debug.DrawRay(groundRayPos - new Vector2(_col.bounds.extents.x, 0), rayCastLength, rayColor);
            Debug.DrawRay(groundRayPos + new Vector2(_col.bounds.extents.x, 0), rayCastLength, rayColor);
            Debug.DrawRay(groundRayPos - new Vector2(_col.bounds.extents.x, extraHeightTest), Vector2.right * _col.bounds.size.x, rayColor);
            return hitGround.collider != null;
        }

        bool IsOnWall()
        {
            onRightWall = Physics2D.OverlapCircle(collisionRayPos + rightOffset, collisionRadius, groundMask);
            onLeftWall = Physics2D.OverlapCircle(collisionRayPos + leftOffset, collisionRadius, groundMask);

            return onRightWall || onLeftWall;
        }
        
    }
}