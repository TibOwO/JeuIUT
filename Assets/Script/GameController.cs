using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public static bool VictoryAchieved { get; set; }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garde le GameController actif entre les scènes
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

}

