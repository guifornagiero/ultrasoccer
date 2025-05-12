using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    private int selectedOption = 0;
    [Header("Horizontal Movement Settings: ")]
    [SerializeField] private float walkSpeed = 1;
    protected float xAxis, yAxis;
    protected Rigidbody2D rb;
    private Animator anim;

    [Header("Kick Settings: ")]
    [SerializeField] private float forcaChute = 8f;
    [SerializeField] private float alcanceChute = 3f;
    [SerializeField] private LayerMask ballLayer;

    [Header("Campo Limits")]
    private float minX = -8.5f, maxX = 9f; // Limites horizontais (x)
    private float minY = -3f, maxY = 1.8f; // Limites verticais (y)

    void Start()
    {
        // Inicializa o Rigidbody e Animator
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Verifica se a opção foi salva no PlayerPrefs, caso contrário, usa a opção padrão
        if(!PlayerPrefs.HasKey("selectedOption")){
            selectedOption = 0;
        }
        else{
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

    void Update()
    {
        GetInputs();
        Move();
        Flip();
        Kick();

        // Limita a posição do jogador no eixo X
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);

        // Limita a posição do jogador no eixo Y
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        // Aplica a posição corrigida ao jogador
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        rb.velocity = new Vector2(walkSpeed * xAxis, walkSpeed * yAxis);
        // Verifica se o player está andando
        anim.SetBool("Walking", xAxis != 0);
        anim.SetBool("walking_up", yAxis > 0);
        anim.SetBool("walking_down", yAxis < 0);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D bola = Physics2D.OverlapCircle(transform.position, alcanceChute, ballLayer);
            if (bola != null && bola.CompareTag("Ball"))
            {
                Vector2 direcao = (bola.transform.position - transform.position).normalized;
                bola.GetComponent<Rigidbody2D>().AddForce(direcao * forcaChute, ForceMode2D.Impulse);
            }
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

    public void SetKickMultiplier(float multiplier)
    {
        forcaChute = 8f * multiplier; // 8f é o valor base
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        walkSpeed = 4.3f * multiplier; // 1f é o valor base (ajuste se o valor base for outro)
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}
