using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

public class PowerupSpawner : MonoBehaviour
{
    public GameManager gameManager;
    public Vector2 topLeft = Vector2.zero;
    public Vector2 botRight = Vector2.zero;
    public List<PowerupSpawnSetting> spawnSettings = new List<PowerupSpawnSetting>();
    [Range(0f, 10f)]
    public double interval = 1;
    public bool spawn = true;

    private double cooldown;

    private void Start() {
    }

    void Update()
    {
        if (!gameManager.gameStarted) return;
        cooldown -= Time.deltaTime;
        if(cooldown < 0) {
            cooldown = interval;
            Spawn();
                
        }

    }


    void Spawn() {
        int chance = Random.Range(0, 100);

        GameObject powerupPrefab = null;
        for (int i = 0; i < spawnSettings.Count; i++) {
            if (spawnSettings[i].chance >=  chance) {
                powerupPrefab = spawnSettings[i].powerup;
                break;
            }
        }

        if(powerupPrefab != null) {
            Vector3 position = new Vector3(Random.Range(botRight.x, topLeft.x), 0, Random.Range(topLeft.y, botRight.y));

            var powerup = Instantiate(powerupPrefab, transform);
            powerup.transform.position = position + this.transform.position;
        }

    }

}


[Serializable]
public struct PowerupSpawnSetting
{
    public GameObject powerup;
    [Range(0,100)]
    public int chance;
}