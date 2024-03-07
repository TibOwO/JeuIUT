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

/// <summary>
/// Gère le fonctionnement du quiz et l'interaction avec le boss dans le jeu.
/// </summary>
public class Quiz : MonoBehaviour
{
    // Déclaration des variables publiques pour les éléments de l'interface utilisateur et les paramètres du jeu
    public TextMeshProUGUI txtQuestion; // Zone de texte pour afficher la question
    public Button btnReponseG; // Bouton pour la réponse gauche
    public Button btnReponseD; // Bouton pour la réponse droite
    public string conn; // Chaîne de connexion à la base de données
    public ProgressBar bossHealthBar; // Référence à la barre de progression du boss
    public int bossHealth, bossMaxHealth = 20; // Santé et santé maximale du boss
    public int playerLives = 3; // Vies initiales du joueur
    public string winSceneName = "Salle 1"; // Nom de la scène de victoire
    public Image[] heartImages; // Références aux images des coeurs dans l'UI
    public Sprite fullHeartSprite; // Sprite pour un coeur plein
    public Sprite emptyHeartSprite; // Sprite pour un coeur vide
    public string loseSceneName = "Couloir"; // Nom de la scène de défaite
    public string Reponse; // Réponse correcte à la question actuelle
    public TypeWriter typeWriter; // Gestionnaire d'effet d'écriture
    public List<Question> questions; // Liste des questions du quiz
    public string selectedProfessor; // Nom du professeur sélectionné
    public Image fadeImage; // Image utilisée pour le fondu au noir
    public GameObject sceneParent; // Référence à l'objet parent de la scène

    /// <summary>
    /// Structure pour représenter une question du quiz.
    /// </summary>
    public struct Question
    {
        public string Text; // Texte de la question
        public List<string> Answers; // Liste des réponses possibles
        public string CorrectAnswer; // Réponse correcte

        // Constructeur de la structure Question
        public Question(string text, List<string> answers, string correctAnswer)
        {
            Text = text;
            Answers = answers;
            CorrectAnswer = correctAnswer;
        }
    }

    // Méthode appelée à chaque frame pour mettre à jour l'interactivité des boutons pendant l'effet d'écriture
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

    // Méthode appelée au démarrage du jeu
    void Awake()
    {
        // Initialisation des références aux composants de l'interface utilisateur et chargement des questions depuis la base de données
        txtQuestion = GameObject.Find("txtQuestion").GetComponent<TextMeshProUGUI>();
        btnReponseG = GameObject.Find("ReponseG").GetComponent<Button>();
        btnReponseD = GameObject.Find("ReponseD").GetComponent<Button>();

        // Vérification si les composants sont attachés au script
        if (txtQuestion == null || btnReponseG == null || btnReponseD == null)
        {
            Debug.LogError("Un ou plusieurs composants d'interface utilisateur ne sont pas attachés au script.");
        }

        // Construction de la chaîne de connexion à la base de données
        string databasePath = Path.Combine(Application.streamingAssetsPath, "DB_Unity.db");
        conn = "URI=file:" + databasePath;

        // Chargement des questions depuis la base de données
        LoadQuestionsFromDatabase();
    }

    // Méthode appelée au démarrage du jeu après le Awake
    void Start()
    {
        // Initialisation des coeurs comme invisibles et pose de la première question
        foreach (var heart in heartImages)
        {
            heart.enabled = false;
        }
        PoseUneQuestion();
    }

    /// <summary>
    /// Contrôleur pour les boutons de réponse.
    /// </summary>
    public class ButtonController : MonoBehaviour
    {
        private Quiz quiz;

        // Méthode appelée au démarrage pour configurer le contrôleur de bouton
        void Start()
        {
            // Recherche de l'instance du Quiz dans la scène
            quiz = FindObjectOfType<Quiz>();

            // Vérification si la référence à l'instance de Quiz est nulle
            if (quiz == null)
            {
                Debug.LogError("La référence à l'instance de Quiz est nulle.");
                return;
            }

            // Ajout d'un écouteur d'événement pour détecter le clic sur le bouton
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        // Méthode appelée lorsqu'un bouton est cliqué
        void OnClick()
        {
            // Vérification si l'instance de Quiz existe, puis appelle la méthode CheckAnswer avec le texte de la réponse
            if (quiz != null)
            {
                quiz.CheckAnswer(GetComponentInChildren<TextMeshProUGUI>().text);
            }
        }
    }

    // Méthode pour charger les questions depuis la base de données
    void LoadQuestionsFromDatabase()
    {
        // Initialisation de la liste de questions
        questions = new List<Question>();

        // Connexion à la base de données SQLite
        using (var dbConnection = new SqliteConnection(conn))
        {
            dbConnection.Open();

            // Commande SQL pour sélectionner les questions du professeur spécifié
            using (var dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = "SELECT * FROM QUESTIONS WHERE Professeur = @Professeur";
                dbCmd.Parameters.AddWithValue("@Professeur", selectedProfessor);

                // Lecture des résultats de la requête
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Récupération des informations de la question
                        int questionID = reader.GetInt32(0);
                        string questionText = reader.GetString(1);

                        List<string> answers = new List<string>();
                        string correctAnswer = "";

                        // Commande SQL pour sélectionner les réponses associées à la question
                        using (var answerCmd = dbConnection.CreateCommand())
                        {
                            answerCmd.CommandText = "SELECT * FROM ANSWERS WHERE ParentID = @ParentID";
                            answerCmd.Parameters.AddWithValue("@ParentID", questionID);

                            // Lecture des réponses associées à la question
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

                        // Ajout de la question à la liste
                        questions.Add(new Question(questionText, answers, correctAnswer));
                    }
                }
            }
            dbConnection.Close();
        }
    }

    // Méthode pour poser une nouvelle question
    void PoseUneQuestion()
    {
        // Vérification si toutes les questions ont été posées
        if (questions.Count == 0)
        {
            Debug.Log("Victoire sévère!");
            // Ici, vous pouvez gérer la logique de fin de jeu ou de réinitialisation
        }

        // Sélection aléatoire d'une question et suppression de la question de la liste
        int randomIndex = Random.Range(0, questions.Count);
        var currentQuestion = questions[randomIndex];
        questions.RemoveAt(randomIndex); // Retire la question utilisée de la liste

        // Mise à jour de la réponse correcte actuelle
        Reponse = currentQuestion.CorrectAnswer;

        // Affichage de la question et des réponses sur les boutons
        typeWriter.SetText(currentQuestion.Text); // Affiche la question
        btnReponseG.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Answers[0]; // Affiche la première réponse
        btnReponseD.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Answers[1]; // Affiche la deuxième réponse
    }

    // Méthode appelée lorsqu'un bouton de réponse est cliqué
    public void OnButtonClicked(Button buttonClicked)
    {
        string buttonText = buttonClicked.GetComponentInChildren<TextMeshProUGUI>().text;
        CheckAnswer(buttonText);
    }

    // Méthode pour mettre à jour la barre de santé du boss
    private void UpdateBossHealthBar()
    {
        float healthPercentage = (float)bossHealth / bossMaxHealth;
        bossHealthBar.SetProgress(healthPercentage);
        Debug.Log("Boss Health: " + healthPercentage * 100 + "%");
    }

    // Méthode appelée pour vérifier la réponse donnée par le joueur
    public void CheckAnswer(string selectedAnswer)
    {
        if (selectedAnswer == Reponse)
        {
            Debug.Log("Correct !");
            bossHealth -= 10;
            UpdateBossHealthBar();
            Debug.Log($"Santé du boss restante : {bossHealth}");

            // Vérification si la santé du boss est épuisée
            if (bossHealth <= 0)
            {
                // Incrémentation de l'étape de quête globale et chargement de la scène de victoire ou des crédits
                GlobalQuest.QuestStep += 1;

                if (SceneManager.GetActiveScene().name == "Boss Makssoud")
                {
                    SceneManager.LoadScene("Credits");
                }
                else
                {
                    SceneManager.LoadScene(winSceneName);
                }

                // Ajout d'une clé à l'inventaire du joueur avec une couleur aléatoire
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
                else
                {
                    ElementalInventory.Instance.addItem("wsh", 1, randomColor, "Il faut quitter le jeu par pitié");
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

        // Poser une nouvelle question après avoir traité la réponse du joueur
        PoseUneQuestion();
    }

    // Coroutine pour gérer la perte de vie du joueur
    IEnumerator LoseLifeRoutine()
    {
        // Affichage des coeurs pendant un court laps de temps
        foreach (var heart in heartImages)
        {
            heart.enabled = true;
        }

        // Effet de secousse de l'écran
        StartCoroutine(ScreenShake());

        // Mise à jour des sprites des coeurs
        UpdateHeartSprites();

        yield return new WaitForSeconds(1f);

        // Vérification si le joueur a épuisé toutes ses vies
        if (playerLives <= 0)
        {
            // Activation du fondu au noir et chargement de la scène de défaite
            fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeToBlackAndLoadScene());
        }
        else
        {
            // Cacher les coeurs si le joueur a encore des vies
            foreach (var heart in heartImages)
            {
                heart.enabled = false;
            }
        }
    }

    // Méthode pour mettre à jour les sprites des coeurs dans l'UI
    void UpdateHeartSprites()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < playerLives ? fullHeartSprite : emptyHeartSprite;
        }
    }

    // Coroutine pour l'effet de secousse de l'écran
    private IEnumerator ScreenShake()
    {
        float duration = 0.5f; // Durée de l'effet de secousse
        float magnitude = 0.8f; // Amplitude de la secousse

        Vector3 originalPosition = sceneParent.transform.localPosition;
        float elapsed = 0.0f;

        // Appliquer la secousse pendant la durée spécifiée
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            sceneParent.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Rétablir la position originale de l'écran
        sceneParent.transform.localPosition = originalPosition;
    }

    // Coroutine pour le fondu au noir et le chargement de la scène de défaite
    private IEnumerator FadeToBlackAndLoadScene()
    {
        Debug.Log("Début de la coroutine de fondu au noir");
        Debug.Log("Image de fondu activée");

        float fadeDuration = 0.8f;
        float fadeStep = 0.2f;

        // Progression progressive du fondu au noir
        for (float i = 0; i <= 1; i += fadeStep)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            Debug.Log("Fondu au noir progressif : Alpha = " + i);
            yield return new WaitForSeconds(fadeDuration * fadeStep);
        }

        Debug.Log("Fondu au noir terminé");

        // Attente avant de charger la scène de défaite
        yield return new WaitForSeconds(1f);
        Debug.Log("Chargement de la nouvelle scène : " + loseSceneName);

        // Chargement de la scène de défaite
        SceneManager.LoadScene(loseSceneName);
    }
}
