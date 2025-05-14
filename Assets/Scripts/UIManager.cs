using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void ReiniciarJogo()
    {
        SceneManager.LoadScene("CharacterSelection"); 
    }
}