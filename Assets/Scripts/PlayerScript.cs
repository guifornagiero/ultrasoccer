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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if(!PlayerPrefs.HasKey("selectedOption")){
            selectedOption = 0;
        }
        else{
            Load();
        }
        UpdateCharacter(selectedOption);
    }

    void Update()
    {
        GetInputs();
        Move();
        Flip();
        Kick();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        rb.velocity = new Vector2(walkSpeed * xAxis, walkSpeed * yAxis);
        //verifica se o player esta andando
        anim.SetBool("Walking", xAxis != 0);

        // verifica se o player esta andando para cima ou para baixo
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
    private void UpdateCharacter(int selectedOption){
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
    }

    private void Load(){
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}
