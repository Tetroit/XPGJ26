using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class JunkSpawner : MonoBehaviour
{
    public JunkManager junkManager;
    public WaveGen waveGen;
    public JunkPool junkPool;
    public float minSpawnX;
    public float maxSpawnX;
    public float spawnZ = 50;

    public float minSpeed = 4;
    public float maxSpeed = 6;
    
    public float minSpawnT = 5;
    public float maxSpawnT = 10;

    public void StartGame()
    {
        StartCoroutine(SpawnCoroutine());
    }
    public void StopGame()
    {
        StopCoroutine(SpawnCoroutine());
    }
    public void VelocityChange(float velocity)
    {
        minSpeed = velocity * 0.8f;
        maxSpeed = velocity * 1.2f;
        minSpawnT = 25f / velocity;
        maxSpawnT = 50f / velocity;
    }
    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(minSpawnT, maxSpawnT));
        }
    }
    private void Spawn()
    {
        var prefab = junkPool.Pull();
        if (prefab == null)
            return;
        float spawnX = Random.Range(minSpawnX, maxSpawnX);
        float speed = Random.Range(minSpeed, maxSpeed);
        var inst = Instantiate(prefab);
        inst.junkManager = junkManager;
        junkManager.junks.Add(inst);
        inst.transform.position = new Vector3(spawnX, 0, spawnZ);
        inst.speed = new Vector3(0, 0, -speed);
        inst.waveGen = waveGen;
    }
}
