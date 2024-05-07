using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorSonidos : MonoBehaviour
{
    [SerializeField] private AudioSource sonidoFuego, sonidoAgua, sonidoPlanta;
    
    public void PlaySonido(int nTipo){
        switch(nTipo){
            case 1:
                sonidoAgua.Play();
                break;
            case 2:
                sonidoFuego.Play();
                break;
            case 3:
                sonidoPlanta.Play();
                break;
            default:
                print("Tipo no registrado");
                break;
        }
    }
}
