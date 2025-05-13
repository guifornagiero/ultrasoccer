using System.Collections;
using UnityEngine;
using TMPro;

public class ViniScript : PlayerScript
{
    [Header("Ult Settings:")]
    [SerializeField] private float attractionForce = 100f;
    [SerializeField] private float ultDuration = 5f;
    private GameObject ball;
    private bool isUltActive = false;

    [SerializeField] private float ultCooldownTime = 20f;
    private float timeRemainingForUlt;

    [Header("UI")]
    private TextMeshPro ultCooldownText;

    void Start()
    {
        timeRemainingForUlt = ultCooldownTime;
        ball = GameObject.FindWithTag("Ball"); // Acha a bola pelo tag
        StartCoroutine(SetupCooldownText());
    }

    private IEnumerator SetupCooldownText()
    {
        while (ultCooldownText == null)
        {
            GameObject textoGO = GameObject.Find("Cooldown");
            if (textoGO != null)
            {
                ultCooldownText = textoGO.GetComponent<TextMeshPro>();
            }
            yield return null;
        }

        if (ultCooldownText != null)
        {
            ultCooldownText.text = FormatTime(timeRemainingForUlt);
        }
    }

    void Update()
    {
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
            if (ultCooldownText != null)
            {
                ultCooldownText.text = "ULT";
            }
        }

        if (ultCooldownText != null)
        {
            ultCooldownText.transform.position = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && timeRemainingForUlt <= 0)
        {
            StartCoroutine(MagneticUlt());
        }
    }

    IEnumerator MagneticUlt()
    {
        isUltActive = true;
        timeRemainingForUlt = ultCooldownTime;

        float timer = 0f;
        while (timer < ultDuration)
        {
            if (ball != null)
            {
                // Move a bola na direção do jogador
                ball.transform.position = Vector3.MoveTowards(
                    ball.transform.position,
                    transform.position,
                    10f * Time.deltaTime // velocidade da atração
                );
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isUltActive = false;
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}