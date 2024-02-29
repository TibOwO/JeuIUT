using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor; // N�cessaire pour acc�der � EditorApplication.ExitPlaymode
#endif

public class QuitGameAfterDelay : MonoBehaviour
{
    public float delay = 10f; // D�lai en secondes avant de quitter le jeu

    void Start()
    {
        StartCoroutine(QuitAfterDelay());
    }

    IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSeconds(delay);

#if UNITY_EDITOR
        // Quitter le mode Play dans l'�diteur Unity
        EditorApplication.ExitPlaymode();
#else
        // Quitter le jeu lorsqu'il est ex�cut� en dehors de l'�diteur
        Application.Quit();
#endif
    }
}