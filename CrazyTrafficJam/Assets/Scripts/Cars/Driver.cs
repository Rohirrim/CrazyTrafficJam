using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	public class Driver : MonoBehaviour, IUpdatable
	{
		[SerializeField]
		private float speed;
		private Vector3 direction;

		private float timer;
		public bool Enable => gameObject.activeInHierarchy;
        public bool isStopped = false;

		[SerializeField]
		private Transform frontCar;
        [SerializeField]
		private Tween pathTween;

        [SerializeField]
        private Grid.Node[] nodePath;
        [SerializeField]
        private int indexNodePath;

        [SerializeField]
        private Grid.Node node;
        [SerializeField]
        private Grid.Node previousNode;

		public void MUpdate()
		{
			Drive();
			Patience();
            SetSpeed();
		}

		private void Drive()
		{
			if (!SecurityDistance())
				return;
			if (node == null || !MoveOnIntersection())
				return;
			timer = 0f;
		}

		private void Patience()
		{
			if (pathTween.IsPlaying())
				return;
            isStopped = true;
			timer += Time.deltaTime;
			if (timer > 2.5f)
			{
				//gameObject.SetActive(false);
				node = null;
				pathTween.Play();
                isStopped = false;
			}
		}

        public void SetSpeed()
        {
            if(currentRoad() != null)
            {
                speed = currentRoad().speedLimit;
            }
            else
            {
                return;
            }
        }

        private IronSideStudio.CrazyTrafficJam.Grid.SimpleRoad currentRoad()
        {
            if(nodePath.Length > 0)
            {
                if (indexNodePath < nodePath.Length)
                {
                    if (nodePath[indexNodePath].transform.GetChild(0).GetComponent<IronSideStudio.CrazyTrafficJam.Grid.SimpleRoad>() != null)
                    {
                        return nodePath[indexNodePath].transform.GetChild(0).GetComponent<IronSideStudio.CrazyTrafficJam.Grid.SimpleRoad>();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private bool SecurityDistance()
		{
			if (!Physics.Raycast(frontCar.position, frontCar.forward, Constante.Gameplay.securityDistance, LayerMask.GetMask(Constante.Layer.Car)))
			{
				if (!pathTween.IsPlaying())
					pathTween.Play();
			}
			else if (pathTween.IsPlaying())
				pathTween.Pause();

			return pathTween.IsPlaying();
		}

		private bool MoveOnIntersection()
		{
			if (node.CanDrive(this))
			{
				if (!pathTween.IsPlaying())
					pathTween.Play();
				node = null;
			}
			else if (pathTween.IsPlaying())
				pathTween.Pause();
			return pathTween.IsPlaying();
		}

		public void SetPath(Grid.Node[] path)
		{
			nodePath = path;
			indexNodePath = 1;

			Vector3 nextDirection = (path[1].GetPosition() + Vector3.up * .51f - transform.position).normalized;
			transform.position += Quaternion.Euler(0f, 90f, 0f) * nextDirection * Constante.Gameplay.roadSpace;

			NextNode();
		}

		private void NextNode()
		{
			if (indexNodePath == nodePath.Length)
			{
				gameObject.SetActive(false);
				return;
			}

			Vector3 previousDirection = nodePath[indexNodePath - 1].GetPosition();
			Vector3 nextDirection = indexNodePath + 1 < nodePath.Length ? nodePath[indexNodePath + 1].GetPosition() : transform.position;
			Vector3[] waypoints = nodePath[indexNodePath].GetWaypoint(previousDirection, nextDirection);

			node = nodePath[indexNodePath++];
			if (waypoints == null)
			{
				Debug.LogWarning("No Path on Node : " + nodePath[indexNodePath - 1]);
				return;
			}
			TweenParams tParam = new TweenParams().SetSpeedBased(true).SetEase(Ease.Linear);
			tParam.OnComplete(NextNode);
			pathTween = transform.DOPath(waypoints, speed, PathType.CatmullRom).SetLookAt(0f).SetAs(tParam);
		}
	}
}