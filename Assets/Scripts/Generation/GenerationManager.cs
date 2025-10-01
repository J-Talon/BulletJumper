using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Generation
{
    public class GenerationManager
    {
        private List<GameObject> activePlatforms;
        
        private GameObject ammoPrefab;
        private GameObject platformPrefab;

        private int maxPlatforms;
        private float platformRiseAmount;
        private float minPlatformHeight;
        private Vector2 spawnReferencePoint;

        private float enemyChance;
        private const float ENEMY_CHANCE_ACC = 0.03f;
        private const float SPAWN_REDUCT = 0.3f;

        public GenerationManager(int maxPlatforms, float platformRiseAmount, Vector2 spawnReferencePoint)
        {
            platformPrefab = Resources.Load<GameObject>("Platform");
            ammoPrefab = Resources.Load<GameObject>("Ammo");
            
            if (ammoPrefab == null)
                Debug.LogError("No bullet prefab found");
            
            if (platformPrefab == null)
                Debug.LogError("No platform prefab found");
            
            activePlatforms = new List<GameObject>();
            this.maxPlatforms = maxPlatforms;
            this.platformRiseAmount = platformRiseAmount;
            this.spawnReferencePoint = spawnReferencePoint;
            enemyChance = 0;
        }


        public void ascend(float yLevel, float frustumWidth, float eliminationPoint)
        {
            minPlatformHeight = yLevel;
            removePlatforms(eliminationPoint);
            if (activePlatforms.Count < maxPlatforms)
                generateWorld(frustumWidth, (maxPlatforms - activePlatforms.Count));
        }
        
        
        
        public void generateWorld(float frustumWidth, int amountToGenerate)
        {
            Debug.Log("generating: "+amountToGenerate);
            int amountGenerated = 0;
            int riseAttempts = 0;
            int MAX_ATTEMPTS = 2;
            
            while (amountGenerated < amountToGenerate)
            {
                if (activePlatforms.Count >= maxPlatforms)
                    break;

                minPlatformHeight += platformRiseAmount;
                bool riseRoll = Random.value > 0.95f;
                if (riseRoll)
                {
                    riseAttempts++;
                }

                if (riseAttempts >= MAX_ATTEMPTS || !riseRoll)
                {
                    minPlatformHeight += platformRiseAmount;
                    riseAttempts = 0;
                }

                float coordinateX = (Random.Range(-frustumWidth, frustumWidth) + spawnReferencePoint.x);
                float direction = Random.value;
                int offset = direction > 0.5f ? 1 : -1;
                
                const int MAX_SIZE = 3;
                int size = 0;
                const float THRESHOLD = 0.5f;

                bool hasDrop = Random.value > 0.7f;
                while (size < MAX_SIZE && Random.value > THRESHOLD)
                {
                    GameObject platform = GameObject.Instantiate(platformPrefab,new Vector3(coordinateX, (int)minPlatformHeight, 0), Quaternion.identity);
                    
                    if (hasDrop)
                    {
                        hasDrop = false;
                        EntityFactory.createPickup(new Vector2(coordinateX, (int)minPlatformHeight + platformPrefab.transform.localScale.y + 0.25f));
                    }
                    
                    coordinateX += +(offset * platformPrefab.transform.localScale.x);
                    activePlatforms.Add(platform);

                    amountGenerated++;
                    size++;
                    
                }
            }

            bool skipGeneration = Random.value > enemyChance;
            if (skipGeneration)
            {
                enemyChance += ENEMY_CHANCE_ACC + (0.3f * Mathf.Sin(minPlatformHeight));
                return;
            }

            enemyChance = Mathf.Max(0, enemyChance - SPAWN_REDUCT);
            float xIn = frustumWidth + 1;
            xIn = Random.value > 0.5f ? -(xIn) : xIn;
            float yIn = minPlatformHeight;

            EntityFactory.createSeagull(new Vector2(xIn, yIn));


        }
        
        
        
        
        //returns the number of platforms removed
        public int removePlatforms(float eliminationPoint)
        {
            if (activePlatforms.Count <= 0)
                return 0;
            

            int destroyed = 0;
            float yLevel = activePlatforms[0].transform.position.y;
            while (activePlatforms.Count > 0 && (yLevel < eliminationPoint))
            {
                GameObject current = activePlatforms[0];
                yLevel = current.transform.position.y;

                if (yLevel >= eliminationPoint)
                    break;
            
                activePlatforms.RemoveAt(0);
                GameObject.Destroy(current);
                destroyed++;
            }

            return destroyed;
        }




    }
}