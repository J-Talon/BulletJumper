

using System;
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
            //maybe create a hit animation here
            Destroy(gameObject);
        }

        public void initialize(String ownerId, Vector2 direction, float moveSpeed)
        {
            this.ownerID = ownerId;
            this.direction = direction;
            this.moveSpeed = moveSpeed;
            base.initID();
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

        public void onCollideEntity(GameObject entity)
        {
            
        }

        public void onCollideTerrain(GameObject terrain)
        {
            
        }
    }
}