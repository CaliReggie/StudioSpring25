using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pickupPrefab;
    
    [SerializeField]
    private int maxPickups = 2;

    [SerializeField]
    private Vector2 spawnTime = new Vector2(3.5f, 7.5f);

    [SerializeField]
    private Transform[] spawnLocs;

    private List<GameObject> pickups;

    private float nextSpawnTime;

    private void Awake()
    {
        pickups = new List<GameObject>();
        
        nextSpawnTime = Time.time + UnityEngine.Random.Range(spawnTime.x, spawnTime.y);
        
        if (spawnLocs.Length == 0)
        {
            Debug.LogError("No spawn locations set for PickupSpawner");
            
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime && CanSpawn())
        {
            SpawnPickup();
            nextSpawnTime = Time.time + UnityEngine.Random.Range(spawnTime.x, spawnTime.y);
        }
    }
    
    private void SpawnPickup()
    {
        Transform spawnLoc = spawnLocs[UnityEngine.Random.Range(0, spawnLocs.Length)];
        
        GameObject pickup = Instantiate(pickupPrefab, spawnLoc.position, Quaternion.identity);
        
        pickups.Add(pickup);
    }
    
    private bool CanSpawn()
    {
        CleanUpPickups();
        
        return pickups.Count < maxPickups;
    }
    
    private void CleanUpPickups()
    {
        pickups.RemoveAll(p => p == null);
    }
}
