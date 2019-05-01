using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IronSideStudio.CrazyTrafficJam
{
    [Serializable]
    public struct carTypes
    {
        public GameObject ResidentialCar;
        public GameObject TouristicCar;
        public GameObject ShoppingCar;
        public GameObject BusinessCar;
    }

    public class CarsManager : MonoBehaviour
    {
        public static CarsManager Instance;

        [Header("Pas touche !")]
        public carTypes allCarTypes = new carTypes();

        private void Awake()
        {
            Instance = this;
        }
    }
}