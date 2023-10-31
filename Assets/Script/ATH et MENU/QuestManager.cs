using UnityEngine;
using TMPro;
using UnityEngine.UI; // N�cessaire pour acc�der au composant CanvasGroup
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public CanvasGroup questCanvasGroup; // Pour contr�ler l'opacit� de tout le groupe
    private string currentQuest;
    public float displayTime = 5.0f; // Temps pendant lequel le texte sera affich� avant de commencer � dispara�tre
    public float fadeDuration = 2.0f; // Dur�e de la disparition progressive

    void Start()
    {
        // Initialisation de la qu�te
        SetQuest("Parlez a (Trouver un nom sympa pour ce pnj).");
    }

    public void SetQuest(string newQuest)
    {
        currentQuest = newQuest;
        questText.text = "Qu�te en cours: " + currentQuest;

        // Afficher le texte et d�marrer la routine de disparition
        StopAllCoroutines(); // Arr�ter toutes les coroutines en cours pour �viter les conflits
        questCanvasGroup.alpha = 1; // Assurez-vous que le texte est enti�rement visible
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        // Attendre un certain temps avant de commencer � faire dispara�tre le texte
        yield return new WaitForSeconds(displayTime);

        float fadeSpeed = 1 / fadeDuration;
        while (questCanvasGroup.alpha > 0)
        {
            questCanvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
