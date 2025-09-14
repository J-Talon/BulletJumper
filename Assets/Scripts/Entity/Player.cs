using System;
using Input;
using Item;
using UnityEngine;

namespace Entity
{
    public class Player: LivingEntity, InputListener
    {


        private const float MOVE_SPEED = 5;
        private bool onGround;
        private int facingDirection;

        private GameObject itemRenderer;
        private Gun holdingItem;
        private int bulletCount;

        private Vector2 moveAxis;
        private Rigidbody2D rigidBody;
        private Animator animator;

        private Vector3 mouseWorldPosition;
        
        
        //how far away to hold the item
        [SerializeField] public float itemOffsetDistance;
        
        [SerializeField] public int startingBullets;
        
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
            base.initID();
            ((InputListener)this).subscribe();
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            moveAxis = Vector2.zero;
            onGround = true;
            facingDirection = 1;
            mouseWorldPosition = Vector3.zero;
            bulletCount = startingBullets;

            itemRenderer = transform.GetChild(0).gameObject;
            holdingItem = new Rifle(itemRenderer);

            if (itemOffsetDistance <= 0)
                itemOffsetDistance = 1;
            
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

        

        private void movementUpdate()
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
                animator.SetBool("isMoving", true);


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
            
            
            float diff = mouseWorldPosition.x - transform.position.x;
            
            if (diff < 0 && facingDirection > 0)
            {
                facingDirection = -1;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
            else if (diff > 0 && facingDirection < 0)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                facingDirection = 1;
            }
        }


        public void FixedUpdate()
        {
            movementUpdate();


            if (holdingItem == null)
                return;
            
            
            Vector2 position = transform.position;
            Vector2 mousePosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
            Vector2 diff = mousePosition - position;
            diff.Normalize();
            diff *= itemOffsetDistance;

            if (facingDirection < 0)
                diff.x *= -1;
            
            holdingItem.transformUpdate(diff);
        }


        public void keyMovementVectorUpdate(Vector2 vector)
        {
            moveAxis = vector;
        }

        public void mousePositionUpdate(Vector2 mousePosition)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.Log("Camera main camera not found");
                return;
            }
            mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        }

        
        
        public void leftMousePress(float mouseValue)
        {
            if (holdingItem != null)
                holdingItem.fire(Time.fixedTime,bulletCount,guid,facingDirection);
            else
            {
                Debug.Log("holdingItem is null");
            }
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