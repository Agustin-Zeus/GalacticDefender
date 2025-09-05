using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSwitcher : MonoBehaviour
{
    public GameObject[] ships;                // Lista de las naves para alternar
    private ProjectileShoot activeProjectileShoot;   // Referencia al disparo de la nave activa

    private int currentShipIndex = 0;
    private float switchCooldown = 3f;        // Tiempo de espera entre cambios
    private float lastSwitchTime = -3f;       // Tiempo del último cambio, inicializado para permitir cambio al inicio

    void Start()
    {
        if (ships.Length > 0)
        {
            SwitchToShip(currentShipIndex);
        }
        else
        {
            Debug.LogError("Error: No hay naves asignadas en el array de ships.");
        }
    }

    void Update()
    {
        // Verificar si se presiona "C" y se cumple el cooldown y que el triple disparo no esté activo en la nave seleccionada
        if (Input.GetKeyDown(KeyCode.C) && CanSwitchShip())
        {
            currentShipIndex = (currentShipIndex + 1) % ships.Length;
            Debug.Log("Cambiando a la nave de índice: " + currentShipIndex);  // Depuración
            SwitchToShip(currentShipIndex);
            lastSwitchTime = Time.time; // Actualizar el último tiempo de cambio
        }

        UpdateInactiveShipsPosition();
    }

    private bool CanSwitchShip()
    {
        // Asegurarse de que activeProjectileShoot esté actualizado
        return (Time.time - lastSwitchTime >= switchCooldown) && !activeProjectileShoot.IsTripleShotActive;
    }

    private void SwitchToShip(int index)
    {
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].SetActive(i == index);

            if (i == index)
            {
                // Actualizar referencia a ProjectileShoot de la nave activa
                activeProjectileShoot = ships[i].GetComponent<ProjectileShoot>();
                Debug.Log("Nave activa: " + i + " con triple disparo activo: " + activeProjectileShoot.IsTripleShotActive);
            }
        }
    }

    private void UpdateInactiveShipsPosition()
    {
        Vector3 activeShipPosition = ships[currentShipIndex].transform.position;
        Quaternion activeShipRotation = ships[currentShipIndex].transform.rotation;

        for (int i = 0; i < ships.Length; i++)
        {
            if (i != currentShipIndex)
            {
                ships[i].transform.position = activeShipPosition;
                ships[i].transform.rotation = activeShipRotation;
            }
        }
    }
}
