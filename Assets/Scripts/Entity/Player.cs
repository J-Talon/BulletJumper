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

        [SerializeField] public float castSeparation;
        [SerializeField] public float verticalCastOffset;
        
        [SerializeField]
        public float castDistance;
        
        public void Start()
        {
            ((InputListener)this).subscribe();
            rigidBody = GetComponent<Rigidbody2D>();
            moveAxis = Vector2.zero;
            castSeparation = 0;
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
            
            Vector2 separationLeft = new Vector2(castSeparation - transform.position.x, transform.position.y + verticalCastOffset);
            Vector2 separationRight = new Vector2(castSeparation + transform.position.x, transform.position.y + verticalCastOffset);

            bool castLeft = Physics2D.Raycast(separationLeft, Vector2.down, castDistance, layerMask);
            bool castRight = Physics2D.Raycast(separationRight, Vector2.down, castDistance, layerMask);

            onGround = (castLeft || castRight);
            Vector2 velocity = rigidBody.linearVelocity;

            if (onGround)
            {
                Vector2 groundMovement = new Vector2(moveAxis.x * MOVE_SPEED, velocity.y);
                
                if (moveAxis.x == 0 && moveAxis.y != 0)
                    groundMovement.y = moveAxis.y * MOVE_SPEED;
                else if (moveAxis.x != 0 && moveAxis.y != 0)
                {
                    //this fixes an issue where moving and jumping makes the player jump lower
                    groundMovement.y = moveAxis.y * MOVE_SPEED * 1.5f;
                }
                
                rigidBody.linearVelocity = groundMovement;
            }
            else
            {
                rigidBody.linearVelocity = new Vector2(moveAxis.x * MOVE_SPEED, velocity.y);
            }
            
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

        
        //this method draws white lines under the player for debugging in the unity editor
        //the lines disappear when the game is running
        private void OnDrawGizmos()
        {
            Vector2 separationLeft = new Vector2(transform.position.x - castSeparation,transform.position.y + verticalCastOffset);
            Vector2 separationRight = new Vector2(transform.position.x + castSeparation, transform.position.y + verticalCastOffset);
            
            Vector2 endLeft = separationLeft + (Vector2.down * castDistance);
            Vector2 endRight = separationRight + (Vector2.down * castDistance);
            
            Gizmos.DrawLine(separationLeft, endLeft);
            Gizmos.DrawLine(separationRight, endRight);
        }

    }
}