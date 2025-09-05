using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace Shmup
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] List<EnemyType> enemyTypes;
        [SerializeField] int maxEnemies;
        [SerializeField] float spawnInterval;

        List<SplineContainer> splines;
        EnemyFactory enemyFactory;

        float spawnTimer;
        int enemiesSpawner;

        void OnValidate()
        {
            splines = new List<SplineContainer>(collection:GetComponentsInChildren<SplineContainer>());
        }

        void Start() => enemyFactory = new EnemyFactory();

        private void Update()
        {
            spawnTimer += Time.deltaTime;

            if (enemiesSpawner < maxEnemies && spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f; 
            }
        }

        void SpawnEnemy()
        {
            EnemyType enemyType = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)];
            SplineContainer spline = splines[UnityEngine.Random.Range(0, splines.Count)];

            enemyFactory.CreateEnemy(enemyType, spline);

            enemiesSpawner++;
        }
    }
}
