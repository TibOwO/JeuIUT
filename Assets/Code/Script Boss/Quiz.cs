using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using MagicPigGames;
using System.IO;

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
    public string selectedProfessor;
    public Image fadeImage;
    public GameObject sceneParent; // Référence à l'objet parent de la scène

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

        string databasePath = Path.Combine(Application.streamingAssetsPath, "DB_Unity.db");
        conn = "URI=file:" + databasePath;
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
                dbCmd.CommandText = "SELECT * FROM QUESTIONS WHERE Professeur = @Professeur";
                dbCmd.Parameters.AddWithValue("@Professeur", selectedProfessor);
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
            Debug.Log("Victoire sérée!");
            // Ici, vous pouvez gérer la logique de fin de jeu ou de réinitialisation
        }

        // Sélection aléatoire d'une question et la retirer de la liste
        int randomIndex = Random.Range(0, questions.Count);
        var currentQuestion = questions[randomIndex];
        questions.RemoveAt(randomIndex); // Retire la question utilisée de la liste

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
            bossHealth -= 10;
            UpdateBossHealthBar();
            Debug.Log($"Santé du boss restante : {bossHealth}");

            if (bossHealth <= 0)
            {

                GlobalQuest.QuestStep += 1;

                if (SceneManager.GetActiveScene().name == "Boss Makssoud")
                {
                    SceneManager.LoadScene("Credits");
                }
                else
                {
                    SceneManager.LoadScene(winSceneName);
                }
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                if (!ElementalInventory.Instance.contains("Cle 2", 1))
                {
                    ElementalInventory.Instance.addItem("Cle 2", 1, randomColor, "Cle qui ouvre la porte 2");
                }
                else if (!ElementalInventory.Instance.contains("Cle 3", 1))
                {
                    ElementalInventory.Instance.addItem("Cle 3", 1, randomColor, "Cle qui ouvre la porte 3");
                }
                else if (!ElementalInventory.Instance.contains("Cle 4", 1))
                {
                    ElementalInventory.Instance.addItem("Cle 4", 1, randomColor, "Cle qui ouvre la porte 4");
                }
                else if (!ElementalInventory.Instance.contains("Cle 5", 1))
                {
                    ElementalInventory.Instance.addItem("Cle 5", 1, randomColor, "Cle qui ouvre la porte 5");
                }
                else if (!ElementalInventory.Instance.contains("Cle 6", 1))
                {
                    ElementalInventory.Instance.addItem("Cle 6", 1, randomColor, "Cle qui ouvre la porte 6");
                }
                else if (!ElementalInventory.Instance.contains("Cle 7", 1))
                {
                    ElementalInventory.Instance.addItem("Cle 7", 1, randomColor, "Cle qui ouvre la porte 7");
                }

            }
        }
        else
        {
            Debug.Log("Incorrect !");
            playerLives -= 1;
            StartCoroutine(LoseLifeRoutine());
            Debug.Log($"Vies du joueur restantes : {playerLives}");
        }
        PoseUneQuestion();
    }

    IEnumerator LoseLifeRoutine()
    {
        foreach (var heart in heartImages)
        {
            heart.enabled = true;
        }


        StartCoroutine(ScreenShake());

        UpdateHeartSprites();

        yield return new WaitForSeconds(1f);


        if (playerLives <= 0)
        {
            Debug.Log("Activation coroutine");
            fadeImage.gameObject.SetActive(true);
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



    private IEnumerator ScreenShake()
    {
        float duration = 0.5f; // Durée de l'effet de secousse
        float magnitude = 0.8f; // Amplitude de la secousse

        Vector3 originalPosition = sceneParent.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            sceneParent.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        sceneParent.transform.localPosition = originalPosition;
    }







    private IEnumerator FadeToBlackAndLoadScene()
    {
        Debug.Log("Début de la coroutine de fondu au noir");
        Debug.Log("Image de fondu activée");

        float fadeDuration = 0.8f;
        float fadeStep = 0.2f;

        for (float i = 0; i <= 1; i += fadeStep)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            Debug.Log("Fondu au noir progressif : Alpha = " + i);
            yield return new WaitForSeconds(fadeDuration * fadeStep);
        }

        Debug.Log("Fondu au noir terminé");

        yield return new WaitForSeconds(1f);
        Debug.Log("Chargement de la nouvelle scène : " + loseSceneName);

        SceneManager.LoadScene(loseSceneName);
    }



}
