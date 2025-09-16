using System;
using Entity.Projectiles;
using UnityEngine;

namespace Entity
{
    public static class EntityFactory
    {

        private static GameObject PROJECTILE_PREFAB = Resources.Load<GameObject>("bullet");
        public static Projectile createProjectile(Vector2 transform, string ownerId, Vector2 direction, float speed)
        {
            if (PROJECTILE_PREFAB == null)
            {
                Debug.Log("PROJECTILE_PREFAB Not Found");
                return null;
            }
            
            float magnitude = direction.magnitude;
            float angleRads = magnitude == 0 ? 0 : (float)Math.Atan2(direction.y, direction.x);
            float angleDegrees = 180f * angleRads / Mathf.PI;
            Quaternion rotation = Quaternion.Euler(0, 0, angleDegrees - 90);  //90 degrees because the model
                                                                              //is vertical in the spritesheet
            

            GameObject bullet = GameObject.Instantiate(PROJECTILE_PREFAB,new Vector3(transform.x, transform.y, 0), rotation);
            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.initialize(ownerId,direction,speed);
            return projectile;
        }
    }
}