using System;
using Entity;
using UnityEngine;

namespace Item
{
    public class Shotgun: Gun
    {
        
        public Shotgun(GameObject reference, LayerMask ground) : base(reference, ground)
        {
            cooldown = 1500;
        }

        protected override bool canFire(float fixedTime, int playerBullets)
        {
            throw new System.NotImplementedException();
        }
        
        public override bool fire(float fixedTime, Player player, int facingDirection, float holdMillis)
        {
            throw new System.NotImplementedException(); 
            
        }

        public override bool isCharged(float holdMillis)
        {
            throw new NotImplementedException();
        }


        public override void transformUpdate(Vector2 delta)
        {
            throw new System.NotImplementedException();
        }
    }
}