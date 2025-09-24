using System;
using Input;
using Item;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Entity
{
    public class Player: LivingEntity, InputListener
    {


        private const float MOVE_SPEED = 5;
        private const float HOR_MAX_SPEED = 6;
        private const float HOR_ACCELERATION_SCALE = 7.07f;
        
        private bool onGround;
        private int facingDirection;

        private GameObject itemRenderer;
        private Gun holdingItem;
        private int bulletCount;

        private Vector2 moveAxis;
        private float axisConstantSeconds;
        private float axisMagnitude;
        private float lockTime;
        
        private Rigidbody2D rigidBody;
        private Animator animator;
        private Vector2 impulsePush;

        private Vector3 mouseWorldPosition;
        private float mouseDownTime;


        [SerializeField]
        private float impulseDamping;
        
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

        //public UIDocument uiDocument;
        //private Label ammoText;
        
        public void Start()
        {

            //ammoText = uiDocument.rootVisualElement.Q<Label>("AmmoCount");
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
            holdingItem = new Rifle(itemRenderer, layerMask);
            itemRenderer.GetComponent<SpriteRenderer>().sortingOrder += 1;
            
            mouseDownTime = 0;
            impulsePush = Vector2.zero;
            
            axisConstantSeconds = 0;
            axisMagnitude = 0;
            lockTime = 0;

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
          ((InputListener)this).unsubscribe();
          SceneManager.LoadSceneAsync("Scenes/EndCard");
        }

        

        private void movementUpdate()
        {
            
            bool zero = (moveAxis.x == 0 && moveAxis.y == 0);
            float movementDelta = 0;

            if (zero)
            {
                axisConstantSeconds = Time.fixedTime;
                axisMagnitude = 0;
            }
   
            
            if (moveAxis.x == 0)
                animator.SetBool("isMoving", false);
            else
                animator.SetBool("isMoving", true);


            Vector2 velocity = rigidBody.linearVelocity;

            if (onGround)
            {
                Vector2 groundMovement = new Vector2(moveAxis.x * MOVE_SPEED + impulsePush.x, velocity.y + impulsePush.y);

                if (moveAxis.x == 0)
                {
                    if (moveAxis.y != 0)
                        groundMovement.y = moveAxis.y * MOVE_SPEED;
                }
                else
                {
                    //this fixes an issue where moving and jumping makes the player jump lower
                    if (moveAxis.y != 0)
                        groundMovement.y = moveAxis.y * MOVE_SPEED * 1.5f;
                }
                
                rigidBody.linearVelocity = groundMovement;
            }
            else
            {
                //gradual movement physics
                //m = max speed
                //s = acc
                float axisTime = Time.fixedTime - axisConstantSeconds;

                if (lockTime > 0)
                    axisMagnitude = 0;
                else
                    axisMagnitude = (float)Math.Min(axisTime, Math.Pow(MOVE_SPEED / HOR_ACCELERATION_SCALE,2));
                lockTime = Math.Max(0,lockTime - (Time.fixedDeltaTime / Time.timeScale));
                
                movementDelta = (float)Math.Min(HOR_ACCELERATION_SCALE * Math.Sqrt(axisMagnitude),MOVE_SPEED);
                
                
                float inAirMovementX = moveAxis.x * movementDelta + velocity.x + impulsePush.x;
                inAirMovementX = Math.Min(HOR_MAX_SPEED, inAirMovementX);
                inAirMovementX = Math.Max(-HOR_MAX_SPEED, inAirMovementX);
                
                rigidBody.linearVelocity = new Vector2(inAirMovementX, velocity.y + impulsePush.y);
            }
            
            impulsePush = Vector2.zero;
            
            
        }


        private void groundChecks()
        {
            float transformX = transform.position.x;
            float transformY = transform.position.y;
            Vector2 separationLeft = new Vector2(transformX - castSeparation, transformY + verticalCastOffset);
            Vector2 separationRight = new Vector2(transformX + castSeparation, transformY + verticalCastOffset);

            bool castLeft = Physics2D.Raycast(separationLeft, Vector2.down, castDistance, layerMask);
            bool castRight = Physics2D.Raycast(separationRight, Vector2.down, castDistance, layerMask);

            
            onGround = (castLeft || castRight);
            animator.SetBool("onGround",onGround);
        }


        private void directionUpdate()
        {
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


        private void itemProceduralAnimation()
        {
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

        
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("ammo"))
            {

                // ammoText.text = "Ammo: " + bulletCount;
                bulletCount += (startingBullets);

                // GameManager.ammoCollected(other);
            }
        }

        public void push(Vector2 vector)
        {
            push(vector, 0);
        }


        //lock time is the time in seconds to lock the horizontal movement from changing
        public void push(Vector2 vector, float lockTime)
        {
            impulsePush += vector;
            if (lockTime != 0)
                this.lockTime = lockTime;
            

        }
        
     
        ////////////////////////////////////////
        //inherited


        public void FixedUpdate()
        {
            groundChecks();
            movementUpdate();
            directionUpdate();
            itemProceduralAnimation();

        }


        public void keyMovementVectorUpdate(Vector2 vector)
        {
            moveAxis = vector;
            axisConstantSeconds = Time.fixedTime;
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
            mouseDownTime = Time.fixedTime * 1000f;
        }

        public void leftMouseRelease(float mouseValue)
        {
            float holdTime = (Time.fixedTime * 1000f) - mouseDownTime;
            if (holdingItem != null)
                holdingItem.fire(Time.fixedTime,this,facingDirection ,holdTime);
            else
            {
                Debug.Log("holdingItem is null");
            }
        }
        
        ///////////////////////////////////
        //getters / setters

        
        
        public int getPlayerBullets()
        {
            return bulletCount;
        }

        public void setPlayerBullets(int bulletCount)
        {
            this.bulletCount = bulletCount;
        }
        
        //////////////////////////////////////////
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