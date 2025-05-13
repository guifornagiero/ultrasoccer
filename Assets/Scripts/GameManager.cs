using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Timer")]
    public float tempoTotal = 180f; // 3 minutos
    private float tempoRestante;
    public TextMeshPro tempoTexto;

    [Header("Cenas")]
    public string faseAtual;
    public string fase2 = "Fase_2";
    public string fase3 = "Fase_3";
    public string gameOver = "GameOver";

    private bool jogoAcabou = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        tempoRestante = tempoTotal;
        AtualizarTempoUI();
        faseAtual = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (jogoAcabou) return;

        tempoRestante -= Time.deltaTime;
        AtualizarTempoUI();

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            VerificarResultado();
            jogoAcabou = true;
        }
    }

    void AtualizarTempoUI()
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        tempoTexto.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void VerificarResultado()
    {
        int esquerdo = ScoreManager.Instance.scoreEsquerdo;
        int direito = ScoreManager.Instance.scoreDireito;

        if (esquerdo > direito)
        {
            if (faseAtual == "Fase_1")
            {
                SceneManager.LoadScene(fase2); // Vai pra Fase_2
            }
            else if (faseAtual == "Fase_2")
            {
                SceneManager.LoadScene(fase3); // Vai pra Fase_3
            }
            else if (faseAtual == "Fase_3")
            {
                // Vitória final (você pode trocar por uma tela de parabéns se quiser)
                SceneManager.LoadScene("CharacterSelection");
            }
        }
        else
        {
            SceneManager.LoadScene(gameOver); // Derrota ou empate
        }
    }

}
