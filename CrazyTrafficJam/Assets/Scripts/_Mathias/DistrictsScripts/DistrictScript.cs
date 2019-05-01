using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IronSideStudio.CrazyTrafficJam
{
    public enum districtType
    {
        RESIDENTIAL = 1 << 0,
        TOURISTIC = 1 << 1,
        SHOPPING = 1 << 2,
        BUSINESS = 1 << 3,

    }

    [Serializable]
    public struct districtAttributes
    {
        public bool isOpened;
        public carSpawnRate spawnRate;

    }

    [Serializable]
    public struct carSpawnRate
    {
        [Range(0, 10)]
        public float minCarSpawnRate, maxCarSpawnRate;
    }

    public class DistrictScript : MonoBehaviour
    {
        [Header("Variables propres à chaque district")]
        public districtType myType;
        public districtAttributes myAttributes = new districtAttributes();

        [Header("Pas touche !")]
        [SerializeField]
        private int spawnTimer = 0;

        public void CreateNewCar()
        {
            if (myAttributes.isOpened)
            {
                int RandCar = UnityEngine.Random.Range(0, 4); //Car il y a 3 types de voiture pour le moment

                switch (RandCar)
                {
                    case 0:
                        GameObject Go0 = Instantiate(IronSideStudio.CrazyTrafficJam.CarsManager.Instance.allCarTypes.ResidentialCar, this.transform.position, Quaternion.identity) as GameObject;
                        if (myType == districtType.RESIDENTIAL)//On ajoute une condition pour pas qu'il indique un type de voiture identique au type du district
                        {
                            GameObject newGo = Instantiate(IronSideStudio.CrazyTrafficJam.CarsManager.Instance.allCarTypes.TouristicCar, this.transform.position, Quaternion.identity) as GameObject;
                        }
                        break;
                    case 1:
                        GameObject Go1 = Instantiate(IronSideStudio.CrazyTrafficJam.CarsManager.Instance.allCarTypes.TouristicCar, this.transform.position, Quaternion.identity) as GameObject;
                        if (myType == districtType.TOURISTIC)//On ajoute une condition pour pas qu'il indique un type de voiture identique au type du district
                        {
                            GameObject newGo = Instantiate(IronSideStudio.CrazyTrafficJam.CarsManager.Instance.allCarTypes.ShoppingCar, this.transform.position, Quaternion.identity) as GameObject;
                        }
                        break;
                    case 2:
                        GameObject Go2 = Instantiate(IronSideStudio.CrazyTrafficJam.CarsManager.Instance.allCarTypes.ShoppingCar, this.transform.position, Quaternion.identity) as GameObject;
                        if (myType == districtType.SHOPPING)//On ajoute une condition pour pas qu'il indique un type de voiture identique au type du district
                        {
                            GameObject newGo = Instantiate(IronSideStudio.CrazyTrafficJam.CarsManager.Instance.allCarTypes.BusinessCar, this.transform.position, Quaternion.identity) as GameObject;
                        }
                        break;
                    case 3:
                        GameObject Go3 = Instantiate(IronSideStudio.CrazyTrafficJam.CarsManager.Instance.allCarTypes.BusinessCar, this.transform.position, Quaternion.identity) as GameObject;
                        if (myType == districtType.BUSINESS)//On ajoute une condition pour pas qu'il indique un type de voiture identique au type du district
                        {
                            GameObject newGo0 = Instantiate(IronSideStudio.CrazyTrafficJam.CarsManager.Instance.allCarTypes.ResidentialCar, this.transform.position, Quaternion.identity) as GameObject;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
