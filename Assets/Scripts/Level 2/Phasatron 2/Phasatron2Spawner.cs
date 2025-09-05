using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phasatron2Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] public float spawnInterval = 1;
    public Transform[] moveSpots;  // A�ade los puntos de movimiento aqu�

    public int maxEnemies = 1;
    private int currentEnemy = 0;

    public int minScoreThreshold;  // Puntaje m�nimo para empezar a spawnear
    public int maxScoreThreshold;  // Puntaje m�ximo para detener el spawn

    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Solo spawnea si el puntaje est� entre el umbral m�nimo y m�ximo, y no se ha alcanzado el m�ximo de enemigos
        if (GameManager.Instance.TotalScore >= minScoreThreshold && GameManager.Instance.TotalScore < maxScoreThreshold && currentEnemy < maxEnemies)
        {
            float spawnPosX = Random.Range(-6, 3);  // Variaci�n en X
            float spawnPosY = 3.88f;  // Posici�n fija en Y

            Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY);

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Asignar los moveSpots al enemigo instanciado
            Phasatron2 enemyScript = newEnemy.GetComponent<Phasatron2>();
            enemyScript.moveSpots = moveSpots;

            currentEnemy++;
        }
    }

    public void EnemyDestroyed()
    {
        currentEnemy--;
    }
}


