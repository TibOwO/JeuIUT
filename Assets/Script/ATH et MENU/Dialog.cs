using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//Pour qu'on puisse la réutilisé dans d'autre script
public class Dialog
{
    public string name;

    [TextArea(3, 10)]//défini la taille de la zone de texte
    public string[] sentences;
}
