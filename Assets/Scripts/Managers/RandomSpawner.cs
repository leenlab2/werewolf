using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{

    public string spawnPointTag = "SpawnPoint";
    public bool alwaysSpawn = true;
    GameObject[] spawnPoints;

    public List<GameObject> prefabsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTag);
        spawnEnemies();
        
    }

    public void spawnEnemies()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            int randomPrefab = Random.Range(0, prefabsToSpawn.Count);
            if (alwaysSpawn)
            {
                GameObject pts = Instantiate(prefabsToSpawn[randomPrefab]);
                pts.transform.position = spawnPoint.transform.position;
            }
            else
            {
                int spawnOrNot = Random.Range(0, 2);
                if (spawnOrNot == 0)
                {
                    GameObject pts = Instantiate(prefabsToSpawn[randomPrefab]);
                    pts.transform.position = spawnPoint.transform.position;
                }
            }
        }
    }

}