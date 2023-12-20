using UnityEngine;

public class CinematicBars : MonoBehaviour
{
    public Animator topBarAnimator;
    public Animator bottomBarAnimator;

    // Utilisez cette m�thode pour activer l'effet cin�matique
    public void ShowBars()
    {
        topBarAnimator.SetTrigger("Enter");
        bottomBarAnimator.SetTrigger("Enter");
    }

    // Utilisez cette m�thode pour d�sactiver l'effet cin�matique
    public void HideBars()
    {
        topBarAnimator.SetTrigger("Exit");
        bottomBarAnimator.SetTrigger("Exit");
    }
}
