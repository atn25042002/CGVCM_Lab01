using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    public GameObject objetivo;
    private bool shotting;
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
        shotting= false;
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
        if(shotting){
            return;
        }
        // Obtener la posición del mouse en el mundo
        Vector3 posicionObj = objetivo.transform.position;
        posicionObj.z = 0f; // Asegurarse de que la coordenada Z sea la misma que la del objeto

        // Calcular la dirección desde la posición del objeto hacia la posición del mouse
        Vector2 direccion = (posicionObj - transform.position).normalized;

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
                break;
            case "Player":
                ImpactoObjetivo(collision.gameObject.transform);
                break;
            default:
                // Lógica para otras etiquetas si es necesario
                break;
        }
    }

    private void ImpactoObjetivo(Transform objTransform){
        float posX= objTransform.position.x;
        float posY= objTransform.position.y;
        float scaleX= objTransform.localScale.x;

        if(scaleX == 1 && posX <= transform.position.x){
            GolpeObjetivo("Usuario recibio golpe critico", posX, posY, 40.0f);
        }else if(scaleX == -1 && posX >= transform.position.x){
            GolpeObjetivo("Usuario recibio golpe critico", posX, posY, 40.0f);
        }else{
            GolpeObjetivo("Usuario recibio golpe", posX, posY, 20.0f);
        }
        sonidos.PlaySonido(tipo);
        shotting= false;
        Destroy(gameObject);
    }

    private void GolpeObjetivo(string aviso, float posX, float posY, float puntos){
        puntaje.avisar(aviso , posX, posY);
        puntaje.quitarPuntos(puntos);
    }

    public void SetDisparador(GameObject obj, GestorSonidos gs, Puntaje p, int t){
        rigidbody2d = GetComponent<Rigidbody2D>();
        objetivo= obj;
        sonidos= gs;
        puntaje= p;
        tipo= t;
        Shoot();
    }

    IEnumerator Duracion(){
        yield return new WaitForSeconds(8.0f);
        fired = false;
        shotting= false;
        Destroy(gameObject);
    }

    IEnumerator Recargar(){
        yield return new WaitForSeconds(4.0f);
        shotting= false;
    }
}
