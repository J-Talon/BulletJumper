using System;
using Entity;
using UnityEngine;

namespace Item
{
    public class Rifle: Gun
    {
        
        public Rifle(GameObject reference, LayerMask ground): base(reference, ground)
        {
            cooldown = 1000; //
            //set the base sprites and animations here based on the gun
        }

        protected override bool canFire(float fixedTime, int playerBullets)
        {
            int millis = (int)(fixedTime * 1000);

            if (playerBullets <= 0)
                return false;

            return millis - lastFireTime >= cooldown;

        }


        public override bool fire(float fixedTime, Player player, int facingDirection, float holdMillis)
        {
            
            float[] millisHoldStages =  new float[]{1000,2000};
            int power = 0;
            int bullets = player.getPlayerBullets();
            while (power < millisHoldStages.Length && holdMillis > millisHoldStages[power])
                power++;
            
            
            const float SPEED = 8;
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

            if (power < 1)
            {
                if (!canFire(fixedTime, bullets))
                {
                    SoundManager.instance.playSound("click");
                    return false;
                }

                lastFireTime = (int)fixedTime * 1000;
                EntityFactory.createProjectile(spawnPosition, player.getID(), direction, SPEED);

                Vector2 recoil = direction * -10;
                player.push(recoil);
                animator.SetTrigger("onFire");
                SoundManager.instance.playSound("shot");
                return true;
            }
            else
            {
                if (!canFire(fixedTime, bullets))
                {
                    SoundManager.instance.playSound("click");
                    return false;
                }

                if (!(bullets >= power))
                {
                    SoundManager.instance.playSound("click");
                    return false;
                }

                lastFireTime = (int)fixedTime * 1000;

                const float OFFSET_RADS = 15 * Mathf.PI / 180f;
                float leftBulletX = (float)Math.Cos(radDegrees - OFFSET_RADS) * hypotenuse;
                float leftBulletY = (float)Math.Sin(radDegrees - OFFSET_RADS) * hypotenuse;
                
                float rightBulletX = (float)Math.Cos(radDegrees + OFFSET_RADS) * hypotenuse;
                float rightBulletY = (float)Math.Sin(radDegrees + OFFSET_RADS) * hypotenuse;

                Vector2 leftBulletDir = new Vector2(leftBulletX, leftBulletY).normalized;
                Vector2 rightBulletDir = new Vector2(rightBulletX, rightBulletY).normalized;
                leftBulletDir.x *= facingDirection;
                rightBulletDir.x *= facingDirection;
                
                EntityFactory.createProjectile(spawnPosition, player.getID(), direction, SPEED);
                EntityFactory.createProjectile(spawnPosition, player.getID(), leftBulletDir, SPEED);
                EntityFactory.createProjectile(spawnPosition, player.getID(), rightBulletDir, SPEED);

                Vector2 recoil = direction * (-10 * (power));
                player.push(recoil);
                
                SoundManager.instance.playSound("shot");
                animator.SetTrigger("onFire");
                return true;
            }
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