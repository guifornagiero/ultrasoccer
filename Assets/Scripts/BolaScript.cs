using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField]  public float forcaEmpurrao = 0.3f;
    [SerializeField] public float MaxVelocity = 1f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.drag = 2f;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            Vector2 direcao = (transform.position - coll.transform.position).normalized;
            rb2d.AddForce(direcao * forcaEmpurrao, ForceMode2D.Impulse);

            Vector2 vel;
            vel.x = rb2d.velocity.x;
            vel.y = (rb2d.velocity.y / 2) + (coll.collider.attachedRigidbody.velocity.y / 3);
            rb2d.velocity = Vector2.Lerp(rb2d.velocity, vel, 0.5f);

            if (rb2d.velocity.magnitude > MaxVelocity)
            {
                rb2d.velocity = rb2d.velocity.normalized * MaxVelocity;
            }
        }
    }
}
