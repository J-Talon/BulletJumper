using System;

namespace Entity.Enemy
{
    public class GameEnemy:LivingEntity
    {
        private void Awake()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public override void die()
        {
            GameManager.instance.removeEntity(getID());
            Destroy(gameObject);
        }

        public override bool damage()
        {
            return true;
        }
    }
}