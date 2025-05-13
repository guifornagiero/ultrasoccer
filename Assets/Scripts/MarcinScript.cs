using System.Collections;
using UnityEngine;
using TMPro;

public class MarcinScript : PlayerScript
{
    [Header("Ult Settings:")]
    [SerializeField] private float ultSpeedMultiplier = 1.5f; // Multiplicador de velocidade
    [SerializeField] private float ultDuration = 5f;
    private bool isUltActive = false;

    [SerializeField] private float ultCooldownTime = 20f;
    private float timeRemainingForUlt;

    [Header("UI")]
    private TextMeshPro ultCooldownText;

    void Start()
    {
        timeRemainingForUlt = ultCooldownTime;
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
            StartCoroutine(SpeedBoostUlt());
        }
    }

    IEnumerator SpeedBoostUlt()
    {
        isUltActive = true;
        PlayerScript playerScript = GetComponent<PlayerScript>();
        if (playerScript != null)
        {
            playerScript.SetSpeedMultiplier(ultSpeedMultiplier);
        }

        timeRemainingForUlt = ultCooldownTime;

        yield return new WaitForSeconds(ultDuration);

        isUltActive = false;
        if (playerScript != null)
        {
            playerScript.SetSpeedMultiplier(1f);
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
