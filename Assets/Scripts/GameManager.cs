using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.Collections;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.Audio;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public HUD hud; // Referencia al HUD para acceder al slider de vidas
    public int TotalScore { get { return totalScore; } }

    private float lives = 3;
    private bool playerInvulnerable = false;
    private float lastDamageTime = -2f;

    public int meteorScoreThreshold;
    public int bossScoreThreshold;
    public int stormScoreThreshold; // Umbral de puntos para activar la tormenta
    private int totalScore = 0;

    private bool bossInvoked = false;
    private bool stormActivated = false; // Controla si la tormenta ya se activó
    private bool healthBarBoss = false;

    public GameObject boss;
    public GameObject lifeBarBoss;

    private bool isImmortal = false;

    public MeteoritoSpawner[] meteoritoSpawners; // Referencia al MeteoritoSpawner
    public MeteoritoSpawnerL[] meteoritoSpawnersL;
    public StormManager stormManager; // Referencia al StormManager

    public float meteoritoRandomizeInterval = 5f; // Intervalo para randomizar spawners

    public GameObject player; // Referencia al GameObject del jugador

    [SerializeField] private VideoPlayer eventVideoPlayer; // VideoPlayer que reproducirá el video del evento


    public TMPro.TextMeshPro ScoreText;
    public TMPro.TextMeshPro HighScore;
    public TMPro.TextMeshPro FinalScoreText;


    public int highScoreLevel1 = 0; // High score para el nivel 1
    public int highScoreLevel2 = 0; // High score para el nivel 2

    public int scoreLevel1;
    public int scoreLevel2;


    // Sonido de muerte
    public AudioClip deathSound;
    private AudioSource audioSource;

    private bool bossMusicPlayed = false; // Controla si la música ya cambió


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;


        }
        else
        {

            return;
        }
        LoadHighScores(); // Cargar los high scores
    }

    public void Start()
    {
        boss.SetActive(false);
        InvokeRepeating(nameof(RandomizeMeteoritoSpawners), meteoritoRandomizeInterval, meteoritoRandomizeInterval);


        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleImmortality();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateCheat();
        }
    }


    public void SetPlayerInvulnerable(bool state)
    {
        playerInvulnerable = state;
    }

    public void AddScore(int ScoreToAdd)
    {
        totalScore += ScoreToAdd;
        hud.UpdateScore(totalScore);


        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();
        // Guardar High Score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (totalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", totalScore);
            PlayerPrefs.Save();
        }
        // Activa la tormenta cuando se alcanza el puntaje necesario, si no se ha activado aún
        if (!stormActivated && totalScore >= stormScoreThreshold)
        {
            ActivateStorm();
        }

        if (!bossInvoked && totalScore >= bossScoreThreshold)
        {
            InvokeBoss();
        }
    }

    private void ActivateStorm()
    {
        if (stormManager != null)
        {
            stormActivated = true; // Marcar que la tormenta ya fue activada
            stormManager.StartStorm(); // Inicia el evento de tormenta
        }
    }

    public void LoseLive(float damage = 1)
    {
        if (isImmortal)
        {
            return;
        }

        if (Time.time - lastDamageTime < 2f)
        {
            return;
        }

        lastDamageTime = Time.time;

        if (playerInvulnerable)
        {
            return;
        }

        lives -= damage;
        hud.UpdateLivesSlider(lives);

        // Llamar a las corutinas para el efecto visual
        if (player != null)
        {
            StartCoroutine(FlashRedAndInvulnerability());
        }

        if (lives <= 0)
        {
            PlayDeathSound();

            int currentScene = SceneManager.GetActiveScene().buildIndex; // Obtiene el índice de la escena actual

            if (currentScene == 2)
            {
                CheckHighScore();
                StartCoroutine(WaitAndLoadScene(4));
            }
            else if (currentScene == 3)
            {
                CheckHighScore();
                StartCoroutine(WaitAndLoadScene(7));
            }
            else if (currentScene == 6)
            {
                CheckHighScore();
                StartCoroutine(WaitAndLoadScene(9));
            }
        }
    }

    // Combinar FlashRed con la activación de la opacidad de invulnerabilidad
    public IEnumerator FlashRedAndInvulnerability()
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.55f);

            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.1f);
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.6f);
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.1f);
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.6f);
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.1f);


            yield return new WaitForSeconds(0.8f);

            // Restaurar el color y la opacidad original
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        }

    }


    private IEnumerator WaitAndLoadScene(int sceneIndex)
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        SceneManager.LoadScene(sceneIndex); // Carga la escena
    }


    public bool RecoverLive()
    {
        if (lives < hud.GetMaxLives())
        {
            lives = Mathf.Min(lives + 1, hud.GetMaxLives());
            hud.UpdateLivesSlider(lives);
            return true;
        }

        return false;
    }

    private void ToggleImmortality()
    {
        isImmortal = !isImmortal;
        hud.SetLivesSliderVisibility(!isImmortal);
    }

    private void ActivateCheat()
    {
        totalScore += 900;
        hud.UpdateScore(totalScore);

        if (!bossInvoked)
        {
            InvokeBoss();
        }
    }

    public bool IsPlayerImmortal()
    {
        return isImmortal;
    }

    public void SaveScore(int level, int score)
    {
        if (level == 1)
        {
            scoreLevel1 = score;
            PlayerPrefs.SetInt("ScoreLevel1", scoreLevel1);
        }
        else if (level == 2)
        {
            scoreLevel2 = score;
            PlayerPrefs.SetInt("ScoreLevel2", scoreLevel2);
        }

        PlayerPrefs.Save();
    }
    public void CheckHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0); // Obtiene el high score guardado
        if (totalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", totalScore); // Guarda el nuevo high score
            PlayerPrefs.Save();
        }
    }

    public void SaveHighScore(int level, int score)
    {
        if (level == 1 && score > highScoreLevel1)
        {
            highScoreLevel1 = score;
            PlayerPrefs.SetInt("HighScoreLevel1", highScoreLevel1);
        }
        else if (level == 2 && score > highScoreLevel2)
        {
            highScoreLevel2 = score;
            PlayerPrefs.SetInt("HighScoreLevel2", highScoreLevel2);
        }
    }

    private void LoadHighScores()
    {
        highScoreLevel1 = PlayerPrefs.GetInt("HighScoreLevel1", 0);
        highScoreLevel2 = PlayerPrefs.GetInt("HighScoreLevel2", 0);
    }

    public void EndLevel(int level)
    {
        int score = totalScore; // Usa el totalScore acumulado en la partida

        SaveScore(level, score);       // Guarda el score del nivel
        SaveHighScore(level, score);   // Guarda el highscore del nivel
    }

    private void RandomizeMeteoritoSpawners()
    {
        if (bossInvoked) return; // Si el jefe está invocado, no se randomizan los spawners

        // Activar/desactivar aleatoriamente los spawners de tipo común
        foreach (var spawner in meteoritoSpawners)
        {
            if (spawner != null)
            {
                spawner.gameObject.SetActive(Random.value > 0.5f); // Activa/desactiva aleatoriamente
            }
        }

        // Activar/desactivar aleatoriamente los spawners de tipo L
        foreach (var spawnerL in meteoritoSpawnersL)
        {
            if (spawnerL != null)
            {
                spawnerL.gameObject.SetActive(Random.value > 0.5f); // Activa/desactiva aleatoriamente
            }
        }
    }

    public void InvokeBoss()
    {
        bossInvoked = true;
        healthBarBoss = true;

        // Inicia la coroutine para mostrar el evento y spawnear el boss
        StartCoroutine(InvokeBossWithDelay());
    }

    private IEnumerator InvokeBossWithDelay()
    {

        yield return new WaitForSeconds(1f);
        // Reproducir el video del evento
        if (eventVideoPlayer != null)
        {
            eventVideoPlayer.gameObject.SetActive(true); // Activar el VideoPlayer
            eventVideoPlayer.Play(); // Reproducir el video

            // Esperar 5 segundos para que termine el video
            yield return new WaitForSeconds(5f);

            eventVideoPlayer.Stop(); // Detener el video
            eventVideoPlayer.gameObject.SetActive(false); // Desactivar el VideoPlayer
        }

        // Esperar los 0.3 segundos adicionales
        yield return new WaitForSeconds(0.3f);

        // Spawn del jefe
        if (boss != null)
        {
            boss.transform.position = new Vector3(0, 2.3f, 0);
            boss.SetActive(true);
        }

        if (lifeBarBoss != null)
        {
            lifeBarBoss.SetActive(true);
        }

        // Detener meteoritos
        CancelInvoke(nameof(RandomizeMeteoritoSpawners));

        foreach (var spawner in meteoritoSpawners)
        {
            if (spawner != null)
            {
                spawner.gameObject.SetActive(false);
            }
        }

        foreach (var spawnerL in meteoritoSpawnersL)
        {
            if (spawnerL != null)
            {
                spawnerL.gameObject.SetActive(false);
            }
        }
    }

    public void ResetTotalScore()
    {
        totalScore = 0;
        hud.UpdateScore(totalScore); // Asegurar que el HUD refleje el cambio
    }





    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }


    public bool IsPlayerInvulnerable()
    {
        return playerInvulnerable;
    }
}
