using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;

public class Quiz : MonoBehaviour
{
    private TextMeshProUGUI txtQuestion;
    private TextMeshProUGUI txtBtnG;
    private TextMeshProUGUI txtBtnD;
    private string conn; // Chaîne de connexion à la base de données

    public string Reponse;
    public TypeWriter typeWriter; // Variable publique pour stocker la référence au script TypeWriter
    public List<Question> questions; // Liste pour stocker les questions et réponses

    // Structure pour représenter une question et ses réponses
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
        // Initialisation des composants (s'assurer que les noms correspondent à ceux dans votre scène)
        txtQuestion = GameObject.Find("txtQuestion").GetComponent<TextMeshProUGUI>();
        txtBtnG = GameObject.Find("TxtG").GetComponent<TextMeshProUGUI>();
        txtBtnD = GameObject.Find("TxtD").GetComponent<TextMeshProUGUI>();

        // Vérifiez si les composants de l'interface utilisateur ont été trouvés
        if (txtQuestion == null || txtBtnG == null || txtBtnD == null)
        {
            Debug.LogError("Un ou plusieurs composants d'interface utilisateur ne sont pas attachés au script.");
        }

        // Initialisation de la chaîne de connexion à la base de données
        conn = "URI=file:" + Application.dataPath + "/Plugins/DB_Unity.db";

        // Chargement des questions depuis la base de données
        LoadQuestionsFromDatabase();
    }

    void Start()
    {
        // Pose la première question
        PoseUneQuestion();
    }

    void Update()
    {
        // Gestion des entrées utilisateur ici...
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);
            if (hitCollider != null)
            {
                if (hitCollider.gameObject == txtBtnG.gameObject)
                {
                    CheckAnswer(txtBtnG.text);
                }
                else if (hitCollider.gameObject == txtBtnD.gameObject)
                {
                    CheckAnswer(txtBtnD.text);
                }
            }
        }
    }

    // Méthode pour vérifier la réponse
    void CheckAnswer(string selectedAnswer)
    {
        if (selectedAnswer == Reponse)
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Incorrect!");
        }
        PoseUneQuestion(); // Pose la question suivante
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
