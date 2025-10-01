using UnityEngine;

namespace Entity
{
    public abstract class LivingEntity : GameEntity
    {
        protected int health;
        protected bool alive;
        public abstract bool damage();
        
        public int getHealth()
        {
            return health;
        }

        public override void die()
        {
            GameManager.instance.removeEntity(getID());
            Destroy(gameObject);
        }
    }
}