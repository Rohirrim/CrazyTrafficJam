using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IronSideStudio.CrazyTrafficJam
{
    [Serializable]
    public struct trafficByHours
    {
        public DayTime hours;
        [Range(0, 100)]
        public float density;
        public destinationProba probabilities;

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



    }
}
