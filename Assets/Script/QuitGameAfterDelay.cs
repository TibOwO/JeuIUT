using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor; // Nécessaire pour accéder à EditorApplication.ExitPlaymode
#endif

public class QuitGameAfterDelay : MonoBehaviour
{
    public float delay = 10f; // Délai en secondes avant de quitter le jeu

    void Start()
    {
        StartCoroutine(QuitAfterDelay());
    }

    IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSeconds(delay);

#if UNITY_EDITOR
        // Quitter le mode Play dans l'éditeur Unity
        EditorApplication.ExitPlaymode();
#else
        // Quitter le jeu lorsqu'il est exécuté en dehors de l'éditeur
        Application.Quit();
#endif
    }
}