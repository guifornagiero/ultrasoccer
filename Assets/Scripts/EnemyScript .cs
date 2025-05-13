using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyScript : MonoBehaviour
{
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    private int selectedOption = 0;
    [Header("Horizontal Movement Settings: ")]
    [SerializeField] private float walkSpeed = 1;
    protected float xAxis, yAxis;
    protected Rigidbody2D rb;
    private Animator anim;
    private Vector2 ballPos;
    private bool gameDelay = true;
    private bool kickDelay = false;
    private Vector2 GoalPosition;

    [Header("Kick Settings: ")]
    [SerializeField] private float forcaChute = 8f;
    [SerializeField] private float alcanceChute = 3f;
    [SerializeField] private LayerMask ballLayer;

    void Start()
    {
        StartCoroutine(WaitGameLoad(0.1f));
        // Inicializa o Rigidbody e Animator
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GoalPosition = GameObject.FindGameObjectWithTag("LeftGoal").transform.position;

        // Verifica se a opção foi salva no PlayerPrefs, caso contrário, usa a opção padrão
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }

        // Verifica se o banco de dados de personagens está atribuído
        if (characterDB != null && characterDB.CharacterCount > 0)
        {
            UpdateCharacter(selectedOption);
        }
        else
        {

            Debug.LogError("CharacterDatabase não está atribuído ou está vazio!");
        }
    }

    IEnumerator WaitGameLoad(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        gameDelay = false;
    }

    IEnumerator KickDelay(float tempo)
    {
        Debug.Log("Chute em espera");
        yield return new WaitForSeconds(tempo);
        Debug.Log("Chute carregado");
        kickDelay = false;
    }

    void Update()
    {
        GetBallPosition();
        Move();
        Flip();
        Kick();
    }

    void GetBallPosition()
    {
        if (gameDelay == false)
        {
            ballPos = GameObject.FindGameObjectWithTag("Ball").transform.position;
        }
    }


    void Move()
    {

        var pos = transform.position;
        if (ballPos.x+1 > transform.position.x)
        {
            Debug.Log("Bola a frente do inimigo");
            var ballPredict = ballPos;
            ballPredict.x += 0.3f;
            if (transform.position.y > ballPos.y)
            {
                ballPredict.y += 0.4f;
            }
            else
            {
                ballPredict.y -= 0.2f;
            }
            pos = Vector3.MoveTowards(transform.position, ballPredict, walkSpeed * Time.deltaTime);
            // Verifica se o player está andando
            anim.SetBool("Walking", xAxis != 0);
            anim.SetBool("walking_up", yAxis > 0);
            anim.SetBool("walking_down", yAxis < 0);
            
        }
        if (ballPos.x <= transform.position.x)
        {
            Debug.Log("Bola a atras do inimigo");
            var ballPredict = ballPos;
            if (transform.position.y > ballPos.y)
            {
                ballPredict.y += 0.6f;
            }
            else
            {
                ballPredict.y -= 0.2f;
            }
            pos = Vector3.MoveTowards(transform.position, ballPos, walkSpeed * Time.deltaTime);
        }


        transform.position = pos;

    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    void Kick()
    {
        Collider2D bola = Physics2D.OverlapCircle(transform.position, alcanceChute, ballLayer);
        if (bola != null && bola.CompareTag("Ball") && kickDelay == false && ballPos.x <= transform.position.x)
        {
            Debug.Log("Chute");
            Vector2 direcao = (GoalPosition - ballPos).normalized;
            bola.GetComponent<Rigidbody2D>().AddForce(direcao * forcaChute, ForceMode2D.Impulse);
            kickDelay = true;
            StartCoroutine(KickDelay(0.2f));
        }

    }

    private void UpdateCharacter(int selectedOption)
    {
        // Adiciona verificação para garantir que o índice seja válido
        if (selectedOption >= 0 && selectedOption < characterDB.CharacterCount)
        {
            Character character = characterDB.GetCharacter(selectedOption);
            if (character != null && character.characterSprite != null)
            {
                artworkSprite.sprite = character.characterSprite;
            }
            else
            {
                Debug.LogError("O personagem ou o sprite do personagem está ausente!");
            }
        }
        else
        {
            Debug.LogError("Índice de personagem inválido: " + selectedOption);
        }
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}
