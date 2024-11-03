using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MenuManagement.Level;
using UnityEngine.UI;

namespace MenuManagement
{
    [RequireComponent(typeof(MissionSelector))]
    public class LevelSelectMenu : Menu<LevelSelectMenu>
    {
        //[SerializeField] protected TMP_Text nameText;
        //[SerializeField] protected TMP_Text descriptionText;
        //[SerializeField] protected Image image;
        [SerializeField] protected float playDelay = 0.5f;
        [SerializeField] protected TransitionFader playTransitionPrefab;
        
        protected MissionSelector missionSelector;
        protected MissionSpecs currentMission;

        protected override void Awake()
        {
            base.Awake();
            missionSelector = GetComponent<MissionSelector>();
            if (missionSelector == null)
            {
                Debug.LogError("MissionSelector component not found on LevelSelectMenu GameObject.");
            }
        }

        private void Start()
        {
            UpdateInfo();
        }

        private void OnEnable()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            if (missionSelector == null)
            {
                Debug.LogError("MissionSelector is null in LevelSelectMenu.");
                return;
            }

            currentMission = missionSelector.GetCurrentMission();
            
            if (currentMission == null)
            {
                Debug.LogWarning("Current mission is null in LevelSelectMenu.");
            }

            /*if (nameText != null)
            {
                nameText.text = currentMission?.Name ?? "No Mission Selected";
            }
            else
            {
                Debug.LogWarning("nameText is null in LevelSelectMenu.");
            }

            if (descriptionText != null)
            {
                descriptionText.text = currentMission?.Description ?? "No description available.";
            }
            else
            {
                Debug.LogWarning("descriptionText is null in LevelSelectMenu.");
            }

            if (image != null)
            {
                image.sprite = currentMission?.Image;
                image.color = currentMission?.Image != null ? Color.white : Color.clear;
            }
            else
            {
                Debug.LogWarning("image is null in LevelSelectMenu.");
            }*/
        }

        public void OnNextPressed()
        {
            if (missionSelector != null)
            {
                missionSelector.IncrementIndex();
                UpdateInfo();
            }
            else
            {
                Debug.LogError("Cannot increment index: missionSelector is null in LevelSelectMenu.");
            }
        }

        public void OnPreviousPressed()
        {
            if (missionSelector != null)
            {
                missionSelector.DecrementIndex();
                UpdateInfo();
            }
            else
            {
                Debug.LogError("Cannot decrement index: missionSelector is null in LevelSelectMenu.");
            }
        }

        public void OnPlayPressed()
        {
            if (currentMission != null)
            {
                StartCoroutine(OnPlayMissionRoutine(currentMission.SceneName));
            }
            else
            {
                Debug.LogWarning("Cannot play: No mission selected in LevelSelectMenu.");
            }
        }

        private IEnumerator OnPlayMissionRoutine(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Cannot load level: Scene name is null or empty.");
                yield break;
            }

            if (playTransitionPrefab != null)
            {
                TransitionFader.PlayTransition(playTransitionPrefab);
            }
            else
            {
                Debug.LogWarning("playTransitionPrefab is null in LevelSelectMenu.");
            }
           
            LevelLoader.LoadLevel(sceneName);
            yield return new WaitForSeconds(playDelay);
            GameMenu.Open();    
        }
    }
}