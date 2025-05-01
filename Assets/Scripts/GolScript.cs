using UnityEngine;
using System.Collections;


public class Gol : MonoBehaviour
{
    [SerializeField] private Transform bola;
    [SerializeField] private Transform meioCampo;
    [SerializeField] private Transform jogadorEsquerdo;
    [SerializeField] private Transform jogadorDireito;
    [SerializeField] private bool golEsquerdo;

    void Start()
    {
        bola.position = meioCampo.position;
        jogadorEsquerdo.position = new Vector2(-5f, 0f);
        jogadorDireito.position = new Vector2(5f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            if (golEsquerdo)
                Debug.Log("Gol da direita!");
            else
                Debug.Log("Gol da esquerda!");

            StartCoroutine(ResetarPosicoesComDelay());
        }
    }

    private IEnumerator ResetarPosicoesComDelay()
    {
        yield return new WaitForSeconds(0.5f);

        bola.position = meioCampo.position;
        jogadorEsquerdo.position = new Vector2(-5f, 0f);
        jogadorDireito.position = new Vector2(5f, 0f);

        Rigidbody2D rbBola = bola.GetComponent<Rigidbody2D>();
        rbBola.velocity = Vector2.zero;
        rbBola.angularVelocity = 0f;
    }
}
