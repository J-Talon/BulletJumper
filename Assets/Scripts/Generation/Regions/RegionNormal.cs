using System;
using System.Collections.Generic;
using Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generation
{
    public class RegionNormal: Region
    {
        
        public RegionNormal(float minX, float maxX, float minY, float maxY, int plats, int pickups, float danger): base(minX, maxX, minY, maxY,plats, pickups, danger)
        {
        }

        public override void generate()
        {
                generated = true;
                int platsGenerated = 0;
                float xDiff = maxX - minX;
                float yDiff = maxY - minY;
            
                while (platsGenerated < plats)
                {
                
                    float x = minX + (Random.value * xDiff);
                    float y = minY + (Random.value * yDiff);
                    spawnPlatform(x,y);
                    platsGenerated++;
                }
            
                HashSet<int> chosen = new HashSet<int>();
                int max = Math.Min(pickups, platforms.Count);
                while (chosen.Count < max)
                {
                    int index = Random.Range(0, platforms.Count);
                    chosen.Add(index);
                }


                foreach (int i in chosen)
                {
                    GameObject current = platforms[i];
                    if (current == null)
                        continue;

                    float y = current.transform.position.y;
                    float x =  current.transform.position.x;
                    
                    EntityFactory.createAmmoPickup(new Vector2(x, y + 1));
                }
                
        }
    }
}