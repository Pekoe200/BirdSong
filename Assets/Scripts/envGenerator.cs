using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class envGenerator : MonoBehaviour
{
    public GameObject envPrefab; // Desired prefab to spawn
    public float spawnX; // Spawn locations for all axis
    public float spawnY;
    public float spawnZ;
    public float spawnRange; // Varied range for spawning prefab
    public float timeStart; // Start time for range
    public float timeEnd; // End time for range
    private float spawnTime; // Timer that is used for spawn check

    void Update(){
        // Updates location fo spawn to keep track with the bird
        spawnX = transform.position.x;
        spawnZ = transform.position.z;

        // Timer check for spawning prefabs
        spawnTime -= Time.deltaTime;
        if(spawnTime <= 0){
            Instantiate(envPrefab, new Vector3(spawnX,Random.Range(spawnY - spawnRange, spawnY + spawnRange),spawnZ), transform.rotation);
            spawnTime = Random.Range(timeStart,timeEnd);
        }        
    }
}
