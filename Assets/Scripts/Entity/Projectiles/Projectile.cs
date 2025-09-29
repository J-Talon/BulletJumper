
using System;
using Entity.Enemy;
using UnityEngine;

namespace Entity.Projectiles
{
    public class Projectile: GameEntity
    {
        protected string ownerID;
        protected Vector2 direction;
        protected float moveSpeed;
        private Rigidbody2D rigidBody;

        
        public override void die()
        {
            GameManager.instance.removeEntity(getID());
            Destroy(gameObject);
        }

        public void initialize(String ownerId, Vector2 direction, float moveSpeed)
        {
            this.ownerID = ownerId;
            this.direction = direction;
            this.moveSpeed = moveSpeed;
        }

        public void Awake()
        {
          rigidBody = GetComponent<Rigidbody2D>();
        }
        
        public void FixedUpdate()
        {
            Vector2 vector = direction * moveSpeed;
            rigidBody.linearVelocity = vector;
        }

        public void onCollideEntity(GameEntity gameEntity)
        {
            
            
            if (!(gameEntity is LivingEntity))
                return;
            Debug.Log(1);
            LivingEntity living = gameEntity as LivingEntity;
            string ownerId = living.getID();
            
            if (ownerId.Equals(ownerID))
                return;
            Debug.Log(2);

            GameEntity hit = GameManager.instance.getEntity(ownerId);
            GameEntity owner = GameManager.instance.getEntity(ownerID);

            if ((hit is GameEnemy) && (owner is GameEnemy))
                return;
            
            Debug.Log(hit == null);
            
            Debug.Log((hit is GameEnemy )+" "+ (owner is GameEnemy));

            living.damage();
            die();
        }


        public void onCollideTerrain(GameObject terrain)
        {
         //
        }


        public void OnTriggerEnter2D(Collider2D other)
        {
            GameEntity gameEntity = other.gameObject.GetComponent<GameEntity>();
            if (gameEntity == null)
                onCollideTerrain(other.gameObject);
            else 
                onCollideEntity(gameEntity);
        }

    }
}