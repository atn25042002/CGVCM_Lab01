using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparador : MonoBehaviour
{
    private bool shooting;
    [SerializeField] private GameObject boltPrefab;
    [SerializeField] private Puntaje puntaje;
    [SerializeField] private GestorSonidos gestorSonidos;
    [SerializeField] private AudioSource sonidoDisparo;
    [SerializeField] private AudioSource sonidoRecargando;
    public int tipo;

    void Start()
    {
        shooting= false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(TryShoot())
            {
                shooting= true;
                Shoot();
            } 
            else
            {
                sonidoRecargando.Play();
                Debug.Log("Todavia hay una bala");
            }
        }
    }

    public void Reload()
    {
        shooting = false;
    }

    private bool TryShoot()
    {
        return !shooting;
    }

    private void Shoot()
    {        
        Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionMouse.z = 0f; // Asegurarse de que la coordenada Z sea la misma que la del objeto

        // Calcular la dirección desde la posición del objeto hacia la posición del mouse
        Vector2 direccion = (posicionMouse - transform.position).normalized;
        
        GameObject nueva = Instantiate(boltPrefab, transform.position + (Vector3)(30.0f * direccion), Quaternion.identity);
        sonidoDisparo.Play();

        Proyectil p= nueva.GetComponent<Proyectil>();
        p.SetDisparador(this, this.gestorSonidos, this.puntaje, this.tipo);
    }
}
