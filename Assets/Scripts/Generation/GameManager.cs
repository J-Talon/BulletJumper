using System;
using System.Collections.Concurrent;
using UnityEngine;
using System.Collections.Generic;
using Entity;
using Generation;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    
    private Camera mainCamera;
    
    [SerializeField] public Player player;

    [SerializeField] public float platformRise;
    
    [SerializeField] public int platformCount = 50;

    [SerializeField] public float baseVerticalScrollRate = 0.25f;
    
    
    private float eliminationPoint;
    
    private Vector3 spawnPosition;
    
    private float minPlatformHeight;
    private float height;

    public UIDocument uiDocument;
    public Label scoreText;
    public int score;
    
    private ConcurrentDictionary<string, GameEntity> entities;
    public static GameManager instance = null;

    private GenerationManager worldManager;
    private float frustumWidth;

    private void Awake()
    {
        //an important note here is the fact that this scene is loaded many times
        //this is important since static elements should still persist since the
        //VM didn't restart; It reloaded.
        
        //if this is an issue see script execution order. 
        //Edit > project settings > script execution order.
        instance = this;
        entities = new ConcurrentDictionary<string, GameEntity>();
        EntityFactory.setActiveManager(this);
     
    }

    void Start()
    {
        
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        spawnPosition = player.transform.position;
        
        mainCamera = Camera.main;
        if (mainCamera == null)
            throw new Exception("No main camera found");
        
        height = player.transform.position.y;
        minPlatformHeight = player.transform.position.y;
        worldManager = new GenerationManager(platformCount, platformRise,player.transform.position);
        
        frustumWidth = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, 0, 0)).x;
        worldManager.generateWorld(frustumWidth,platformCount);

    }

    
    void Update()
    {
        Transform transform = player.gameObject.transform;
        float cameraLevel =  Math.Max(transform.position.y, height); //height is the camera height
        
        if (cameraLevel > height)
        {
            height = cameraLevel;
            score = Mathf.FloorToInt(height);
            scoreText.text = "Score: " + score;
            
            ScoreManager.Instance.AddScore(score);
            worldUpdates();
        }
        else
        {
            float ascension = (Time.fixedDeltaTime / Time.timeScale) * baseVerticalScrollRate;
            cameraLevel += ascension;
        }
        
        mainCamera.transform.position = new Vector3(spawnPosition.x, cameraLevel, mainCamera.transform.position.z);


        float cameraMin = mainCamera.ScreenToWorldPoint(new Vector3(0, mainCamera.pixelHeight, 0)).y;
        float diff = cameraMin - mainCamera.transform.position.y;
        eliminationPoint = mainCamera.transform.position.y - diff;
        minPlatformHeight = Math.Max(mainCamera.transform.position.y + diff, minPlatformHeight);

        if (transform.position.y < eliminationPoint)
        {
            player.die();
            return;
        }  
        
        height = cameraLevel;
        
        

        int reflect = transform.position.x > 0 ? -1 : 1;
        float boundary = Math.Abs(transform.position.x);
        if (boundary >= frustumWidth)
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
        float width = frustumWidth - 1;
        worldManager.ascend(minPlatformHeight, width, eliminationPoint);
        updateBoundaries();
    }

    private void updateBoundaries()
    {
        foreach (KeyValuePair<string, GameEntity> pair in entities) {
            GameEntity entity = pair.Value;
          //  entity.updateBoundaryY(eliminationPoint);
        }
    }


    public void removeEntity(string uuid)
    {
        if (entities.ContainsKey(uuid))
            entities.Remove(uuid, out GameEntity entity);
    }

    public void addEntity(GameEntity entity)
    {
        bool res = entities.TryAdd(entity.getID(), entity);
    }

    public GameEntity getEntity(string uuid)
    {
        GameEntity entity = null;
       entities.TryGetValue(uuid, out entity);
        return entity;
    }

    public float getFrustumWidth()
    {
        return frustumWidth;
    }

    //returns camera transform y and 1/2 the width of the frustum
    public Vector2 getCameraParams()
    {
        float cameraMin = mainCamera.ScreenToWorldPoint(new Vector3(0, mainCamera.pixelHeight, 0)).y;
        float diff = cameraMin - mainCamera.transform.position.y;
        return new Vector2(mainCamera.transform.position.y, diff);
    }

    public float getSpawnX()
    {
        return spawnPosition.x;
    }
}


