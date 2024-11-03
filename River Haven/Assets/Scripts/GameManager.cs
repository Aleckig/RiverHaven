using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MenuManagement;

namespace RiverHaven
{   
    public class GameManager : MonoBehaviour
    {

        private bool isGameOver;
        public bool IsGameOver { get { return isGameOver; } }

        private static GameManager instance;
        public static GameManager Instance{ get { return instance; }}

        //[SerializeField] private TransitionsFader endTransitionPrefab;
        //[SerializeField] private float endDelay = 0.5f;
        


        // initialize references
        private void Awake()
        {
            if (instance != null)
            { 
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            
            
        }



        private void OnDestroy() 
        {
           if (instance == this)
           {
             instance = null;
           } 
        }

        
    }

        
}



