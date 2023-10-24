using UnityEngine;
using TMPro; // Si vous utilisez TextMeshPro

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText; // Si vous utilisez TextMeshPro, sinon utilisez Text
    private string currentQuest;

    void Start()
    {
        // Initialisation de la quête
        SetQuest("Rejoignez la première salle.");
    }

    public void SetQuest(string newQuest)
    {
        currentQuest = newQuest;
        questText.text = "Quête en cours: " + currentQuest;
    }
}
