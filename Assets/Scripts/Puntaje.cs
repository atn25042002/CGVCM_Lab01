using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puntaje : MonoBehaviour
{
    private float puntos;
    private TextMeshProUGUI textmesh;
    [SerializeField] private GameObject avisoPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        textmesh = GetComponent<TextMeshProUGUI>();
        puntos= 0.0f;
    }

    public void añadirPuntos(float p){
        puntos+= p;
        textmesh.text= puntos.ToString("0");
    }

    public void quitarPuntos(float p){
        if(puntos <= p){
            puntos= 0;
        }else{
            puntos-= p;
        }
        textmesh.text= puntos.ToString("0");
    }

    public void avisar(string s, float x, float y){
        GameObject nueva = Instantiate(avisoPrefab, transform);
        nueva.transform.position = new Vector3(x, y + 10.0f, 0f);
        Aviso aviso= nueva.GetComponent<Aviso>();
        aviso.setText(s);
    }
}
