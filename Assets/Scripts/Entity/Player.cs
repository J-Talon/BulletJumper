using Input;
using Item;
using UnityEngine;

namespace Entity
{
    public class Player: GameEntity, InputListener
    {


        private const float MOVE_SPEED = 5;
        private bool onGround;
        private Gun gun;

        private Vector2 moveAxis;
        private Rigidbody2D rigidBody;
        
        [SerializeField]
        public LayerMask layerMask;

        [SerializeField]
        public Vector2 groundCast;

        [SerializeField]
        public float castDistance;
        
        public void Start()
        {
            ((InputListener)this).subscribe();
            rigidBody = GetComponent<Rigidbody2D>();
            moveAxis = Vector2.zero;
            groundCast = Vector2.zero;
            castDistance = 0;
            onGround = true;
        }

        
        //damages the player
        //returns whether the player was damaged
        public override bool damage()
        {
            health -= 1;
            if (health <= 0)
            {
                this.die();
            }

            return true;
        }


        public override void die()
        {
          //do something related to a game over state here
        }

        public void FixedUpdate()
        {
            
            
            RaycastHit2D hit =
                Physics2D.BoxCast(transform.position, groundCast, 0, -transform.up, castDistance);
            if (hit)
            {
                onGround = true;
            }
            else onGround = false;
            //
            //
            //
            Debug.Log(onGround);
            
            
            Vector2 velocity = rigidBody.linearVelocity;
            rigidBody.linearVelocity = new Vector2(moveAxis.x * MOVE_SPEED, velocity.y);

            if (moveAxis.y != 0 && onGround)
                rigidBody.AddForce(new Vector2(0,moveAxis.y * MOVE_SPEED), ForceMode2D.Impulse);
        }


        public void keyMovementVectorUpdate(Vector2 vector)
        {
            moveAxis = vector;
        }

        public void mousePositionUpdate(Vector2 mousePosition)
        {
          //  Debug.Log(mousePosition);
        }

        public void leftMousePress(float mouseValue)
        {
          //  Debug.Log("left mouse press");
        }

        public void leftMouseRelease(float mouseValue)
        {
          //  Debug.Log("left mouse release");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (Vector2.down * castDistance));
        }

    }
}