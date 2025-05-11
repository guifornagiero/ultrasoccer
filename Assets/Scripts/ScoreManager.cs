using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int scoreEsquerdo = 0;
    public int scoreDireito = 0;

    public TextMeshProUGUI textoPlacarEsquerdo;
    public TextMeshProUGUI textoPlacarDireito;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GolEsquerdo()
    {
        scoreEsquerdo++;
        AtualizarPlacar();
    }

    public void GolDireito()
    {
        scoreDireito++;
        AtualizarPlacar();
    }

    private void AtualizarPlacar()
    {
        if (textoPlacarEsquerdo != null)
            textoPlacarEsquerdo.text = scoreEsquerdo.ToString();

        if (textoPlacarDireito != null)
            textoPlacarDireito.text = scoreDireito.ToString();
    }
}
