using UnityEngine;

namespace Entity
{
    public abstract class LivingEntity : GameEntity
    {
        protected int health;
        public abstract bool damage();
        
        public int getHealth()
        {
            return health;
        }

    }
}