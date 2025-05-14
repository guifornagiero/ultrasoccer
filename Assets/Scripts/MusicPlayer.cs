using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    [SerializeField] private AudioClip musicaPrincipal; // Música para o jogo
    [SerializeField] private AudioClip musicaVitoria;  // Música de vitória
    [SerializeField] private AudioClip musicaDerrota;   // Música de derrota

    private AudioSource audioSource;

    [SerializeField] private string[] cenasSemMusica = { "Victory Scene", "GameOver" };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource.loop = true; // Para garantir que a música toque em loop
        audioSource.Play();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Se a cena for de vitória ou derrota, muda a música
        if (scene.name == "Victory Scene")
        {
            if (audioSource.clip != musicaVitoria)
            {
                audioSource.clip = musicaVitoria;
                audioSource.Play();
            }
        }
        else if (scene.name == "GameOver")
        {
            if (audioSource.clip != musicaDerrota)
            {
                audioSource.clip = musicaDerrota;
                audioSource.Play();
            }
        }
        else
        {
            // Música principal durante o jogo
            if (audioSource.clip != musicaPrincipal)
            {
                audioSource.clip = musicaPrincipal;
                audioSource.Play();
            }
        }
    }
}
