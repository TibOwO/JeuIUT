using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reponse : MonoBehaviour
{
    void OnMouseDown()
    {
        if (GameObject.Find("Canvas").GetComponent<Quiz>().Reponse == transform.GetChild(0).GetComponent<TextMesh>().text)
        {
            Debug.Log("Gagné");
        }
        else
        {
            Debug.Log("Perdu");
        }
    }
}
