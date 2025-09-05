using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage; // Imagen del tutorial (asígnala desde el Inspector)
    public float displayTime = 5f;  // Tiempo que se muestra la imagen (en segundos)

    private void Start()
    {
        // Verificar si estamos en la escena del Nivel 1 (Scene 3)
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            ShowTutorial();
        }
        else
        {
            // Si no es la escena 3, desactivar la imagen por si está activa
            if (tutorialImage != null)
                tutorialImage.SetActive(false);
        }
    }

    private void ShowTutorial()
    {
        if (tutorialImage != null)
        {
            tutorialImage.SetActive(true); // Mostrar la imagen del tutorial

            // Pausar el tiempo del juego
            Time.timeScale = 0f;

            // Iniciar una corrutina para manejar el tiempo de espera
            StartCoroutine(HideTutorialAfterDelay());
        }
        else
        {
            Debug.LogWarning("No se ha asignado la imagen del tutorial en el Inspector.");
        }
    }

    private IEnumerator HideTutorialAfterDelay()
    {
        // Esperar el tiempo especificado mientras la escala de tiempo está pausada
        yield return new WaitForSecondsRealtime(displayTime);

        HideTutorial();
    }

    private void HideTutorial()
    {
        if (tutorialImage != null)
        {
            tutorialImage.SetActive(false); // Ocultar la imagen del tutorial
        }

        // Reanudar el tiempo del juego
        Time.timeScale = 1f;
    }
}
