using System;
using Input;
using Item;
using UnityEngine;

namespace Entity
{
    public class Player: GameEntity, InputListener
    {


        private const float MOVE_SPEED = 5;
        private bool onGround;
        private int facingDirection;
        private Gun gun;

        private Vector2 moveAxis;
        private Rigidbody2D rigidBody;
        private Animator animator;
        
        //layer mask for ground checks
        [SerializeField] public LayerMask layerMask;
        
        //x separation of ground check raycasts
        [SerializeField] public float castSeparation;
        
        //y offset of ground check raycasts
        [SerializeField] public float verticalCastOffset;
        
        //distance to perform the raycast
        [SerializeField] public float castDistance;
        
        public void Start()
        {
            ((InputListener)this).subscribe();
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            moveAxis = Vector2.zero;
            onGround = true;
            facingDirection = 1;
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

            float transformX = transform.position.x;
            float transformY = transform.position.y;
            Vector2 separationLeft = new Vector2(transformX - castSeparation, transformY + verticalCastOffset);
            Vector2 separationRight = new Vector2(transformX + castSeparation, transformY + verticalCastOffset);

            bool castLeft = Physics2D.Raycast(separationLeft, Vector2.down, castDistance, layerMask);
            bool castRight = Physics2D.Raycast(separationRight, Vector2.down, castDistance, layerMask);
            
            onGround = (castLeft || castRight);
            animator.SetBool("onGround",onGround);
            
            if (moveAxis.x == 0)
                animator.SetBool("isMoving", false);
            else
            {
                int moveDirection = moveAxis.x > 0 ? 1 : -1;
                if (moveDirection != facingDirection)
                {
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                    facingDirection = moveDirection;
                }

                animator.SetBool("isMoving", true);
            }


            Vector2 velocity = rigidBody.linearVelocity;

            if (onGround)
            {
                Vector2 groundMovement = new Vector2(moveAxis.x * MOVE_SPEED, velocity.y);

                if (moveAxis.x == 0 && moveAxis.y != 0)
                    groundMovement.y = moveAxis.y * MOVE_SPEED;
                else if (moveAxis.x != 0 && moveAxis.y != 0)
                {
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
            float transformX = transform.position.x;
            float transformY = transform.position.y;
            Vector2 separationLeft = new Vector2(transformX - castSeparation, transformY + verticalCastOffset);
            Vector2 separationRight = new Vector2(transformX + castSeparation, transformY + verticalCastOffset);

            Vector2 endLeft = separationLeft + (Vector2.down * castDistance);
            Vector2 endRight = separationRight + (Vector2.down * castDistance);
            
            Gizmos.DrawLine(separationLeft, endLeft);
            Gizmos.DrawLine(separationRight, endRight);
        }

    }
}