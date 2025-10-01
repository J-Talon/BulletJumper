using System;
using UnityEngine;

namespace Entity.GamePickup
{
    public abstract class Pickup : GameEntity
    {

        //destruction is handled by the game manager
        public abstract void onPickup(Player player);

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject collided = other.gameObject;
            Player player = collided.GetComponent<Player>();

            if (player == null)
                return;
            
            onPickup(player);
            die();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //background == background means no collision
            if (CompareTag(other.gameObject.tag))
                Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
            Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
            body.linearVelocity = Vector2.zero;
            
        }



        public override void die()
        {
            GameManager.instance.removeEntity(getID());
            Destroy(gameObject);
        }
        
        public void setHasGravity(bool gravity)
        {
            Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
            if (body == null)
                body = gameObject.AddComponent<Rigidbody2D>();
            
            body.gravityScale = (gravity ? 1 : 0);
            
        }
    }
}
