
using UnityEngine;

namespace Entity
{
    
    public abstract class GameEntity: MonoBehaviour
    {

        protected string guid;
        protected float minimumLoadedY;
        
        public abstract void die();

        //lazy initialization
        public string getID()
        {
            if (guid == null)
                guid = System.Guid.NewGuid().ToString();
            return guid;
        }

        public void updateBoundary(float eliminationY)
        {
            this.minimumLoadedY = eliminationY;
        }


    }
}
