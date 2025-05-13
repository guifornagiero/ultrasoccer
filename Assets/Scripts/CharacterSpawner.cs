using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public CharacterDatabase characterDB;

    void Start()
    {
        int index = PlayerPrefs.GetInt("selectedOption", 0);
        Character character = characterDB.GetCharacter(index);
        
        if (character != null && character.prefabPersonagem != null)
        {
            GameObject jogadorEsquerdo = Instantiate(character.prefabPersonagem, transform.position, Quaternion.identity);
            jogadorEsquerdo.name = "JogadorEsquerdo";
    }
        else
    {
            Debug.LogError("Personagem ou prefab não definido!");
        }
    }
}