using System;
using UnityEngine;
using System.Collections.Generic;
using Entity;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private GameObject platformPrefab;
    private Camera mainCamera;
    
    [SerializeField] public Player player;

    [SerializeField] public float platformRise;
    
    [SerializeField] public int platformCount = 40;

    [SerializeField] public int ammoAmount = 5;
    
    private float eliminationPoint;
    
    private GameObject ammoPrefab;
    private List<GameObject> activeAmmo;


    private float minPlatformHeight;
    private float height;

    //public UIDocument uiDocument;
    //public Label scoreText;
    //public int score;
    
    private Vector3 spawnPosition;
    private List<GameObject> activePlatforms;
    
    void Start()
    {
       // scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        platformPrefab = Resources.Load<GameObject>("Platform");
        spawnPosition = player.transform.position;
        if (platformPrefab == null)
            Debug.LogError("No platform prefab found");

//////////////////////////////         
        ammoPrefab = Resources.Load<GameObject>("Ammo");

        if (ammoPrefab == null)
            Debug.LogError("No bullet prefab found");
//////////////////////////////              

        mainCamera = Camera.main;
        if (mainCamera == null)
            throw new Exception("No main camera found");
        

        
        activePlatforms = new List<GameObject>();
        activeAmmo = new List<GameObject>();
        
        height = player.transform.position.y;
        minPlatformHeight = player.transform.position.y;
        worldUpdates();
    }

    
    void Update()
    {
        Transform transform = player.gameObject.transform;
        float cameraLevel =  Math.Max(transform.position.y, height); //height is the camera height
        minPlatformHeight = Math.Max(cameraLevel, minPlatformHeight);
        
        mainCamera.transform.position = new Vector3(spawnPosition.x, cameraLevel, mainCamera.transform.position.z);
        

        float cameraMin = mainCamera.ScreenToWorldPoint(new Vector3(0, mainCamera.pixelHeight, 0)).y;
        float diff = cameraMin - mainCamera.transform.position.y;
        eliminationPoint = mainCamera.transform.position.y - diff;

        if (transform.position.y < eliminationPoint)
        {
            player.die();
            return;
        }


        if (cameraLevel > height)
        {
            height = cameraLevel;
           // score = Mathf.FloorToInt(height);
           // scoreText.text = "Score: " + score;
            
           // Debug.Log(ScoreManager.Instance);
          //  ScoreManager.Instance.AddScore(score);
            worldUpdates();
        }

        height = cameraLevel;
        
        float width = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, 0, 0)).x;

        int reflect = transform.position.x > 0 ? -1 : 1;
        float boundary = Math.Abs(transform.position.x);
        if (boundary >= width)
        {
            if (player.isOnGround())
                player.setHorizontalMovementRestriction(-reflect);
            else
                player.push(new Vector2(reflect, 0), 0.35f); //7 is arbitrary (the number that seems to work well-ish)
        }
        else player.setHorizontalMovementRestriction(0);
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

        while (activeAmmo.Count > 0)
        {
            GameObject ammoPickup = activeAmmo[0];
            if (ammoPickup == null)
            {
                activeAmmo.RemoveAt(0);
                continue;
            }
            
            float pickupYCoord = ammoPickup.transform.position.y;
            if (pickupYCoord >= eliminationPoint)
                break;

            activeAmmo.RemoveAt(0);
            Destroy(ammoPickup);

        }
        
        

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
        
        
        bool ammoSpawnChance = (Random.value) > 0.7f;
        if (activeAmmo.Count < ammoAmount && ammoSpawnChance)
        {
            GameObject ammoDrop = Instantiate(ammoPrefab,new Vector3(coordinateX, (int)minPlatformHeight + 1, 0), Quaternion.identity);
            activeAmmo.Add(ammoDrop);
        }
        
        
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
}


