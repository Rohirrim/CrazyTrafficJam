using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IronSideStudio.CrazyTrafficJam
{
    [Serializable]
    public struct trafficByHours
    {
        public DayTime periodOfDay;
        [Range(0, 100)]
        public float density;
        [Range(0, 10)]
        public float timerForSpawn;
        public destinationProba PriorityDistrict, secondDistrict, thirdDistrict, fourthDistrict;

    }

    [Serializable]
    public struct destinationProba
    {
        public districtType typeOfDistrict;
        [Range(0,100)]
        public float probability;
    }

    public enum districtType
    {
        RESIDENTIAL,
        TOURISTIC,
        SHOPPING,
        BUSINESS,

    }

    [Serializable]
    public struct districtAttributes//OLD
    {
        public bool isOpened;
        public carSpawnRate spawnRate;

    }

    [Serializable]
    public struct carSpawnRate//OLD
    {
        [Range(0, 10)]
        public float minCarSpawnRate, maxCarSpawnRate;
    }

    public class DistrictScript : MonoBehaviour
    {
        [Header("Variables propres au district")]
        public districtType myType;
        [SerializeField]
        private trafficByHours myMorningTraffic, myAfternoonTraffic, myEveningTraffic, myNightTraffic;

        [Header("Pas touche")]
        public trafficByHours currentTraffic = new trafficByHours();
        public Car.Spawner carSpawner;
        float timer;

        private void Start()
        {
            IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Add(this);
        }

        void Update()
        {
            if (currentTraffic.timerForSpawn > 0)
            {
                timer += Time.deltaTime;

                if (timer >= CurrentTimerForSpawn())
                {
                    SpawnCar();
                    timer = 0;
                }
            }
        }

        public void SpawnCar()
        {
            int randDistrict = UnityEngine.Random.Range(0, 100);

            if(InstantiatedCarDestination(randDistrict) != null)
            carSpawner.Spawn(myNode(), InstantiatedCarDestination(randDistrict));
        }

        public IronSideStudio.CrazyTrafficJam.Grid.Node myNode()
        {
            return transform.parent.GetComponent<IronSideStudio.CrazyTrafficJam.Grid.Node>();
        }

        public void SetDistrictTraffic(DayTime currentHours)
        {
            switch (currentHours)
            {
                case DayTime.MATIN:
                    currentTraffic = myMorningTraffic;
                    return;
                case DayTime.APRESMIDI:
                    currentTraffic = myAfternoonTraffic;
                    return;
                case DayTime.SOIREE:
                    currentTraffic = myEveningTraffic;
                    return;
                case DayTime.NUIT:
                    currentTraffic = myNightTraffic;
                    return;
                default:
                    Debug.LogError("L'heure n'est pas reconnue");
                    return;
            }
        }

        public float CurrentTimerForSpawn()
        {
            float Ctimer = (currentTraffic.timerForSpawn * currentTraffic.density) / 100;

            return Ctimer;
        }

        public Grid.Node InstantiatedCarDestination(int randDistrict)
        {
            Grid.Node destinationNode = new Grid.Node();
            List<Grid.Node> potentialDestinations = new List<Grid.Node>();

            if(randDistrict <= currentTraffic.PriorityDistrict.probability)//Si c'est inférieur à la proba
            {
                int rand = UnityEngine.Random.Range(0, IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Count);
                
                for(int i = 0; i < IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Count; i++)
                {
                    if(IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts[i].myType == currentTraffic.PriorityDistrict.typeOfDistrict)
                    {
                        potentialDestinations.Add(IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts[i].transform.parent.GetComponent<IronSideStudio.CrazyTrafficJam.Grid.Node>());//On créé une liste des districts correspondant aux districts de bons types
                    }
                }
            }
            else if(randDistrict > currentTraffic.PriorityDistrict.probability && randDistrict <= currentTraffic.secondDistrict.probability)
            {
                int rand = UnityEngine.Random.Range(0, IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Count);

                for (int i = 0; i < IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Count; i++)
                {
                    if (IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts[i].myType == currentTraffic.secondDistrict.typeOfDistrict)
                    {
                        potentialDestinations.Add(IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts[i].transform.parent.GetComponent<IronSideStudio.CrazyTrafficJam.Grid.Node>());
                    }
                }
            }
            else if(randDistrict <= currentTraffic.thirdDistrict.probability)
            {
                int rand = UnityEngine.Random.Range(0, IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Count);

                for (int i = 0; i < IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Count; i++)
                {
                    if (IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts[i].myType == currentTraffic.thirdDistrict.typeOfDistrict)
                    {
                        potentialDestinations.Add(IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts[i].transform.parent.GetComponent<IronSideStudio.CrazyTrafficJam.Grid.Node>());
                    }
                }
            }
            else if (randDistrict <= currentTraffic.fourthDistrict.probability)
            {
                int rand = UnityEngine.Random.Range(0, IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Count);

                for (int i = 0; i < IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts.Count; i++)
                {
                    if (IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts[i].myType == currentTraffic.fourthDistrict.typeOfDistrict)
                    {
                        potentialDestinations.Add(IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.allDistricts[i].transform.parent.GetComponent<IronSideStudio.CrazyTrafficJam.Grid.Node>());
                    }
                }
            }

            if(potentialDestinations.Count > 0)
            {
                int randD = UnityEngine.Random.Range(0, potentialDestinations.Count);

                while (potentialDestinations[randD] == myNode())//Pour être sûr qu'on aille pas au même endroit
                {
                    randD = UnityEngine.Random.Range(0, potentialDestinations.Count);
                }

                destinationNode = potentialDestinations[randD];
            }

            return destinationNode;
        }
    }
}
