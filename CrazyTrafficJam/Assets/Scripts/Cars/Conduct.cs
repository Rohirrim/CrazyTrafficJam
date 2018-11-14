using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	public class Conduct : MonoBehaviour, IUpdatable
	{
		[SerializeField]
		private float speed;
		private Vector3 direction;

		public bool Enable => gameObject.activeInHierarchy;

		[SerializeField]
		private Transform frontCar;
		private Tween pathTween;

		public void MUpdate()
		{
			if (!Physics.Raycast(frontCar.position, frontCar.forward, 0.1f, LayerMask.GetMask(Constante.Layer.Car)))
			{
				if (!pathTween.IsPlaying())
					pathTween.Play();
			}
			else if (pathTween.IsPlaying())
				pathTween.Pause();
		}

		public void SetPath(Grid.Node[] path)
		{
			List<Vector3> pointPath = new List<Vector3>();
			Vector3 cPosition;
			Vector3 nPosition;
			Vector3 nextDirection = (path[0].GetPosition() + Vector3.up * .51f - transform.position).normalized;

			transform.position += Quaternion.Euler(0f, 90f, 0f) * nextDirection * 0.25f;

			for (int c = 0, n = 1 ; c < path.Length ; ++c, ++n)
			{
				cPosition = path[c].GetPosition() + Vector3.up * .51f;
				if (n < path.Length)
				{
					nPosition = path[n].GetPosition() + Vector3.up * .51f;
					nextDirection = (nPosition - cPosition).normalized;

					cPosition += nextDirection * .5f + Quaternion.Euler(0f, 90f, 0f) * nextDirection * 0.25f;
				}
				else
				{
					nPosition = path[c - 1].GetPosition() + Vector3.up * .51f;
					nextDirection = (cPosition - nPosition).normalized;

					cPosition += nextDirection * .5f + Quaternion.Euler(0f, 90f, 0f) * nextDirection * 0.25f;
				}
				pointPath.Add(cPosition);
			}

			TweenParams tParam = new TweenParams().SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() => gameObject.SetActive(false));
			pathTween = transform.DOPath(pointPath.ToArray(), speed, PathType.CatmullRom).SetLookAt(0f).SetAs(tParam);
		}
	}
}