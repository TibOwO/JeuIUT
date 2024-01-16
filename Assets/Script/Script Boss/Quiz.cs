using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using MagicPigGames;

public class Quiz : MonoBehaviour
{
    public TextMeshProUGUI txtQuestion;
    public Button btnReponseG;
    public Button btnReponseD;
    public string conn;
    public ProgressBar bossHealthBar; // Référence à la barre de progression du boss

    public int bossHealth, bossMaxHealth = 20;
    public int playerLives = 3; // Vies initiales du joueur
    public string winSceneName = "Salle 1"; // Nom de la scène de victoire
    public Image[] heartImages; // Références aux images des coeurs dans l'UI
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;
    public string loseSceneName = "Couloir"; // Nom de la scène de défaite
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

    void Update()
    {
        if (typeWriter.IsTyping)
        {
            btnReponseG.interactable = false;
            btnReponseD.interactable = false;
        }
        else
        {
            btnReponseG.interactable = true;
            btnReponseD.interactable = true;
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
        // Initialiser les cœurs comme invisibles
        foreach (var heart in heartImages)
        {
            heart.enabled = false;
        }
        PoseUneQuestion();
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


    public void OnButtonClicked(Button buttonClicked)
    {
        string buttonText = buttonClicked.GetComponentInChildren<TextMeshProUGUI>().text;
        CheckAnswer(buttonText);
    }

    private void UpdateBossHealthBar()
    {
        float healthPercentage = (float)bossHealth / bossMaxHealth;
        bossHealthBar.SetProgress(healthPercentage);
        Debug.Log("Boss Health: " + healthPercentage * 100 + "%");
    }

    public void CheckAnswer(string selectedAnswer)
    {
        if (selectedAnswer == Reponse)
        {
            Debug.Log("Correct !");
            bossHealth -= 10; // Diminuez la santé du boss
            UpdateBossHealthBar();
            Debug.Log($"Santé du boss restante : {bossHealth}");

            if (bossHealth <= 0)
            {
                Debug.Log("Le joueur gagne !");
                SceneManager.LoadScene(winSceneName); // Chargez la scène de victoire
            }
        }
        else
        {
            Debug.Log("Incorrect !");
            playerLives -= 1; // Diminuez les vies du joueur
            StartCoroutine(LoseLifeRoutine());
            Debug.Log($"Vies du joueur restantes : {playerLives}");

            if (playerLives <= 0)
            {
                Debug.Log("Le joueur perd !");
                SceneManager.LoadScene(loseSceneName); // Chargez la scène de défaite
            }
        }
        PoseUneQuestion();
    }

    IEnumerator LoseLifeRoutine()
    {
        // Afficher brièvement les cœurs
        foreach (var heart in heartImages)
        {
            heart.enabled = true;
        }

        // Vibrer l'écran
        StartCoroutine(ScreenShake());

        // Mettre à jour les cœurs
        UpdateHeartSprites();

        // Attendre un moment
        yield return new WaitForSeconds(1f);

        // Si le joueur n'a plus de vie
        if (playerLives <= 0)
        {
            StartCoroutine(FadeToBlackAndLoadScene());
        }
        else
        {
            foreach (var heart in heartImages)
            {
                heart.enabled = false;
            }
        }
    }

    void UpdateHeartSprites()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < playerLives ? fullHeartSprite : emptyHeartSprite;
        }
    }

    IEnumerator ScreenShake()
    {
        float duration = 4.5f; // Durée de l'effet de vibration
        float magnitude = 50f; // Amplitude de la vibration

        GameObject cameraGameObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraGameObject == null)
        {
            Debug.LogError("Camera with tag 'MainCamera' not found.");
            yield break; // Arrête la coroutine si la caméra n'est pas trouvée
        }

        Vector3 originalPosition = cameraGameObject.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraGameObject.transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraGameObject.transform.localPosition = originalPosition;
    }





    IEnumerator FadeToBlackAndLoadScene()
    {
        // Implémentez la transition vers l'écran noir ici
        yield return new WaitForSeconds(1f); // Durée de l'effet
        SceneManager.LoadScene(loseSceneName);
    }

}
