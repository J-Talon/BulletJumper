using System;
using Entity;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generation
{
    public class RegionSeagull: RegionNormal
    {
        public RegionSeagull(float minX, float maxX, float minY, float maxY, int plats, int pickups, float danger): base(minX, maxX, minY, maxY,plats, pickups + 1, danger)
        {
        }


        public override void generate()
        {
            base.generate();

            float chance = 0;
            const int MAX_GULLS = 3;
            
            if (danger < 0)
                chance = Random.value * 0.1f;
            else
                chance = Math.Min(danger, 0.6f);

            int gulls = 0;
            do
            {
                float xDiff = maxX - minX;
                float yDiff = maxY - minY;
                float yLevel = minY + (yDiff / 2f) + (Random.value * yDiff);
                float x = minX + (xDiff * Random.value);
                EntityFactory.createSeagull(new Vector2(x, yLevel));

                gulls++;
            } while (chance > Random.value && gulls < MAX_GULLS);


        }
    }
}