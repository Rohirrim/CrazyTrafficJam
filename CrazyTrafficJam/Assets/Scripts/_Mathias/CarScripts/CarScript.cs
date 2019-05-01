using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
    public class CarScript : MonoBehaviour
    {
        public districtType myDestinationType;
        public Transform myDestinationDistrict;
        public float speed;
        [Range(0.1f, 1f)]
        public float raycastRange = 0.1f;

        public void GiveMeMyDestination(districtType newType, Transform newDistrict)
        {
            myDestinationType = newType;
            myDestinationDistrict = newDistrict;
        }
    }
}