using System;
using System.Collections.Generic;
using Entity;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generation
{
    public abstract class Region
    {
        protected float minY, maxY;
        protected float minX, maxX;

        protected List<GameObject> platforms;

        protected bool generated;

        protected int plats;

        protected int pickups;

        protected float danger;
        //private generator gen;

        public Region(float minX, float maxX, float minY, float maxY, int plats, int pickups, float danger)
        {
            this.maxX = maxX;
            this.minY = minY;
            this.minX = minX;
            this.maxY = maxY;
    
            platforms = new List<GameObject>();
            generated = false;
            this.plats = plats;
            this.pickups = pickups;
            this.danger = danger;
        }


        public float getMinY()
        {
            return minY;
        }

        public float getMaxY()
        {
            return maxY;
        }

        public bool isGenerated()
        {
            return generated;
        }

        public abstract void generate();


        protected void spawnPlatform(float x, float y)
        {
            int size = (int)(Random.value * 3) + 1;
            int dir = Random.value > 0.5 ? 1 : -1;

            int i = 0;
            while (size > 0)
            {
                GameObject platform = PlatformFactory.CreatePlatform(x + (dir * i), y);
                platforms.Add(platform);
                
                i++;
                size--;
            }
        }

        public void clean()
        {
            foreach (GameObject plat in platforms)
            {
                GameObject.Destroy(plat);
            }
        }
    }
}