using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    [RequireComponent(typeof(ScreenFader))]
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private ScreenFader screenFader;

        [SerializeField] private float delay = 1f;

        private void Awake()
        {
            screenFader = GetComponent<ScreenFader>();
        }

        private void Start()
        {
            screenFader.FadeOn();
        }

        public void FadeAndLoad()
        {
            StartCoroutine(FadeAndLoadRoutine());
        }

        private IEnumerator FadeAndLoadRoutine()
        {
            
            yield return new WaitForSeconds(delay);
            screenFader.FadeOff();
            LevelLoader.LoadMainMenuLevel();

            yield return new WaitForSeconds(screenFader.FadeOnDuration);

            Object.Destroy(gameObject);
        }
    }
}