using System;
using Entity;
using UnityEngine;

namespace Item
{
    public class Rifle: Gun
    {
        
        public Rifle(GameObject reference): base(reference)
        {
            cooldown = 1000;
            //set the base sprites and animations here based on the gun
        }

        protected override bool canFire(float fixedTime, int playerBullets)
        {
            int millis = (int)(fixedTime * 1000);

            if (playerBullets <= 0)
                return false;

            return millis - lastFireTime >= cooldown;

        }


        public override bool fire(float fixedTime, int playerBullets, string ownerId, int facingDirection)
        {
            if (!canFire(fixedTime, playerBullets))
                return false;

            const float SPEED = 4;
            GameObject reference = base.renderer;
            Transform form = reference.transform;
            
            float hypotenuse = form.localScale.x;
            float radDegrees = axisZRotation * Mathf.PI / 180f;
            
            //deg = 180*rad / pi
            //deg * pi / 180 = rad
            
            //cos(theta) = a/h
            //a = cos(theta) * h
            
            //sin(theta) = o/h
            //h * sin(theta) = o
            
            float horizontal = (float)Math.Cos(radDegrees) * hypotenuse;
            float vertical = (float)Math.Sin(radDegrees) * hypotenuse;

            Vector2 direction = new Vector2(horizontal, vertical);
            direction.Normalize();
            direction.x *= facingDirection;
            
            Vector2 scale = direction * hypotenuse;
            Vector2 spawnPosition = new Vector2(form.position.x + scale.x, form.position.y + scale.y);

            EntityFactory.createProjectile(spawnPosition, ownerId, direction, SPEED);
           // animator.SetBool("onFire",true);
            
            return true;
        }

        
        public override void transformUpdate(Vector2 delta)
        {
            GameObject reference = base.renderer;
            Transform form = reference.transform;
            
            // soh cah toa
             Vector2 normal = delta.normalized; 
             double theta = normal.x == 0 ? 0 : Math.Atan2(normal.y,normal.x);
            theta = theta * (180f) / Mathf.PI;
            axisZRotation = (float)theta;
            
            form.localRotation = Quaternion.Euler(0, 0, (float)theta);
            form.localPosition = new Vector3(delta.x, delta.y, 0);
            //animator.SetBool("onFire",false);
        }
    }
}