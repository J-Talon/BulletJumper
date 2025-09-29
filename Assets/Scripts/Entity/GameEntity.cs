
using UnityEngine;

namespace Entity
{
    
    public abstract class GameEntity: MonoBehaviour
    {

        protected string guid;
        protected float loadedX = 0;
        
        public abstract void die();

        //lazy initialization
        public string getID()
        {
            if (guid == null)
                guid = System.Guid.NewGuid().ToString();
            return guid;
        }
        

        public void updateBoundaryX(float eliminationX)
        {
            this.loadedX = eliminationX;
        }
        

        public float getWidthX()
        {
            return loadedX;
        }


    }
}
