using UnityEngine;
using TMPro; // Si vous utilisez TextMeshPro

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText; // Si vous utilisez TextMeshPro, sinon utilisez Text
    private string currentQuest;

    void Start()
    {
        // Initialisation de la qu�te
        SetQuest("Rejoignez la premi�re salle.");
    }

    public void SetQuest(string newQuest)
    {
        currentQuest = newQuest;
        questText.text = "Qu�te en cours: " + currentQuest;
    }
}
