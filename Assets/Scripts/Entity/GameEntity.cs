
using UnityEngine;

namespace Entity
{
    
    public abstract class GameEntity: MonoBehaviour
    {

        protected int health;

        public abstract bool damage();
        public abstract void die();



    }
}
