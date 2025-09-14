
using UnityEngine;

namespace Entity
{
    
    public abstract class GameEntity: MonoBehaviour
    {

        protected string guid;
        

        protected void initID()
        {
            guid = System.Guid.NewGuid().ToString();
        }
        
        public abstract void die();

        public string getID()
        {
            return guid;
        }
    }
}
