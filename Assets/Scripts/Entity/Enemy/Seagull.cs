using System;
using Entity.GamePickup;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity.Enemy
{
    public class Seagull:GameEnemy
    {
        private GameEntity target;
        private Vector2 patrolPosition;
        private Rigidbody2D rigidBody;

        private const float ATTACK_COOLDOWN = 3; // 3 secs
        private const float PATROL_TIME = 3; // 5 secs
        private const float MOVE_SPEED = 3;
        
        
        private float periodTime;
        private float attackTime;
        private Animator animator;
        
        public void Start()
        {
            target = GameManager.instance.player;
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            periodTime = 0;
            attackTime = 0;
        }

        public void FixedUpdate()
        {
            float delta = (Time.fixedDeltaTime / Time.timeScale);
            periodTime += delta;
            attackTime += delta;
            
            if (periodTime > PATROL_TIME)
                newTargetPosition();
            moveToLocation(patrolPosition);

            if (attackTime >= ATTACK_COOLDOWN)
            {
                attack();
            }
        }
        
        
        
        
        private void moveToLocation(Vector2 targetPos)
        {
            Vector2 location = transform.position;
            Vector2 diff = targetPos - location;
            
            if (diff.magnitude < 0.1f)
            {
                rigidBody.linearVelocity = Vector2.zero;
                return;
            }

            diff.Normalize(); // vector to move towards
            diff *= MOVE_SPEED;
            rigidBody.linearVelocity = diff;
        }

        
        
        private void newTargetPosition()
        {
            GameManager manager = GameManager.instance;
            float boundsX = manager.getFrustumWidth();
            
            Vector2 targetPosition = target.gameObject.transform.position;
            Vector2 camera = manager.getCameraParams();

            float nextY = targetPosition.y + (Random.value * camera.y);

            float nextX = (Random.value * boundsX  * 2) - (boundsX + 1);
            
            patrolPosition = new Vector2(nextX, nextY);
            periodTime = 0;
        }


        public override void attack()
        {
            Vector2 direction = target.transform.position - transform.position;
            direction.Normalize();
            
            //(Vector2 transform, string ownerId, Vector2 direction, float speed)
            EntityFactory.createEgg(gameObject.transform.position, getID(), direction, 3);
            attackTime = 0;
            animator.SetTrigger("triggerAttack");
        }


        public override void die()
        {
            Pickup pickup = EntityFactory.createPickup(gameObject.transform.position);
            pickup.setHasGravity(true);
            base.die();
        }


        public override bool damage()
        {
            die();
            return true;
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            bool compare = other.collider.CompareTag(gameObject.tag);
            if (!compare)
                Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
            
        }
    }
}