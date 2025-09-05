using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSourceMain;  // M�sica principal
    public AudioSource audioSourceBoss; // M�sica del jefe
    public AudioClip mainMusic;         // Clip de m�sica principal
    public AudioClip bossMusic;         // Clip de m�sica del jefe

    private bool bossMusicPlayed = false; // Controla si la m�sica del jefe ya se activ�

    public static MusicManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Configurar la m�sica principal y desactivar la del jefe al inicio
        audioSourceMain.clip = mainMusic;
        audioSourceMain.Play();

        audioSourceBoss.Stop(); // Asegurarse de que el AudioSource del jefe est� detenido
    }

    public void CheckScoreAndChangeMusic(int currentScore)
    {
        Debug.Log("Puntaje actual: " + currentScore); // Para seguimiento en consola

        // Cambiar a la m�sica del jefe si el puntaje es mayor o igual a 900
        if (currentScore >= 900 && !bossMusicPlayed)
        {
            ChangeToBossMusic();
        }
    }

    private void ChangeToBossMusic()
    {
        Debug.Log("Cambiando a la m�sica del jefe...");

        // Detener la m�sica principal
        if (audioSourceMain.isPlaying)
        {
            audioSourceMain.Stop();
        }

        // Reproducir la m�sica del jefe
        if (!audioSourceBoss.isPlaying)
        {
            audioSourceBoss.clip = bossMusic; // Asignar el clip de m�sica del jefe
            audioSourceBoss.Play();          // Reproducir la m�sica del jefe
        }

        bossMusicPlayed = true; // Marcar que la m�sica del jefe ya se activ�
    }
}
