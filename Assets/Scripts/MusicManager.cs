using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSourceMain;  // Música principal
    public AudioSource audioSourceBoss; // Música del jefe
    public AudioClip mainMusic;         // Clip de música principal
    public AudioClip bossMusic;         // Clip de música del jefe

    private bool bossMusicPlayed = false; // Controla si la música del jefe ya se activó

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
        // Configurar la música principal y desactivar la del jefe al inicio
        audioSourceMain.clip = mainMusic;
        audioSourceMain.Play();

        audioSourceBoss.Stop(); // Asegurarse de que el AudioSource del jefe esté detenido
    }

    public void CheckScoreAndChangeMusic(int currentScore)
    {
        Debug.Log("Puntaje actual: " + currentScore); // Para seguimiento en consola

        // Cambiar a la música del jefe si el puntaje es mayor o igual a 900
        if (currentScore >= 900 && !bossMusicPlayed)
        {
            ChangeToBossMusic();
        }
    }

    private void ChangeToBossMusic()
    {
        Debug.Log("Cambiando a la música del jefe...");

        // Detener la música principal
        if (audioSourceMain.isPlaying)
        {
            audioSourceMain.Stop();
        }

        // Reproducir la música del jefe
        if (!audioSourceBoss.isPlaying)
        {
            audioSourceBoss.clip = bossMusic; // Asignar el clip de música del jefe
            audioSourceBoss.Play();          // Reproducir la música del jefe
        }

        bossMusicPlayed = true; // Marcar que la música del jefe ya se activó
    }
}
