using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura entrada nos eixos X e Y
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // Normaliza pra não correr mais na diagonal
    }

    void FixedUpdate()
    {
        // Move o jogador com base na entrada
        rb.velocity = movement * moveSpeed;
    }
}
