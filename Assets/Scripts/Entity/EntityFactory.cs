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

            GameObject bullet = GameObject.Instantiate(PROJECTILE_PREFAB,new Vector3(transform.x, transform.y, 0), Quaternion.identity);
            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.initialize(ownerId,direction,speed);
            return projectile;
        }
    }
}