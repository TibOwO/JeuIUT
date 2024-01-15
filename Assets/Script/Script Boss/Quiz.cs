using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    public TextMeshProUGUI txtQuestion;
    public Button btnReponseG;
    public Button btnReponseD;
    public string conn;

    public string Reponse;
    public TypeWriter typeWriter;
    public List<Question> questions;

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
        btnReponseG = GameObject.Find("ReponseG").GetComponent<Button>();
        btnReponseD = GameObject.Find("ReponseD").GetComponent<Button>();

        if (txtQuestion == null || btnReponseG == null || btnReponseD == null)
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
    // jessaie de debuger
    if (Input.GetMouseButtonDown(0))
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

       
        if (btnReponseG != null && btnReponseD != null)
        {
            if (hitCollider != null)
            {
                if (hitCollider.gameObject == btnReponseG.gameObject)
                {
                    CheckAnswer(btnReponseG.GetComponentInChildren<TextMeshProUGUI>().text);
                }
                else if (hitCollider.gameObject == btnReponseD.gameObject)
                {
                    CheckAnswer(btnReponseD.GetComponentInChildren<TextMeshProUGUI>().text);
                }
            }
        }
        else
        {
            Debug.LogError("Un ou plusieurs boutons ne sont pas initialisés.");
        }
    }
}

 public class ButtonController : MonoBehaviour
    {
        private Quiz quiz; 

        void Start()
        {
            quiz = FindObjectOfType<Quiz>(); 

            if (quiz == null)
            {
                Debug.LogError("La référence à l'instance de Quiz est nulle.");
                return;
            }

            
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        void OnClick()
        {
     
            if (quiz != null)
            {
                quiz.CheckAnswer(GetComponentInChildren<TextMeshProUGUI>().text);
            }
        }
    }

 
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
                                    if (answerReader.GetInt32(3) == 1) 
                                    {
                                        correctAnswer = answerText;
                                    }
                                }
                            }
                        }

                        
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

    
    btnReponseG.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Answers[0]; // Affiche la première réponse
    btnReponseD.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Answers[1]; // Affiche la deuxième réponse
}


public void CheckAnswer(string selectedAnswer)
    {
        if (selectedAnswer == Reponse)
        {
            Debug.Log("Correct !");
            txtQuestion.text = "Gagné !";
        }
        else
        {
            Debug.Log("Incorrect !");
            txtQuestion.text = "Perdu !";
        }
        PoseUneQuestion();
    }
}
