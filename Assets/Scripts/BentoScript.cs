using System.Collections;
using UnityEngine;
using TMPro; // Para usar TextMeshPro

public class BentoScript : PlayerScript
{
    [Header("Ult Settings:")]
    public bool canDash = true;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float ultCooldownTime = 20f; 
    private float timeRemainingForUlt;

    private bool isDashing;
    private Vector2 lastMoveDirection = Vector2.right; // Dire��o inicial (para a direita)

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

        // Atualiza a dire��o do movimento
        if (xAxis != 0 || yAxis != 0)
        {
            lastMoveDirection = new Vector2(xAxis, yAxis).normalized;
        }

        // Atualiza a posi��o do contador para ficar acima do personagem
        if (ultCooldownText != null)
        {
            ultCooldownText.transform.position = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z); // Ajuste o valor `+ 1f` para a altura desejada
        }

        // Ativar o dash e reiniciar o cooldown quando pressionar R
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing && timeRemainingForUlt <= 0)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Movimenta o jogador na dire��o do �ltimo movimento
        float dashTimer = 0f;
        while (dashTimer < dashTime)
        {
            transform.position += (Vector3)(lastMoveDirection * dashSpeed * Time.deltaTime);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        // Reinicia o cooldown da ult (2 minutos) ap�s usar a habilidade
        timeRemainingForUlt = ultCooldownTime;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // Fun��o para formatar o tempo restante em minutos e segundos
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
