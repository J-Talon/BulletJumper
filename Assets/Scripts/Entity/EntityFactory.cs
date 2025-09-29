using System;
using Entity.Enemy;
using Entity.GamePickup;
using Entity.Projectiles;
using UnityEngine;

namespace Entity
{
    public static class EntityFactory
    {

        private static GameObject PROJECTILE_PREFAB = Resources.Load<GameObject>("bullet");
        private static GameObject AMMO_PICKUP_PREFAB = Resources.Load<GameObject>("Ammo");
        private static GameObject SEAGULL = Resources.Load<GameObject>("Seagull");
        private static Sprite EGG_SPRITE = Resources.Load<Sprite>("DynamicSprites/egg-32x32");
        
        private static GameManager manager;

        //the idea here is that you will be able to add entities to the game manager to track.
        //by gameManager.trackEntity(entity) or something similar
        public static void setActiveManager(GameManager newManager)
        { 
            manager = newManager;
        }
        
        

        public static Projectile createBullet(Vector2 transform, string ownerId, Vector2 direction, float speed)
        {
            return createProjectile(transform, ownerId,  direction, speed, null);
        }

        public static Projectile createEgg(Vector2 transform, string ownerId, Vector2 direction, float speed)
        {
            return createProjectile(transform, ownerId, direction, speed, EGG_SPRITE);
        }



        public static Projectile createProjectile(Vector2 transform, string ownerId, Vector2 direction, float speed, Sprite sprite)
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

            if (sprite != null)
            {
               SpriteRenderer renderer = bullet.GetComponent<SpriteRenderer>();
               renderer.sprite = sprite;
            }
            manager.addEntity(projectile);
            projectile.initialize(ownerId,direction,speed);
            return projectile;
        }


        public static Pickup createPickup(Vector2 transform)
        {
            if (AMMO_PICKUP_PREFAB == null)
            {
                Debug.Log("AMMO_PICKUP_PREFAB Not Found");
                return null;
            }
            
            GameObject ammo = GameObject.Instantiate(AMMO_PICKUP_PREFAB,new Vector3(transform.x, transform.y, 0), Quaternion.identity);
            Pickup pickup = ammo.GetComponent<Pickup>();
            pickup.setHasGravity(false);
            manager.addEntity(pickup);
            return pickup;
        }


        public static GameEnemy createSeagull(Vector2 transform) 
        {
            if (SEAGULL == null)
            {
                Debug.Log("SEAGULL Not Found");
                return null;
            }
            
            GameObject seagull = GameObject.Instantiate(SEAGULL,new Vector3(transform.x, transform.y, 0), Quaternion.identity);
            GameEnemy enemy = seagull.GetComponent<Seagull>();
            manager.addEntity(enemy);
            return enemy;
        }
    }
}