using UnityEngine;
using System.Collections;

public class Gol : MonoBehaviour
{
    [SerializeField] private Transform bola;
    [SerializeField] private Transform meioCampo;
    [SerializeField] private bool golEsquerdo;

    private Transform jogadorEsquerdo;
    private Transform jogadorDireito;

    void Start()
    {
        bola.position = meioCampo.position;
        StartCoroutine(SetupJogadoresComDelay());
    }
    private IEnumerator SetupJogadoresComDelay()
    {
        yield return new WaitForSeconds(0.1f); // tempo para garantir que os jogadores foram instanciados

        GameObject objEsquerdo = GameObject.Find("JogadorEsquerdo");
        GameObject objDireito = GameObject.Find("JogadorDireito");

        if (objEsquerdo != null && objDireito != null)
        {
            jogadorEsquerdo = objEsquerdo.transform;
            jogadorDireito = objDireito.transform;

            jogadorEsquerdo.position = new Vector2(-5f, 0f);
            jogadorDireito.position = new Vector2(5f, 0f);
        }
        else
        {
            Debug.LogError("Jogadores não encontrados na cena!");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Ball"))
    {
        if (golEsquerdo)
        {
            ScoreManager.Instance.GolDireito(); // Gol na esquerda → ponto para o time direito
        }
        else
        {
            ScoreManager.Instance.GolEsquerdo(); // Gol na direita → ponto para o time esquerdo
        }

        StartCoroutine(ResetarPosicoesComDelay());
    }
}

    private IEnumerator ResetarPosicoesComDelay()
    {
        yield return new WaitForSeconds(0.5f);

        bola.position = meioCampo.position;

        if (jogadorEsquerdo != null && jogadorDireito != null)
        {
            jogadorEsquerdo.position = new Vector2(-5f, 0f);
            jogadorDireito.position = new Vector2(5f, 0f);
        }

        Rigidbody2D rbBola = bola.GetComponent<Rigidbody2D>();
        if (rbBola != null)
        {
            rbBola.velocity = Vector2.zero;
            rbBola.angularVelocity = 0f;
        }
    }
}
