using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictorySceneScript : MonoBehaviour
{
    public DialogTrigger victoryDialog;

    void Start()
    {
        if (GameController.VictoryAchieved) // GameController.VictoryAchieved est un indicateur de victoire
        {
            victoryDialog.TriggerDialog();
            GameController.VictoryAchieved = false; // R�initialisez l'indicateur pour les prochaines parties
        }
    }
}
