using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // necessário para Image


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

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
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
