using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Importamos el paquete necesario para Network 
using UnityEngine.Networking;

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
        // creacion de la peticion web con la clase UnityWebRequest
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Peticion y espera a que la petición sea procesada.
            yield return webRequest.SendWebRequest();
            // separamos en un array de strings la web a la que se hace la peticion para poder obtener los errores
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            
            // Miramos que estado nos devuelve la peticion y depuramos
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
             // guardamos en un string la respuesta 
            string jsonString = webRequest.downloadHandler.text;
            
            // creamos la clase que almacenara todos los datos
            Preguntas pre = Preguntas.CreateFromJSON(jsonString);
    
            // entero para iterar el nº de pregunta
            int preguntaN = 0;
            // recorremos los resultados y arrojamos lo que contiene
            foreach (Pregunta pregunta in pre.results)
            {
                preguntaN++;
                Debug.Log($"Pregunta numero : {preguntaN}");
                Debug.Log($"Categoria: {pregunta.category}");
                //Debug.Log(respuestas.type);
                Debug.Log($"Dificultad: {pregunta.difficulty}");
                Debug.Log($"Pregunta: {pregunta.question}");
                Debug.Log("Respuestas");
                Debug.Log(pregunta.correct_answer);
                for (int i = 0; i < pregunta.incorrect_answers.Length; i++)
                {
                    Debug.Log(pregunta.incorrect_answers[i]);
                }
            }
        }
    }
}
