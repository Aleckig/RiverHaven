using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MenuManagement.Level;
using UnityEngine.UI;


namespace  MenuManagement
{
    [RequireComponent(typeof(MissionSelector))]
    public class LevelSelectMenu : Menu<LevelSelectMenu>
    {
        [SerializeField] protected TMP_Text nameText;
        [SerializeField] protected TMP_Text descriptionText;
        [SerializeField] protected Image image;

        [SerializeField] protected float playDelay = 0.5f;
        [SerializeField] protected TransitionFader playTransitionPrefab;


        protected MissionSelector missionSelector;
        protected MissionSpecs currentMission;

        protected override void Awake()
        {
            base.Awake();
            missionSelector = GetComponent<MissionSelector>();
            UpdateInfo();
        }

        private void OnEnable()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            currentMission = missionSelector.GetCurrentMission();

            
            nameText.text = currentMission?.Name;
            descriptionText.text = currentMission?.Description;
            image.sprite = currentMission?.Image;
            image.color = Color.white;

        }

        public void OnNextPressed()
        {
            missionSelector.IncrementIndex();
            UpdateInfo();
        }

        public void OnPreviousPressed()
        {
            missionSelector.DecrementIndex();
            UpdateInfo();
        }

        public void OnPlayPressed()
        {
            StartCoroutine(OnPlayMissionRoutine(currentMission?.SceneName));
        }

        private IEnumerator OnPlayMissionRoutine(string sceneName)
        {
            TransitionFader.PlayTransition(playTransitionPrefab);
            
            LevelLoader.LoadLevel(sceneName);

            yield return new WaitForSeconds(playDelay);

            GameMenu.Open();    
        }
       
       





        
    }


}
