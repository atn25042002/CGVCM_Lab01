using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public Disparador disparador;
    private Rigidbody2D rigidbody2d;
    private Puntaje puntaje;
    private bool fired = false;
    private float fuerza= 200.0f;
    public int tipo;
    [SerializeField] private GestorSonidos sonidos;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(fired)
        {
            float angleRad = Mathf.Atan2(rigidbody2d.velocity.y, rigidbody2d.velocity.x);
            float angleDeg = (180 / Mathf.PI) * angleRad - 90; // Offset by 90 Degrees

            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        }
    }

    public void Shoot()
    {
         // Obtener la posición del mouse en el mundo
        Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionMouse.z = 0f; // Asegurarse de que la coordenada Z sea la misma que la del objeto

        // Calcular la dirección desde la posición del objeto hacia la posición del mouse
        Vector2 direccion = (posicionMouse - transform.position).normalized;

        rigidbody2d.AddForce(direccion * fuerza, ForceMode2D.Impulse);
        fired = true;
        StartCoroutine(Duracion());
        StartCoroutine(Recargar());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //puntaje.añadirPuntos(20.0f);
        Transform objTransform = collision.gameObject.transform;
        switch (collision.gameObject.tag)
        {
            case "Enemigo":
                ImpactoEnemigo(collision.gameObject.transform);
                break;
            case "Player":
                ImpactoPropio(collision.gameObject.transform);
                break;
            default:
                // Lógica para otras etiquetas si es necesario
                break;
        }
    }

    private void ImpactoEnemigo(Transform objTransform){
        float posX= objTransform.position.x;
        float posY= objTransform.position.y;
        float scaleX= objTransform.localScale.x;

        if(scaleX == 1 && posX <= transform.position.x){
            GolpeEnemigo("Golpe critico al enemigo", posX, posY, 80.0f);
        }else if(scaleX == -1 && posX >= transform.position.x){
            GolpeEnemigo("Golpe critico al enemigo", posX, posY, 40.0f);
        }else{
            GolpeEnemigo("Golpe normal", posX, posY, 40.0f);
        }
        
        sonidos.PlaySonido(tipo);
        disparador.Reload();
        Destroy(gameObject);
    }

    private void GolpeEnemigo(string aviso, float posX, float posY, float puntos){
        puntaje.avisar(aviso , posX, posY);
        puntaje.añadirPuntos(puntos);
    }

    private void ImpactoPropio(Transform objTransform){
        float posX= objTransform.position.x;
        float posY= objTransform.position.y;
        float scaleX= objTransform.localScale.x;

        if(scaleX == 1 && posX <= transform.position.x){
            GolpePropio("Golpe critico al usuario", posX, posY, 40.0f);
        }else if(scaleX == -1 && posX >= transform.position.x){
            GolpePropio("Golpe critico al usuario", posX, posY, 40.0f);
        }else{
            GolpePropio("Golpe normal al usuario", posX, posY, 20.0f);
        }
        sonidos.PlaySonido(tipo);
        disparador.Reload();
        Destroy(gameObject);
    }

    private void GolpePropio(string aviso, float posX, float posY, float puntos){
        puntaje.avisar(aviso , posX, posY);
        puntaje.quitarPuntos(puntos);
    }

    public void SetDisparador(Disparador d, GestorSonidos gs, Puntaje p, int t){
        rigidbody2d = GetComponent<Rigidbody2D>();
        disparador= d;
        sonidos= gs;
        puntaje= p;
        tipo= t;
        Shoot();
    }

    IEnumerator Duracion(){
        yield return new WaitForSeconds(8.0f);
        fired = false;
        disparador.Reload();
        Destroy(gameObject);
    }

    IEnumerator Recargar(){
        yield return new WaitForSeconds(4.0f);
        disparador.Reload();
    }
}
