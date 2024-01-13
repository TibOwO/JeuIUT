using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class ReponseGButton : MonoBehaviour
{
    public Quiz quiz; // Attachez le script Quiz ici dans l'inspecteur Unity.

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        quiz.CheckAnswer(quiz.txtBtnG.text);
    }
}