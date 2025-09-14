using Entity;
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
        protected LayerMask groundMask;
        
        //protected float recoilOffset;  //recoil effect
        public Gun(GameObject renderer, LayerMask ground)
        {
            this.renderer = renderer;
            axisZRotation = 0;
            animator =  renderer.GetComponent<Animator>();
            lastFireTime = 0;
            //recoilOffset = 0;
        }
        
        //return whether the gun fired successfully
        public abstract bool fire(float fixedTime, Player player, int facingDirection, float holdMillis);
        
        //return whether the gun can fire
        protected abstract bool canFire(float fixedTime, int playerBullets);


        //tick function to change the position the gun is pointing at
        public abstract void transformUpdate(Vector2 delta);
        
        
        
        
    }
}