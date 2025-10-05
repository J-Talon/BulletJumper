using UnityEngine;

namespace Generation
{
    public class PlatformFactory
    {

        private static GameObject platform = Resources.Load<GameObject>("Platform");

        public static GameObject CreatePlatform(float x, float y)
        {
            if (platform == null)
            {
                Debug.LogError("No platform prefab found");
                return null;
            }
            GameObject plat = GameObject.Instantiate(platform,new Vector3(x,y, 0), Quaternion.identity);
            return plat;
        }
    }
}