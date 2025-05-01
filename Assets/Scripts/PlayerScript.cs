using UnityEngine;

public class PlayerScript : MonoBehaviour
{
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
        anim.SetBool("Walking", rb.velocity.x != 0 || rb.velocity.y != 0);
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
}
