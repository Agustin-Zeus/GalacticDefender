using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalidadBorroso : MonoBehaviour
{
    private GameObject filtroBorroso;

    void Start()
    {
        if (PlayerPrefs.GetInt("numeroDeCalidad", 2) == 0) // 0 = Low
        {
            InstanciarFiltro();
        }
    }

    void InstanciarFiltro()
    {
        // Verifica si ya existe un filtro en la escena
        if (GameObject.Find("FiltroBorroso") == null)
        {
            filtroBorroso = Instantiate(Resources.Load<GameObject>("FiltroBorroso"));
            filtroBorroso.name = "FiltroBorroso"; // Asegurar que siempre se llame igual
        }
    }

    void OnLevelWasLoaded(int level)
    {
        // Instancia el filtro en cada escena nueva
        if (PlayerPrefs.GetInt("numeroDeCalidad", 2) == 0)
        {
            InstanciarFiltro();
        }
    }
}
