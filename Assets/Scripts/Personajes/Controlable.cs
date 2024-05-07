using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controlable : MonoBehaviour
{
    public float velocidad = 50f;
    public float fuerzaSalto = 100f; // Fuerza del salto
    private bool enSuelo;
    private Rigidbody2D rb;
    private Transform spriteTransform;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = GetComponentInChildren<Image>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener la entrada del teclado
        // Obtener la entrada del teclado
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // Calcular el movimiento horizontal
        Vector2 movimiento = new Vector2(movimientoHorizontal * velocidad, rb.velocity.y);

        // Aplicar el movimiento al Rigidbody2D
        rb.velocity = movimiento;
        if (movimientoHorizontal > 0)
        {
            spriteTransform.localScale = new Vector3(-1, 1, 1); // Escala normal
        }
        else if (movimientoHorizontal < 0)
        {
            spriteTransform.localScale = new Vector3(1, 1, 1); // Escala invertida horizontalmente
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && enSuelo)
        {
            Saltar();
        }
    }

    void Saltar()
    {
        // Aplicar una fuerza vertical al Rigidbody2D para simular el salto
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);

        // El personaje ya no está en el suelo después de saltar
        enSuelo = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el personaje está en el suelo al colisionar con un objeto etiquetado como "Suelo"
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }
}
