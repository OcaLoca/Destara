using UnityEngine;

public class LogoIntroAnimationIsFinish : MonoBehaviour
{
    static bool introFinish;
    private void OnEnable()
    {
        introFinish = false;
    }

    internal void SetIntroAnimationFinish() { introFinish = true; }
    internal static bool IntroAnimationFinish()
    {
        return introFinish;
    }
}
