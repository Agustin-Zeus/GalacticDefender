using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseBox;
    public GameObject ControlsBox;  

    public bool onPause;
    private GlobalMusicManager musicManager;

    [Header("Video Clip para el menú de pausa")]
    [SerializeField] private VideoPlayer pauseVideo;

    [Header("Video Clip para la pantalla de controles")]
    [SerializeField] private VideoPlayer controlsVideo;   

    private void Start()
    {
        musicManager = FindObjectOfType<GlobalMusicManager>();

        ControlsBox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ControlsBox.activeSelf)
            {
                CloseControls();
                return;
            }

            if (onPause)
                Continue();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        onPause = true;
        PauseBox.SetActive(true);
        ControlsBox.SetActive(false);

        Time.timeScale = 0f;
        musicManager.PauseMusic();

        if (pauseVideo != null)
        {
            pauseVideo.frame = 0;
            pauseVideo.Play();
        }
    }

    public void Continue()
    {
        onPause = false;
        PauseBox.SetActive(false);
        ControlsBox.SetActive(false);

        Time.timeScale = 1f;
        musicManager.ResumeMusic();

        if (pauseVideo != null)
            pauseVideo.Stop();

        if (controlsVideo != null)
            controlsVideo.Stop();
    }


    public void OpenControls()
    {
        PauseBox.SetActive(false);

        ControlsBox.SetActive(true);

        if (pauseVideo != null)
            pauseVideo.Pause();

        if (controlsVideo != null)
        {
            controlsVideo.frame = 0;
            controlsVideo.Play();
        }
    }

    public void CloseControls()
    {
        ControlsBox.SetActive(false);

        PauseBox.SetActive(true);

        if (controlsVideo != null)
            controlsVideo.Stop();

        if (pauseVideo != null)
            pauseVideo.Play();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
