using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quiz : MonoBehaviour
{
    private TextMeshProUGUI txtQuestion;
    private TextMeshProUGUI txtBtnG;
    private TextMeshProUGUI txtBtnD;
    private int Nr; // Nombre aléatoire

    public string Reponse;
    public TypeWriter typeWriter; // Variable publique pour stocker la référence au script TypeWriter

    // Déclaration du tableau
    string[] Quizz = new string[3];

    void Awake()
    {
        // Composants
        txtQuestion = GameObject.Find("txtQuestion").GetComponent<TextMeshProUGUI>();
        txtBtnG = GameObject.Find("TxtG").GetComponent<TextMeshProUGUI>();
        txtBtnD = GameObject.Find("TxtD").GetComponent<TextMeshProUGUI>();

        if (txtQuestion == null || txtBtnG == null || txtBtnD == null)
        {
            Debug.LogError("Une ou plusieurs références ne sont pas initialisées. Assurez-vous que les noms d'objets sont corrects et que les composants sont attachés.");
        }
    }

    void Start()
    {
        // Déclaration du contenu du tableau
        Quizz[0] = "HTML est un langage de programmation,Vrai,Faux,Faux";
        Quizz[1] = "Quelle est la valeur de 7 que multiplie 8?,64,56,56";
        Quizz[2] = "Qu'est-ce qu'un réseau informatique ?,Un ensemble d'appareils connectés qui communiquent entre eux,Un logiciel de traitement de texte,Un ensemble d'appareils connectés qui communiquent entre eux";

        PoseUneQuestion();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);
            if (hitCollider != null)
            {
                if (hitCollider.gameObject == txtBtnG.gameObject)
                {
                    if (txtBtnG.text == Reponse)
                    {
                        Debug.Log("Gagné");
                    }
                    else
                    {
                        Debug.Log("Perdu");
                    }
                    PoseUneQuestion(); // Affichez la question suivante.
                }
                else if (hitCollider.gameObject == txtBtnD.gameObject)
                {
                    if (txtBtnD.text == Reponse)
                    {
                        Debug.Log("Gagné");
                    }
                    else
                    {
                        Debug.Log("Perdu");
                    }
                    PoseUneQuestion(); // Affichez la question suivante.
                }
            }
        }
    }

    void PoseUneQuestion()
    {
        Nr = Random.Range(0, Quizz.Length);
        string[] Col = Quizz[Nr].Split(',');
        string questionText = Col[0]; // Extrait le texte de la question
        Reponse = Col[3];

        typeWriter.SetText(questionText);

        txtBtnG.text = Col[1]; // Réponse de gauche
        txtBtnD.text = Col[2]; // Réponse de droite

        Debug.Log("Question : " + questionText);
    }
}
