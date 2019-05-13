using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace IronSideStudio.CrazyTrafficJam
{
    public class VictoryConditions : MonoBehaviour
    {
        public static VictoryConditions Instance;

        [Header("A modifier")]
        [Range(1, 100)]
        public int defeatPercentage = 30;
        public float debugPercentage;
        [Range(10, 100)]
        public int minimumCarsNumber = 10;

        [HideInInspector]
        public List<IronSideStudio.CrazyTrafficJam.Car.Driver> allCars;

        [Header("Pas touche")]
        public GameObject victoryPannel;
        public GameObject defeatPannel;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //victoryPannel.SetActive(false);
            defeatPannel.SetActive(false);

            if(LevelManager.Instance != null)
            {
                defeatPercentage = (int)LevelManager.Instance.levelSelected.defeatPercentage;
                minimumCarsNumber = LevelManager.Instance.levelSelected.minimumCarsNumber;
            }
        }

        private void LateUpdate()
        {
            CheckDefeat();
            debugPercentage = percentageOfStoppedDriver();
        }

        private void CheckDefeat()
        {
            if(allCars.Count < minimumCarsNumber)
            {
                return;
            }
            else
            {
                if (percentageOfStoppedDriver() >= defeatPercentage)
                {
                    GameOver();
                }
            }
        }

        public float allStoppedDrivers()
        {
            float numberOfStoppedDrivers = 0;

            if (allCars.Count > 0)
            {
                for (int i = 0; i < allCars.Count; i++)
                {
                    if (allCars[i].isStopped)
                    {
                        numberOfStoppedDrivers += 1;
                    }
                }
            }

            return numberOfStoppedDrivers;
        }

        public float percentageOfStoppedDriver()
        {
            if (allStoppedDrivers() > 0)
            {
                float percentage = allStoppedDrivers() / allCars.Count;

                return percentage * 100;
            }
            else
            {
                return -1;
            }
        }

        public void GameOver()
        {
            TimeManager.Instance.SetTimeScale(0);
            defeatPannel.SetActive(true);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
