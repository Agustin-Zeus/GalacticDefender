using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyThroughSpawner : MonoBehaviour
{
    public GameObject enemyFlyThroughPrefab;
    public float spawnInterval;
    private int activeEnemies = 0;

    public int minScoreThreshold; // Puntaje mínimo para empezar a spawnear
    public int maxScoreThreshold; // Puntaje máximo para detener el spawn

    private int lastSpawnSide = -1; // -1 significa que aún no se ha hecho spawn

    private void Start()
    {
        InvokeRepeating("SpawnEnemyFlyThrough", spawnInterval, spawnInterval);
    }

    private void SpawnEnemyFlyThrough()
    {
        if (GameManager.Instance.TotalScore >= minScoreThreshold &&
            GameManager.Instance.TotalScore < maxScoreThreshold &&
            activeEnemies < maxScoreThreshold)
        {
            Vector2 spawnPosition;
            Vector2 moveDirection;
            GetBalancedSpawnPositionAndDirection(out spawnPosition, out moveDirection);

            GameObject enemy = Instantiate(enemyFlyThroughPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<EnemyFlyThrough>().SetMoveDirection(moveDirection);
            activeEnemies++;
        }
    }

    private void GetBalancedSpawnPositionAndDirection(out Vector2 spawnPosition, out Vector2 moveDirection)
    {
        int spawnSide;

        // Asegura que el siguiente enemigo no aparezca en el mismo lado que el anterior
        do
        {
            spawnSide = Random.Range(0, 3); // 0 = top, 1 = left, 2 = right
        } while (spawnSide == lastSpawnSide);

        lastSpawnSide = spawnSide; // Actualizar el último lado de spawn

        // Determinar posición de spawn y dirección según el lado elegido
        switch (spawnSide)
        {
            case 0: // Top spawn
                spawnPosition = new Vector2(Random.Range(-7f, 7f), 6f);
                moveDirection = Vector2.down;
                break;

            case 1: // Left spawn
                spawnPosition = new Vector2(-9f, Random.Range(-2f, 2f));
                moveDirection = Vector2.right;
                break;

            case 2: // Right spawn
                spawnPosition = new Vector2(8f, Random.Range(-2f, 2f));
                moveDirection = Vector2.left;
                break;

            default:
                spawnPosition = new Vector2(0f, 6f);
                moveDirection = Vector2.down;
                break;
        }
    }

    public void EnemyDestroyed()
    {
        activeEnemies--;
    }
}
