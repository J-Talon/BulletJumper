using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject Platforms;
    public GameObject guy;
    public GameObject plat1;
    public GameObject plat2;
    public GameObject plat3;
    public GameObject plat4;
    public GameObject plat5;
    public GameObject deathWall;   

    public int platformCount = 20;

    Vector3 spawnPosition = new Vector3();

        private List<GameObject> spawnedPlatforms = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        spawnPosition.y += 10; 
        GameObject newPlatform = Instantiate(plat1, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat2, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat3, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat4, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat5, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);
                spawnPosition.y += 10;        
        newPlatform = Instantiate(plat2, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat3, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat4, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat5, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);
        
                spawnPosition.y += 10;        
        newPlatform = Instantiate(plat2, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat3, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat4, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);

        spawnPosition.y += 10;        
        newPlatform = Instantiate(plat5, spawnPosition, Quaternion.identity);
        spawnedPlatforms.Add(newPlatform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDeathWallHitPlatform(GameObject platform)
    {

        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            if (spawnedPlatforms[i] != null && 
                spawnedPlatforms[i].transform.position.y < deathWall.transform.position.y)
            {
                Destroy(spawnedPlatforms[i]);
                spawnedPlatforms.RemoveAt(i);

                Debug.Log("Death wall hit platform!");
                spawnPosition.y += Random.Range(.5f, 10f);
                spawnPosition.x = Random.Range(-10f, 10f);
                GameObject newPlatform = Instantiate(plat1, spawnPosition, Quaternion.identity);
                spawnedPlatforms.Add(newPlatform);
            }
        }

        // Debug.Log("diePlat?");
        // spawnPosition.y += Random.Range(.5f, 10f);
        // spawnPosition.x = Random.Range(-10f, 10f);
        // Destroy(collision.gameObject);
        // GameObject newPlatform = Instantiate(plat1, spawnPosition, Quaternion.identity);
        // spawnedPlatforms.Add(newPlatform);
    
    }

        // if( collision.gameObject.tag.Equals("plat1") == true )
        // {
        //     spawnPosition.y += Random.Range(.5f, 10f);
        //     spawnPosition.x = Random.Range(-10f, 10f);
        //     Destroy(spawnedPlatforms[3]);
        //     Instantiate(spawnedPlatforms[2], spawnPosition, Quaternion.identity);
        // }

        // if( collision.gameObject.tag.Equals("plat2") == true )
        // {
        //     spawnPosition.y += Random.Range(.5f, 10f);
        //     spawnPosition.x = Random.Range(-10f, 10f);
        //     Destroy(spawnedPlatforms[4]);
        //     Instantiate(spawnedPlatforms[3], spawnPosition, Quaternion.identity);
        // }

        // if( collision.gameObject.tag.Equals("plat3") == true )
        // {
        //     spawnPosition.y += Random.Range(.5f, 10f);
        //     spawnPosition.x = Random.Range(-10f, 10f);
        //     Destroy(spawnedPlatforms[0]);
        //     Instantiate(spawnedPlatforms[4], spawnPosition, Quaternion.identity);
        // }
        
        // if( collision.gameObject.tag.Equals("plat4") == true )
        // {
        //     spawnPosition.y += Random.Range(.5f, 10f);
        //     spawnPosition.x = Random.Range(-10f, 10f);
        //     Destroy(spawnedPlatforms[1]);
        //     Instantiate(spawnedPlatforms[0], spawnPosition, Quaternion.identity);
        // }
        
        // if( collision.gameObject.tag.Equals("plat5") == true )
        // {
        //     spawnPosition.y += Random.Range(.5f, 10f);
        //     spawnPosition.x = Random.Range(-10f, 10f);
        //     Destroy(spawnedPlatforms[2]);
        //     Instantiate(spawnedPlatforms[1], spawnPosition, Quaternion.identity);
        // }
}


