using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Importamos el paquete necesario para Network 
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    private string apiURL;
    // Start is called before the first frame update
    void Start()
    {
        apiURL = "https://opentdb.com/api.php?amount=10&category=20&type=multiple"
        // Iniciar una corrutina para pedir los datos a una api de una web pasando como parametro la llmada get
        StartCoroutine(GetRequest(apiURL));
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
                    // guardamos en un string la respuesta 
                    string jsonString = webRequest.downloadHandler.text;
                    // creamos la clase que almacenara todos los datos
                    Preguntas pre = Preguntas.CreateFromJSON(jsonString);
                    // Salida de el numero de preguntas solicitada
                    Debug.Log($"Numero de preguntas solicitadas: {pre.results.Count}");
                    Debug.Log($"Categoria: {pre.results[0].category}");
                    Debug.Log($"Dificultad: {pre.results[0].difficulty}");
                    Debug.Log($"Pregunta: {pre.results[0].question}");
                    Debug.Log($"Respuesta Correcta: {pre.results[0].correct_answer}");
                    break;
            }
            
        }
    }
}
