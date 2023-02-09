using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Importamos el paquete necesario para Network 
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    private string apiURL;

    private Preguntas pre;

    private bool ready, mostrarPantalla;
    private int indicePregunta,indiceRespuesta;
    private int aciertos, fallos;
    private List<string> respuestas;
    private string resCompletas;
    private string aciertoFallo;

    // Start is called before the first frame update
    void Start()
    {
        ready = false;
        mostrarPantalla = false;
        aciertos = 0;
        fallos = 0;
        resCompletas= "";        
        apiURL = "https://opentdb.com/api.php?amount=10&category=20&type=multiple";
        // Iniciar una corrutina para pedir los datos a una api de una web pasando como parametro la llmada get
        StartCoroutine(GetRequest(apiURL));
        
    }

    // Update is called once per frame
    void Update()
    {
        // si estamos listos empieza el juego
        if(ready){
            
            string userInput=Input.inputString.ToUpper();
            
            if(userInput!=""){
                indiceRespuesta = userInput[0]-'1';
                
                if(indiceRespuesta<0|| indiceRespuesta>= respuestas.Count){
                    indiceRespuesta=-1;
                    aciertoFallo="";
                }else{if(indiceRespuesta==0){
                        aciertoFallo="Ganaste";
                        ready = false;
                        aciertos++;
                        Invoke("LanzarJugada",1.5f);
                    }else{
                        aciertoFallo="Perdiste";
                        ready = false;
                        fallos++;
                        Invoke("LanzarJugada",1.5f);
                    }
                }
            }
           

        }
    }

    private void LanzarJugada(){

        indicePregunta = Random.Range(0,pre.results.Count);
        indiceRespuesta=-1;
        ready = true;
        aciertoFallo="";
        resCompletas="";
        GuardarRespuestas();

    }

    private void GuardarRespuestas(){
        respuestas = new List<string>();
        int indice = 0;

        respuestas.Add(pre.results[indicePregunta].correct_answer);

        foreach (string str in pre.results[indicePregunta].incorrect_answers)
        {
            respuestas.Add(str);
        }


        foreach (string str in respuestas)
        {
            
            resCompletas += $"{(char)(indice+'1')}) {str} \n";
            indice++;
            
        }

        // TO DO falta aleatorizar el orden
    }

    private void OnGUI(){

        if(mostrarPantalla){
        GUI.Label(new Rect(10,10,450,25), $"Aciertos= {aciertos} \t Fallos= {fallos}");

        GUI.Label(new Rect(10,40,450,25), "Dificultad: " + pre.results[indicePregunta]?.difficulty);

        GUI.Label(new Rect(10,80,800,300),pre.results[indicePregunta]?.question);

        GUI.Label(new Rect(10,120,450,300),resCompletas);
        
            if(indiceRespuesta>=0 && !ready){
                GUI.Label(new Rect(10,210,450,25), $"La respuesta correcta es: {respuestas[indiceRespuesta]}");
                GUI.Label(new Rect(10,230,100,25), aciertoFallo);
            }
        }
        
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
                    pre = Preguntas.CreateFromJSON(jsonString);
                    // bool de control para poder ejecutar el juego una vez tenemos todos los datos
                    mostrarPantalla = true;
                    LanzarJugada();
                    break;
            }
            
        }
    }
}
