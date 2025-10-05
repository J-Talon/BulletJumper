using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generation
{
    public class GenerationManager
    {

        private int maxPlatforms;
        private List<Region> regions;
        private Region activeRegion = null;

        private Vector2 reference;
        
        private int splits = 3;
        
        private int platsPerRegion;
        private float platRise;
        private float cameraHeight;

        private float dangerOffset;


        public GenerationManager(int maxPlatforms, float platformRiseAmount, Vector2 spawnReferencePoint, float cameraHeight)
        {
            regions = new List<Region>();
            platsPerRegion = Math.Max(maxPlatforms / splits, 1);
            reference = spawnReferencePoint;
            this.cameraHeight = cameraHeight;
            dangerOffset = 0;
        }


        //min plat height is the height above the frustum
        public void generate(float minPlatformHeight, float frustumWidth, float eliminationPoint)
        {
            
            if (activeRegion == null)
            {
                activeRegion = nextRegion(frustumWidth, minPlatformHeight);
                return;
            }

            float maxY = activeRegion.getMaxY();
            if (maxY >= eliminationPoint)
                return;
            
            activeRegion.clean();
            activeRegion = nextRegion(frustumWidth, minPlatformHeight);
        }


        private Region nextRegion(float frustumWidth, float minPlatSpawnHeight)
        {
            float minX = reference.x - frustumWidth;
            float maxX = reference.x + frustumWidth;
            
            if (regions.Count == 0)
            {
                Region starting = regionSelect(minX, maxX, minPlatSpawnHeight);
                starting.generate();
                regions.Add(starting);
            }

            Region next = regions[0];
            float y = regions[regions.Count - 1].getMaxY();
            
            
            while (regions.Count < splits)
            {
                Region newRegion = regionSelect(minX, maxX, y);
                newRegion.generate();
                y += cameraHeight;
                regions.Add(newRegion);
            }
            
            regions.RemoveAt(0);
            return next;
        }



        private Region regionSelect(float minX, float maxX, float y)
        {
            dangerOffset = (Random.value - Random.value) + 0.1f;
            Region newRegion;

            float choice = Random.value;
            if (choice > 0.5f)
            {
                newRegion = new RegionSeagull(minX, maxX, y, y + cameraHeight,platsPerRegion,2,dangerOffset);
                return newRegion;
            }
            else
            {
                return new RegionNormal(minX, maxX, y, y + cameraHeight,platsPerRegion,2,dangerOffset);
            }
        }





    }
}