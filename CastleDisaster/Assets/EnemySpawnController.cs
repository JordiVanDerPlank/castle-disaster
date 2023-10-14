using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    int amountToSpawn;
    public void PressSpawnEnemiesButton(int amount)
    {
        amountToSpawn = amount;
    }

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemySpawnpoint;
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Instantiate(enemyPrefab, enemySpawnpoint.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
    }
}
