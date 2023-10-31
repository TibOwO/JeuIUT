using UnityEngine;
using TMPro;
using UnityEngine.UI; // Nécessaire pour accéder au composant CanvasGroup
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public CanvasGroup questCanvasGroup; // Pour contrôler l'opacité de tout le groupe
    private string currentQuest;
    public float displayTime = 5.0f; // Temps pendant lequel le texte sera affiché avant de commencer à disparaître
    public float fadeDuration = 2.0f; // Durée de la disparition progressive

    void Start()
    {
        // Initialisation de la quête
        SetQuest("Parlez a (Trouver un nom sympa pour ce pnj).");
    }

    public void SetQuest(string newQuest)
    {
        currentQuest = newQuest;
        questText.text = "Quête en cours: " + currentQuest;

        // Afficher le texte et démarrer la routine de disparition
        StopAllCoroutines(); // Arrêter toutes les coroutines en cours pour éviter les conflits
        questCanvasGroup.alpha = 1; // Assurez-vous que le texte est entièrement visible
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        // Attendre un certain temps avant de commencer à faire disparaître le texte
        yield return new WaitForSeconds(displayTime);

        float fadeSpeed = 1 / fadeDuration;
        while (questCanvasGroup.alpha > 0)
        {
            questCanvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
