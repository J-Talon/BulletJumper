using UnityEngine;

namespace Item
{
    public abstract class Gun
    {

        protected GameObject renderer;
        protected float axisZRotation;
        protected Animator animator;
        protected int cooldown;
        protected int lastFireTime;
        
        //protected float recoilOffset;  //recoil effect
        public Gun(GameObject renderer)
        {
            this.renderer = renderer;
            axisZRotation = 0;
            animator =  renderer.GetComponent<Animator>();
            lastFireTime = 0;
            //recoilOffset = 0;
        }
        
        //return whether the gun fired successfully
        public abstract bool fire(float fixedTime, int playerBullets, string ownerId, int facingDirection);
        
        //return whether the gun can fire
        protected abstract bool canFire(float fixedTime, int playerBullets);


        //tick function to change the position the gun is pointing at
        public abstract void transformUpdate(Vector2 delta);
        
        
        
        
    }
}