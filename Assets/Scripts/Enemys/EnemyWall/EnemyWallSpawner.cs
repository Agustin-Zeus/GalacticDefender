using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallSpawner : MonoBehaviour
{
    public GameObject enemyWallPrefab;   // El prefab de EnemyWall
    public float spawnInterval = 1f;     // Intervalo de tiempo entre spawns

    public int maxEnemyWalls = 1;        // M�ximo n�mero de EnemyWalls permitidos
    private int currentEnemyWalls = 0;   // Contador de EnemyWalls activos

    public int minScoreThreshold = 150;  // Puntaje m�nimo para empezar a spawnear
    public int maxScoreThreshold = 700;  // Puntaje m�ximo para detener el spawn

    // flags para no spamear logs
    private bool _warnedNoGM, _warnedNoPrefab;

    public void Start()
    {
        if (!enemyWallPrefab)
        {
            if (!_warnedNoPrefab)
            {
                _warnedNoPrefab = true;
                Debug.LogError("[EnemyWallSpawner] Falta 'enemyWallPrefab'. Asignalo en el Inspector.", this);
            }
            // Evita programar spawns si falta el prefab
            return;
        }

        InvokeRepeating(nameof(SpawnEnemyWall), spawnInterval, spawnInterval);
    }

    private void Update()
    {
        var gm = GameManager.Instance; // puede ser null en algunos frames

        if (gm == null)
        {
            if (!_warnedNoGM)
            {
                _warnedNoGM = true;
                Debug.LogWarning("[EnemyWallSpawner] GameManager.Instance es null en Update(). Asegur� su creaci�n/persistencia (DontDestroyOnLoad) antes de usarlo.", this);
            }
            return;
        }

        // Si ya super� el umbral superior, deten� los spawns programados
        if (gm.TotalScore >= maxScoreThreshold)
        {
            CancelSpawn(); // cancela InvokeRepeating de este behaviour
        }
    }

    void SpawnEnemyWall()
    {
        var gm = GameManager.Instance;
        if (gm == null) return; // seguridad extra

        // Spawnea solo si el puntaje est� en rango y no excede el m�ximo activo
        if (gm.TotalScore >= minScoreThreshold && currentEnemyWalls < maxEnemyWalls)
        {
            // Posici�n de spawn del EnemyWall
            float spawnPosX = Random.Range(-5f, 5f); // float overload v�lido
            float spawnPosY = 2.5f;

            Vector2 spawnPosition = new Vector2(spawnPosX, spawnPosY);

            // Validaci�n por si el prefab se perdi� despu�s de Start
            if (!enemyWallPrefab)
            {
                if (!_warnedNoPrefab)
                {
                    _warnedNoPrefab = true;
                    Debug.LogError("[EnemyWallSpawner] 'enemyWallPrefab' es null al spawnear. Cancelando futuros spawns.", this);
                }
                CancelSpawn();
                return;
            }

            GameObject newEnemyWall = Instantiate(enemyWallPrefab, spawnPosition, Quaternion.identity);
            currentEnemyWalls++;  // Incrementar el contador de EnemyWalls activos

            // Si necesit�s el script:
            // EnemyWall enemyWallScript = newEnemyWall.GetComponent<EnemyWall>();
        }
    }

    public void EnemyWallDestroyed()
    {
        currentEnemyWalls = Mathf.Max(0, currentEnemyWalls - 1); // evita negativos
    }

    private void CancelSpawn()
    {
        CancelInvoke(nameof(SpawnEnemyWall)); // Documentado por Unity
    }

    private void OnDestroy()
    {
        // Evita que queden invokes activos apuntando a un objeto destruido
        CancelSpawn();
    }
}
