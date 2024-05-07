using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemigoSimple : MonoBehaviour
{
    public float velocidad = 50f;
    public float fuerzaSalto = 100f; // Fuerza del salto
    private bool enSuelo= false;
    public float distancia = 5f; // Distancia que recorre el enemigo
    private bool moviendoseDerecha = false; // Bandera para controlar la dirección del movimiento
    public float tiempoEntreDisparos = 5f;
    private Vector3 puntoInicial; // Posición inicial del enemigo
    private Rigidbody2D rb;
    private Transform spriteTransform;
    [SerializeField] private GameObject boltPrefab;
    [SerializeField] private GameObject objetivo;
    [SerializeField] private Puntaje puntaje;
    [SerializeField] private AudioSource sonidoDisparo;
    [SerializeField] private GestorSonidos gestorSonidos;
    public int tipo;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = GetComponentInChildren<Image>().transform;
        puntoInicial = transform.position;
        InvokeRepeating("Disparar", tiempoEntreDisparos, tiempoEntreDisparos);
    }

    // Update is called once per frame
    void Update()
    {
        if (moviendoseDerecha)
        {
            transform.Translate(Vector2.right * velocidad * Time.deltaTime); // Movemos hacia la derecha
            if (transform.position.x >= puntoInicial.x + distancia)
            {
                CambiarDireccion();
            }
        }
        else
        {
            transform.Translate(Vector2.left * velocidad * Time.deltaTime); // Movemos hacia la izquierda
            if (transform.position.x <= puntoInicial.x - distancia)
            {
                CambiarDireccion();
            }
        }
    }

    void CambiarDireccion()
    {
        if (Random.value < 0.5f)
        {
            Saltar();
        }
        moviendoseDerecha = !moviendoseDerecha; // Cambiamos la dirección
        Vector3 escala = transform.localScale; // Obtenemos la escala actual
        escala.x *= -1; // Invertimos la escala en el eje X para voltear la imagen
        transform.localScale = escala; // Asignamos la nueva escala
    }

    void Saltar()
    {
        if(!enSuelo){
            return;
        }
        // Aplicar una fuerza vertical al Rigidbody2D para simular el salto
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);

        // El personaje ya no está en el suelo después de saltar
        enSuelo = false;
    }

    void Disparar()
    {
        Vector3 posicionObj = objetivo.transform.position;
        posicionObj.z = 0f; // Asegurarse de que la coordenada Z sea la misma que la del objeto

        // Calcular la dirección desde la posición del objeto hacia la posición del objetivo
        Vector2 direccion = (posicionObj - transform.position).normalized;
        
        GameObject nueva = Instantiate(boltPrefab, transform.position + (Vector3)(30.0f * direccion), Quaternion.identity);
        sonidoDisparo.Play();

        ProyectilEnemigo p= nueva.GetComponent<ProyectilEnemigo>();
        p.SetDisparador(this.objetivo,this.gestorSonidos, this.puntaje, this.tipo);
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
