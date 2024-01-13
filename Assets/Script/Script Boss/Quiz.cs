using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    public TextMeshProUGUI txtQuestion;
    public TextMeshProUGUI txtBtnG;
    public TextMeshProUGUI txtBtnD;
    public string conn;

    public string Reponse;
    public TypeWriter typeWriter;
    public List<Question> questions;
    public GameObject ZoneReponse1;
    public GameObject ZoneReponse2;

    public ButtonController buttonController;

    public struct Question
    {
        public string Text;
        public List<string> Answers;
        public string CorrectAnswer;

        public Question(string text, List<string> answers, string correctAnswer)
        {
            Text = text;
            Answers = answers;
            CorrectAnswer = correctAnswer;
        }
    }

    void Awake()
    {
        txtQuestion = GameObject.Find("txtQuestion").GetComponent<TextMeshProUGUI>();
        txtBtnG = GameObject.Find("TxtG").GetComponent<TextMeshProUGUI>();
        txtBtnD = GameObject.Find("TxtD").GetComponent<TextMeshProUGUI>();

        buttonController = GetComponent<ButtonController>();

        if (txtQuestion == null || txtBtnG == null || txtBtnD == null || buttonController == null)
        {
            Debug.LogError("Un ou plusieurs composants d'interface utilisateur ne sont pas attachés au script.");
        }

        conn = "URI=file:" + Application.dataPath + "/Plugins/DB_Unity.db";

        LoadQuestionsFromDatabase();
    }

    void Start()
    {
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
                if (hitCollider.gameObject == ZoneReponse1)
                {
                    buttonController.CheckAnswer(txtBtnG.text);
                }
                else if (hitCollider.gameObject == ZoneReponse2)
                {
                    buttonController.CheckAnswer(txtBtnD.text);
                }
            }
        }
    }

    public class ButtonController : MonoBehaviour
    {
        public Button ReponseG;
        public Button ReponseD;

        public Quiz quiz;

        void Start()
        {
            ReponseG.onClick.AddListener(() => CheckAnswer(quiz.txtBtnG.text));
            ReponseD.onClick.AddListener(() => CheckAnswer(quiz.txtBtnD.text));
        }

        public void CheckAnswer(string selectedAnswer)
        {
            if (selectedAnswer == quiz.Reponse)
            {
                Debug.Log("Correct!");
                quiz.txtQuestion.text = "Gagné!";
            }
            else
            {
                Debug.Log("Incorrect!");
                quiz.txtQuestion.text = "Perdu!";
            }
            quiz.PoseUneQuestion();
        }
    }



    // Méthode pour charger les questions depuis la base de données
    void LoadQuestionsFromDatabase()
    {
        questions = new List<Question>();

        using (var dbConnection = new SqliteConnection(conn))
        {
            dbConnection.Open();

            using (var dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = "SELECT * FROM QUESTIONS";
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questionID = reader.GetInt32(0);
                        string questionText = reader.GetString(1);

                        List<string> answers = new List<string>();
                        string correctAnswer = "";

                        // Utilisez un nouveau SqliteCommand pour la requête des réponses
                        using (var answerCmd = dbConnection.CreateCommand())
                        {
                            answerCmd.CommandText = "SELECT * FROM ANSWERS WHERE ParentID = @ParentID";
                            answerCmd.Parameters.AddWithValue("@ParentID", questionID);

                            using (IDataReader answerReader = answerCmd.ExecuteReader())
                            {
                                while (answerReader.Read())
                                {
                                    string answerText = answerReader.GetString(1);
                                    answers.Add(answerText);
                                    if (answerReader.GetInt32(3) == 1) // isCorrect
                                    {
                                        correctAnswer = answerText;
                                    }
                                }
                            }
                        }

                        // Ajout de la question et des réponses à la liste
                        questions.Add(new Question(questionText, answers, correctAnswer));
                    }
                }
            }
            dbConnection.Close();
        }
    }


    // Méthode pour poser une question
    void PoseUneQuestion()
    {
        if (questions.Count == 0)
        {
            Debug.LogError("Aucune question n'a été chargée de la base de données.");
            return;
        }

        // Sélection aléatoire d'une question
        var currentQuestion = questions[Random.Range(0, questions.Count)];
        Reponse = currentQuestion.CorrectAnswer;

        typeWriter.SetText(currentQuestion.Text); // Affiche la question
        txtBtnG.text = currentQuestion.Answers[0]; // Affiche la première réponse
        txtBtnD.text = currentQuestion.Answers[1]; // Affiche la deuxième réponse
    }
}
