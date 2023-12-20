using UnityEngine;

public class CinematicBars : MonoBehaviour
{
    public Animator topBarAnimator;
    public Animator bottomBarAnimator;

    // Utilisez cette méthode pour activer l'effet cinématique
    public void ShowBars()
    {
        topBarAnimator.SetTrigger("Enter");
        bottomBarAnimator.SetTrigger("Enter");
    }

    // Utilisez cette méthode pour désactiver l'effet cinématique
    public void HideBars()
    {
        topBarAnimator.SetTrigger("Exit");
        bottomBarAnimator.SetTrigger("Exit");
    }
}
