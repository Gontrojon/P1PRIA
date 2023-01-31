using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase que almacenara los datos de cada pregunta
[System.Serializable]
public class Pregunta
{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;
}
