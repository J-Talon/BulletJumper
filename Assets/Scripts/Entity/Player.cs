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

        private int movementRestriction;

        private const float INVULERABILITY_MILLIS = 1000;
        private float lastDamageTime;


        private bool chargeNotified;

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

        [SerializeField] public int maxHealth = 3;

        public UIDocument uiDocument;
        private Label ammoText;
        private Label healthCount;
        
        public void Start()
        {

            ammoText = uiDocument.rootVisualElement.Q<Label>("AmmoCount");
            healthCount = uiDocument.rootVisualElement.Q<Label>("HealthCount");
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
            movementRestriction = 0;
            
            lastDamageTime = 0;
            health = maxHealth;
            chargeNotified = false;

            if (itemOffsetDistance <= 0)
                itemOffsetDistance = 1;

            ammoText.text = "Ammo: " + bulletCount;
            healthCount.text = "Health: " + maxHealth;
            
        }



        public void setGun()
        {
            itemRenderer = transform.GetChild(0).gameObject;
            holdingItem = new Rifle(itemRenderer, layerMask);
            SpriteRenderer rend = itemRenderer.GetComponent<SpriteRenderer>();
            Animator anim = itemRenderer.GetComponent<Animator>();
            
            anim.runtimeAnimatorController = anim.runtimeAnimatorController;
            
            
            rend.sortingOrder += 1;
            
        }




        //damages the player
        //returns whether the player was damaged
        public override bool damage()
        {
            float timeSeconds = Time.fixedTime;
            float timeDiff = (timeSeconds - lastDamageTime) * 1000;
            if (timeDiff < INVULERABILITY_MILLIS)
                return false;
            
            lastDamageTime = timeSeconds;
            
            health -= 1;
            healthCount.text = "Health: " + health;
            if (health <= 0)
            {
                this.die();
            }

            return true;
        }

        public void heal()
        {
            health = Math.Min(maxHealth, health + 1);
            healthCount.text = "Health: " + health;
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

            //the amount of time the player has been moving in a given direction (via input)
            // and the magnitude of their movement according to the movement function
            if (zero)
            {
                axisConstantSeconds = Time.fixedTime;
                axisMagnitude = 0;
            }
   
            //animator updates
            if (moveAxis.x == 0)
                animator.SetBool("isMoving", false);
            else
                animator.SetBool("isMoving", true);


            Vector2 velocity = rigidBody.linearVelocity;

            if (onGround)
            {
                //raw movement according to axis input and impulse force
                Vector2 groundMovement = new Vector2(moveAxis.x * MOVE_SPEED + impulsePush.x, velocity.y + impulsePush.y);
                
                //if the player is standing still (via input)
                if (moveAxis.x == 0)
                {
                    if (moveAxis.y != 0)
                        groundMovement.y = moveAxis.y * MOVE_SPEED;
                }
                else
                {
                    //if the player is moving horizontally, then add a bit more velocity to the jumping action of the player
                    //this fixes an issue where moving and jumping makes the player jump lower
                    if (moveAxis.y != 0)
                        groundMovement.y = moveAxis.y * MOVE_SPEED * 1.5f;
                }

                
                //movement restriction logic for the case when the player is at the boundary and is on ground
                if (movementRestriction != 0)
                {
                    int directionX = groundMovement.x > 0 ? 1 : -1;
                    if (directionX * movementRestriction > 0)
                        groundMovement.x = 0;
                }

                rigidBody.linearVelocity = groundMovement;
            }
            else
            {
                //logic for when player is airborne
                
                //gradual movement physics
                //m = max speed
                //s = acc
                
                //axisTime is the time since the player started moving via input via a certain axis
                float axisTime = Time.fixedTime - axisConstantSeconds;

                //if horizontal movement is locked, then don't worry about it
                //otherwise use a square root function to interpolate the movement back to maximum.
                //this fixes the jittering movement of the player when changing directions
                if (lockTime > 0)
                    axisMagnitude = 0;
                else
                    axisMagnitude = (float)Math.Min(axisTime, Math.Pow(MOVE_SPEED / HOR_ACCELERATION_SCALE,2));
                
                //update the locktime so that it runs out when it should and unlock player movement
                lockTime = Math.Max(0,lockTime - (Time.fixedDeltaTime / Time.timeScale));
                
                //the magnitude of movement according to the axis magnitude
                //based on the amount of time the player has been moving
                /*
                  formula:    
                  The point at which y = max_speed is derived as:
                  
                  max_speed = s * sqrt(y)
                  max_speed / s = sqrt(y)
                  y = (max_speed / s) ^ 2
                  So therefore axisMagnitude = min(timeElapsed, (max_speed/acceleration) ^ 2)
                  
                  we're basically clamping x such that it doesn't go beyond the first value where y = max_speed.
                  this is so that when we change directions we don't wait for like 10 minutes before something
                  actually happens. (the further beyond from where y first hits maxspeed axisMagnitude is, the less accurate the movement is)
                 */
                movementDelta = (float)Math.Min(HOR_ACCELERATION_SCALE * Math.Sqrt(axisMagnitude),MOVE_SPEED);
                
                float inAirMovementX = velocity.x + impulsePush.x;
                float magnitude = Math.Abs(inAirMovementX);
                float diff = MOVE_SPEED - magnitude;
                float axisAddition = moveAxis.x * movementDelta;
                
                //if the player is above max speed horizontally that's okay,
                //but don't add axis input into it unless it opposes the velocity. Allow it to gradually go 
                //back under threshold before adding axisAddition. And even so add it such that the max it goes to
                //is the max speed
                int axisDirection = (axisAddition < 0 ? -1 : 1);
                int velocityDirection = (inAirMovementX < 0 ? -1 : 1);

                if (axisDirection != velocityDirection)
                {
                    inAirMovementX += axisAddition;
                }
                else
                {
                    if (diff < 0)
                        axisAddition = 0;
                    else
                    {
                        axisAddition = Math.Min(Math.Abs(axisAddition), diff);
                        axisAddition *= axisDirection;
                    }
                    inAirMovementX += axisAddition;
                }
                
                
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

        
        //
        // void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (other.CompareTag("ammo"))
        //     {
        //
        //         // ammoText.text = "Ammo: " + bulletCount;
        //         bulletCount += (startingBullets);
        //
        //         // GameManager.ammoCollected(other);
        //     }
        // }

        public void setHorizontalMovementRestriction(int xAxis)
        {
            movementRestriction = xAxis;
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
            chargeNotified = false;
            float holdTime = (Time.fixedTime * 1000f) - mouseDownTime;
            if (holdingItem != null)
                holdingItem.fire(Time.fixedTime,this,facingDirection ,holdTime);
            else
            {
                Debug.Log("holdingItem is null");
            }
        }


        //this is called iteratively while the mouse is held down
        public void mouseHoldDown(float mouseValue)
        {
            if (holdingItem == null)
                return;

            if (chargeNotified)
                return;
            
            float holdTime = (Time.fixedTime * 1000f) - mouseDownTime;
            bool charged = holdingItem.isCharged(holdTime);

            if (!charged)
                return;
            
            chargeNotified = true;
            
            
            //Set UI here for the gun being charged
            
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
            ScoreManager.Instance.AddAmmo(bulletCount);
            ammoText.text = "Ammo: " + bulletCount;
        }

        public void addBullets(int bullets)
        {
            this.bulletCount += bullets;
            ScoreManager.Instance.AddAmmo(bulletCount);
            ammoText.text = "Ammo: " + bulletCount;
        }

        public void removeBullets(int bullets)
        {
            this.bulletCount = Math.Max(0, bulletCount - bullets);
        }

        public bool isOnGround()
        {
            return onGround;
        }

        public Vector2 getVelocity()
        {
            return rigidBody.linearVelocity;
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