using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class Link : MonoBehaviour
	{
		[SerializeField]
		private LineRenderer line;
		[SerializeField]
		private BoxCollider2D boxCollider;
		private CrossPoint crossA;
		private CrossPoint crossB;

		public CrossPoint A => crossA;
		public CrossPoint B => crossB;

		public void Init(CrossPoint a, CrossPoint b)
		{
			crossA = a;
			crossB = b;

			UpdateRenderer();
		}

		public void Delete()
		{
			crossA.RemoveLink(crossB);
			Destroy(gameObject);
		}

		public void UpdateRenderer()
		{
			Vector2 posA = crossA.transform.position;
			Vector2 posB = crossB.transform.position;

			line.SetPosition(0, posA);
			line.SetPosition(1, posB);

			transform.position = (posA + posB) * .5f;
			if (posA.x < posB.x)
				transform.eulerAngles = Vector3.forward * Vector3.Angle(Vector3.up, posA - posB);
			else
				transform.eulerAngles = Vector3.forward * Vector3.Angle(Vector3.up, posB - posA);
			boxCollider.size = new Vector2(.1f, (posA - posB).magnitude);
		}

		public bool Contains(CrossPoint crossPoint)
		{
			return crossA == crossPoint || crossB == crossPoint;
		}
	}
}