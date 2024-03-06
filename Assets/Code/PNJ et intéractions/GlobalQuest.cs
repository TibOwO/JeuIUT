using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GlobalQuest : MonoBehaviour
{

    public static int QuestStep = 1; // Etape actuelle de la qu�te
    GameObject targetobject;
    Scene currentScene;


    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {


    }
    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        switch (QuestStep)
        {
            case 1: // Apr�s avoir ramass� la cl�
                if (currentScene.name == "Couloir")
                {
                    targetobject = GameObject.Find("Salle 1");
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle 1")
                {
                    targetobject = GameObject.Find("Boss Morancey");
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                break;

            case 2: // Apr�s avoir battu Mr.Morancey
                if (currentScene.name == "Couloir")
                {
                    targetobject = GameObject.Find("Salle " + QuestStep.ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + (QuestStep - 1).ToString())
                {
                    targetobject = GameObject.Find("Porte_Salle" + (QuestStep - 1).ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + QuestStep.ToString())
                {
                    targetobject = GameObject.Find("Boss Casali");
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                break;

            case 3: // Apr�s avoir battu Mr.Casali
                if (currentScene.name == "Couloir")
                {
                    targetobject = GameObject.Find("Salle " + QuestStep.ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + (QuestStep - 1).ToString())
                {
                    targetobject = GameObject.Find("Porte_Salle" + (QuestStep - 1).ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + QuestStep.ToString())
                {
                    targetobject = GameObject.Find("Boss Nevot");
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                break;
            case 4:
                if (currentScene.name == "Couloir")
                {
                    targetobject = GameObject.Find("Salle " + QuestStep.ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + (QuestStep - 1).ToString())
                {
                    targetobject = GameObject.Find("Porte_Salle" + (QuestStep - 1).ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + QuestStep.ToString())
                {
                    targetobject = GameObject.Find("Boss Salou");
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                break;


            case 5:
                if (currentScene.name == "Couloir")
                {
                    targetobject = GameObject.Find("Salle " + QuestStep.ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + (QuestStep - 1).ToString())
                {
                    targetobject = GameObject.Find("Porte_Salle" + (QuestStep - 1).ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + QuestStep.ToString())
                {
                    targetobject = GameObject.Find("Boss Slezak");
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                break;
            case 6:
                if (currentScene.name == "Couloir")
                {
                    targetobject = GameObject.Find("Salle" + QuestStep.ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + (QuestStep - 1).ToString())
                {
                    targetobject = GameObject.Find("Porte_Salle" + (QuestStep - 1).ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + QuestStep.ToString())
                {
                    targetobject = GameObject.Find("Boss Random");
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                break;
            case 7:
                if (currentScene.name == "Couloir")
                {
                    targetobject = GameObject.Find("Salle " + QuestStep.ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + (QuestStep - 1).ToString())
                {
                    targetobject = GameObject.Find("Porte_Salle" + (QuestStep - 1).ToString());
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                else if (currentScene.name == "Salle " + QuestStep.ToString())
                {
                    targetobject = GameObject.Find("Boss Makssoud");
                    ArrowOrbit.ChangeTarget(targetobject.transform);
                }
                break;
            default:
                break;
        }
    }
}
