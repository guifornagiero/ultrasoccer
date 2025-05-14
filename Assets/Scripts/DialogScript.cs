using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // necessário para Image
using UnityEngine.SceneManagement;


[System.Serializable]
public class DialogueLine
{
    public string text;
    public Sprite characterSprite;
}



public class DialogScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Image characterImage;
    public DialogueLine[] dialogueLines;
    public float textSpeed;
    private int index;
    public string faseAtual;
    public string preFase1 = "Pre Fase 1";
    public string preFase2 = "Pre Fase 2";
    public string preFase3 = "Pre Fase 3";
    public string fase1 = "Fase_1";
    public string fase2 = "Fase_2";
    public string fase3 = "Fase_3";

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
        faseAtual = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == dialogueLines[index].text)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = dialogueLines[index].text;
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        SetCharacterImage();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;

        foreach (char c in dialogueLines[index].text.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            SetCharacterImage();
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            if(faseAtual == preFase1)
            {
                SceneManager.LoadScene(fase1);
            }
            else if(faseAtual == preFase2)
            {
                SceneManager.LoadScene(fase2);
            }
            else if(faseAtual == preFase3)
            {
                SceneManager.LoadScene(fase3);
            }
        }
    }

    void SetCharacterImage()
    {
        if (characterImage != null)
        {
            characterImage.sprite = dialogueLines[index].characterSprite;
        }
    }
}
