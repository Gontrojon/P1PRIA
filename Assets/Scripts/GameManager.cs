using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Iniciar una corrutina para pedir los datos a una api de una web pasando como parametro la llmada get
        StartCoroutine(GetRequest("https://opentdb.com/api.php?amount=10&category=20&type=multiple"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // creacion del metodo que ejecutara la corrutina
    IEnumerator GetRequest(string uri)
    {
        yield return new WaitForSeconds(2);
        // Mensaje de control
        Debug.Log("Corrutina finalizada");
    }
}
