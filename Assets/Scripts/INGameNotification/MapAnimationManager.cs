using UnityEngine;
namespace Game
{
    public class MapAnimationManager : MonoBehaviour
    {
        public static MapAnimationManager Singleton { get; private set; }

        /// <summary>
        /// Serve a nascondere la mappa ma a lasciarla accesa
        /// </summary>
        public Canvas mapCanvas;

        private void Awake()
        {
            Singleton = this;
        }
        public static bool mapsAnimationIsFinish;

        public Animator currentAnimator;

        public void TurnOnMapAnimation(ScriptablePage.Title title)
        {
            SetAnimatorAtTheEndOfChapter(title);
            mapsAnimationIsFinish = false;
            mapCanvas.enabled = true;
        }

        public void ResetDefaultSettingsAtTheEndOfMapAnimation()
        {
            mapsAnimationIsFinish = false;
            mapCanvas.enabled = false;
        }

        public void SetAnimatorAtTheEndOfChapter(ScriptablePage.Title title)
        {
            switch (title)
            {
                case ScriptablePage.Title.AllaSalute:
                    currentAnimator.Play("AllaSaluteMapAnimation", 0, 0f);
                    break;
                case ScriptablePage.Title.Scappa:
                    currentAnimator.Play("ScappaMapAnimation", 0, 0f);
                    break;
                case ScriptablePage.Title.InVinoVeritas:
                    currentAnimator.Play("InVinoVeritasMapAnimation", 0, 0f);
                    break;
                default:
                    break;
            }
        }

    }
}

