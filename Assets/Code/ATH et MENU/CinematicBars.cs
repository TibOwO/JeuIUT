using UnityEngine;

public class CinematicBars : MonoBehaviour
{
    public Animator topBarAnimator;
    public Animator bottomBarAnimator;

    public void ShowBars()
    {
        topBarAnimator.SetTrigger("Enter");
        bottomBarAnimator.SetTrigger("Enter");
    }

    public void HideBars()
    {
        topBarAnimator.SetTrigger("Exit");
        bottomBarAnimator.SetTrigger("Exit");
    }
}
