using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Aviso : MonoBehaviour
{
    private string aviso;
    private TextMeshProUGUI textmesh;

    public void setText(string a){
        aviso= a;
        GetComponent<TextMeshProUGUI>().text= aviso;
        StartCoroutine(Duracion());
    }

    IEnumerator Duracion(){
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
