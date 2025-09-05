using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private float delayTime = 1f; // Tiempo de espera antes de cambiar de escena

    // M�todos principales del men�
    public void StartGame()
    {

        Invoke(nameof(LoadScene2), delayTime); // Comienza desde el nivel 1
    }

    public void LevelSelection()
    {
        Invoke(nameof(LoadScene1), delayTime); // Carga la escena de selecci�n de nivel
    }

    public void Level1()
    {

        Invoke(nameof(LoadScene2), delayTime); // Escena asignada al Nivel 1
    }

    public void Level2()
    {

        Invoke(nameof(LoadScene3), delayTime); // Escena asignada al Nivel 2
    }

    public void Level3()
    {

        Invoke(nameof(LoadScene6), delayTime); // Escena asignada al Nivel 3
    }

    public void BackMenu()
    {
        GameManager.Instance.CheckHighScore(); // Guardar el high score antes de resetear
                                               //GameManager.Instance.ResetTotalScore();

        Invoke(nameof(LoadScene0), delayTime); // Regresa al men� principal
    }

    public void Exit()
    {
        Invoke(nameof(QuitGame), delayTime); // Sale del juego
    }

    // M�todos para cargar escenas espec�ficas
    private void LoadScene0()
    {
        SceneManager.LoadScene(0); // Men� principal
    }

    private void LoadScene1()
    {
        SceneManager.LoadScene(1); // Selecci�n de nivel
    }

    private void LoadScene2()
    {
        SceneManager.LoadScene(2); // Nivel 1
    }

    private void LoadScene3()
    {
        SceneManager.LoadScene(3); // Nivel 2
    }

    private void LoadScene6()
    {
        SceneManager.LoadScene(6); // Nivel 3
    }

    private void QuitGame()
    {
        Application.Quit(); // Cierra la aplicaci�n
    }

    // M�todo auxiliar para reiniciar el estado del GameManager
    private void ResetGameManager()
    {
        GameManager.Instance.ResetTotalScore();

    }

}
