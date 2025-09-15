using System;
using UnityEngine;
using System.Collections.Generic;
using Entity;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private GameObject platformPrefab;
    private Camera mainCamera;
    
    [SerializeField] public Player player;

    [SerializeField] public float platformRise;
    
    [SerializeField] public int platformCount = 40;

    [SerializeField] public float eliminationOffset = 5;
    



    private float minPlatformHeight;
    private float height;
    
    private Vector3 spawnPosition;
    private List<GameObject> activePlatforms;
    
    void Start()
    {
        platformPrefab = Resources.Load<GameObject>("Platform");
        spawnPosition = player.transform.position;
        if (platformPrefab == null)
            Debug.LogError("No platform prefab found");

        mainCamera = Camera.main;
        if (mainCamera == null)
            throw new Exception("No main camera found");
        
        activePlatforms = new List<GameObject>();
        height = player.transform.position.y;
        minPlatformHeight = player.transform.position.y;
        worldUpdates();
    }

    
    void Update()
    {
        Transform transform = player.gameObject.transform;
        float cameraLevel =  Math.Max(transform.position.y, height);
        minPlatformHeight = Math.Max(cameraLevel, minPlatformHeight);
        
        mainCamera.transform.position = new Vector3(spawnPosition.x, cameraLevel, mainCamera.transform.position.z);

        if (cameraLevel > height)
        {
            height = cameraLevel;
            worldUpdates();
        }

        height = cameraLevel;
        
        float width = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, 0, 0)).x - 1;

        int reflect = transform.position.x > 0 ? -1 : 1;
        float boundary = Math.Abs(transform.position.x);
        if (boundary >= width)
            player.push(new Vector2(reflect * 7,0)); //7 is arbitrary (the number that seems to work well-ish)
        
    }


    private void worldUpdates()
    {
        int destroyed = removePlatforms();
        int diff = platformCount - destroyed;

        for (int i = 0; i < diff; i++)
            generateWorld();
    }


    //returns the number of platforms removed
    public int removePlatforms()
    {
        if (activePlatforms.Count <= 0)
            return 0;

        int destroyed = 0;
        float yLevel = activePlatforms[0].transform.position.y;
        while (activePlatforms.Count > 0 && (height - yLevel >= eliminationOffset))
        {
            GameObject current = activePlatforms[0];
            yLevel = current.transform.position.y;
            activePlatforms.RemoveAt(0);
            Destroy(current);
            destroyed++;
        }

        return destroyed;
    }

    public void generateWorld()
    {
        if (activePlatforms.Count > platformCount)
            return;
        
        float width = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, 0, 0)).x - 1;
       
        minPlatformHeight += platformRise;
        
        float coordinateX = (int)(Random.Range(-width, width) + spawnPosition.x);
        GameObject platform = Instantiate(platformPrefab,new Vector3(coordinateX, (int)minPlatformHeight, 0), Quaternion.identity);

        float roll = Random.value;
        const float THRESHOLD = 0.7f;

        GameObject platform2 = null;
        if (roll > THRESHOLD)
        {
            float direction = Random.value;
            int offset = direction > 0.5f ? 1 : -1;
            platform2 = Instantiate(platformPrefab, new Vector3(coordinateX + (offset * platformPrefab.transform.localScale.x), (int)minPlatformHeight,0),
                Quaternion.identity);
        }
        
        activePlatforms.Add(platform);
        if (platform2 != null)
            activePlatforms.Add(platform2);
        
    }



    public void OnDeathWallHitPlatform(GameObject platform)
    {

        // for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        // {
        //     if (spawnedPlatforms[i] != null && 
        //         spawnedPlatforms[i].transform.position.y < deathWall.transform.position.y)
        //     {
        //         Destroy(spawnedPlatforms[i]);
        //         spawnedPlatforms.RemoveAt(i);
        //
        //         Debug.Log("Death wall hit platform!");
        //         spawnPosition.y += Random.Range(.5f, 10f);
        //         spawnPosition.x = Random.Range(-10f, 10f);
        //         GameObject newPlatform = Instantiate(plat1, spawnPosition, Quaternion.identity);
        //         spawnedPlatforms.Add(newPlatform);
        //     }
        // }
    }
}


