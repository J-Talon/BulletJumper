
using UnityEngine;

namespace Entity.Enemy
{
    public abstract class GameEnemy:LivingEntity
    {
        public abstract void attack();
        
        public void OnCollisionEnter2D(Collision2D other)
        {
            bool compare = other.collider.CompareTag(gameObject.tag);
            if (compare)
            {
                Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
            }

            GameObject col = other.gameObject;
            Player player = col.GetComponent<Player>();
            if (player == null)
                return;

            player.damage();
        }
    }
}