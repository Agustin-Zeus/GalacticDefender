using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoritoSpawnerL : MonoBehaviour
{
    public GameObject meteoritoPrefab; // Prefab del meteorito
    public float spawnInterval = 15f; // Intervalo de aparici�n en segundos
    public float meteoritoSpeed = 5f; // Velocidad del meteorito
    public Vector2 spawnPosition = new Vector2(0f, 6f); // Posici�n fija de spawn

    public GameObject warningSprite; // Sprite de advertencia
    public float warningDuration = 3f; // Tiempo que el sprite de advertencia estar� activo

    private void Awake()
    {
        if (warningSprite != null)
        {
            warningSprite.SetActive(false); // Aseg�rate de que el sprite est� inicialmente desactivado
        }
    }

    private void OnEnable()
    {
        // Inicia el spawn solo cuando el spawner est� activado
        InvokeRepeating("StartWarningSequence", spawnInterval, spawnInterval);
    }

    private void OnDisable()
    {
        // Detiene el spawn cuando el spawner est� desactivado
        CancelInvoke("StartWarningSequence");
    }

    private void StartWarningSequence()
    {
        if (warningSprite != null)
        {
            StartCoroutine(WarningSequence());
        }
    }

    private IEnumerator WarningSequence()
    {
        // Mostrar el sprite de advertencia
        warningSprite.SetActive(true);
        yield return new WaitForSeconds(warningDuration);

        // Ocultar el sprite y spawnear el meteorito
        warningSprite.SetActive(false);
        SpawnMeteorito();
    }

    private void SpawnMeteorito()
    {
        // Instanciar el meteorito en la posici�n fija
        GameObject meteorito = Instantiate(meteoritoPrefab, spawnPosition, Quaternion.identity);

        // Darle una velocidad en diagonal hacia abajo
        Rigidbody2D rb = meteorito.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(2f, -meteoritoSpeed); // Ajusta la direcci�n de la diagonal aqu� si lo deseas
        }
    }
}
