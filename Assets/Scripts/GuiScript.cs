using System.Collections;
using UnityEngine;
using TMPro; // Para usar TextMeshPro

public class GuiScript : PlayerScript
{
    [SerializeField] private float ultKickMultiplier = 2f; // Multiplicador de força
    [SerializeField] private float ultDuration = 5f; // Duração da ULT
    private bool isUltActive = false;

    [Header("Ult Settings:")]
    [SerializeField] private float ultCooldownTime = 20f;
    private float timeRemainingForUlt;

    [Header("UI")]
    private TextMeshPro ultCooldownText; // Refer�ncia ao texto que vai mostrar o cooldown

    void Start()
    {
        timeRemainingForUlt = ultCooldownTime;
        StartCoroutine(SetupCooldownText());
    }
    private IEnumerator SetupCooldownText()
    {
        // Espera até que o objeto "Cooldown" exista na cena
        while (ultCooldownText == null)
        {
            GameObject textoGO = GameObject.Find("Cooldown");
            if (textoGO != null)
            {
                ultCooldownText = textoGO.GetComponent<TextMeshPro>();
            }
            yield return null; // Espera o próximo frame
        }

        // Quando encontrar, atualiza o texto inicial
        if (ultCooldownText != null)
        {
            ultCooldownText.text = FormatTime(timeRemainingForUlt);
        }
    }


    void Update()
    {
        // Atualiza o contador de cooldown da ult
        if (timeRemainingForUlt > 0)
        {
            timeRemainingForUlt -= Time.deltaTime;
            if (ultCooldownText != null)
            {
                ultCooldownText.text = FormatTime(timeRemainingForUlt);
            }
        }
        else
        {
            // Quando o cooldown acabar, exibe "ULT"
            if (ultCooldownText != null)
            {
                ultCooldownText.text = "ULT";
            }
        }

        // Atualiza a posi��o do contador para ficar acima do personagem
        if (ultCooldownText != null)
        {
            ultCooldownText.transform.position = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z); // Ajuste o valor `+ 1f` para a altura desejada
        }

        // Ativar o dash e reiniciar o cooldown quando pressionar R
        if (Input.GetKeyDown(KeyCode.LeftShift) && timeRemainingForUlt <= 0)
        {
            StartCoroutine(SuperKick());
        }
    }

    IEnumerator SuperKick()
    {
        // Ativa o modo ULT por alguns segundos
        isUltActive = true;
        PlayerScript playerScript = GetComponent<PlayerScript>();
        if (playerScript != null)
        {
            playerScript.SetKickMultiplier(ultKickMultiplier);
        }

        // Reinicia o cooldown da ult
        timeRemainingForUlt = ultCooldownTime;

        yield return new WaitForSeconds(ultDuration);

        // Desativa o modo ULT
        isUltActive = false;
        if (playerScript != null)
        {
            playerScript.SetKickMultiplier(1f); // volta ao normal
        }

    }

    // Fun��o para formatar o tempo restante em minutos e segundos
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
