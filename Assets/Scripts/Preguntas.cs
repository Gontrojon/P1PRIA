using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// clase que almacenara la cabecera de las preguntas y la llamda para la interpretacion del json
[System.Serializable]
public class Preguntas
{
    public int response_code;
    public List<Preguntas> results;
    
    // metodo que realiza la asignacion de los datos con la llamada a la utilidad de unity
    public static Preguntas CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Preguntas>(jsonString);
    }
}
