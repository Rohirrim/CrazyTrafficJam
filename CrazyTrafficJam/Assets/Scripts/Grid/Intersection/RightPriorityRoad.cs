using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronSideStudio.CrazyTrafficJam.Car;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
    public class RightPriorityRoad : SimpleRoad, IUpdatable
    {
        protected HashSet<SDriverIn> driversIn;

        public bool Enable => driversIn.Count > 0;

        private void Awake()
        {
            driversIn = new HashSet<SDriverIn>();
        }

        public void MUpdate()
        {
        }

        public override bool CanDrive(Car.Driver driver)
        {
            Vector3 driverRight = -driver.transform.right;
            Vector3 startRaycast = transform.position + driver.transform.forward * Constante.Gameplay.roadSpace;
            startRaycast.y = driver.transform.position.y;

            if (Physics.Raycast(startRaycast, driverRight, Constante.Gameplay.securityDistance * 10f, LayerMask.GetMask(Constante.Layer.Car)))
            {
                SDriverIn sDriver = new SDriverIn
                {
                    driver = driver,
                    time = Time.time
                };
                driversIn.Add(sDriver);
                return false;
            }
            driversIn.RemoveWhere(d => d.driver == driver);
            return true;
        }

        /*public override bool CanDrive(Driver driver)
        {
            Vector3 driverForward = driver.transform.forward;
            Vector3 startRaycast = transform.position + driver.transform.forward * Constante.Gameplay.roadSpace;
            startRaycast.y = driver.transform.position.y;

            if (Physics.Raycast(startRaycast, driverForward, Constante.Gameplay.securityDistance * 10f, LayerMask.GetMask(Constante.Layer.Car)))
            {
                SDriverIn sDriver = new SDriverIn
                {
                    driver = driver,
                    time = Time.time
                };
                driversIn.Add(sDriver);
                return false;
            }
            driversIn.RemoveWhere(d => d.driver == driver);
            return true;
        }*/
    }
}
