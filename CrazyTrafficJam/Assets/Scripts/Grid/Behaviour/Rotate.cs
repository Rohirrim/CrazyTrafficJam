using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	[CreateAssetMenu(fileName = "Rotate", menuName = "Rotate")]
	public class Rotate : IGridBehaviour
	{
		[SerializeField]
		private float angle;

		public override void Behaviour(GridNode n)
		{
			n.transform.Rotate(Vector3.up, angle * Time.deltaTime);
		}
	}
}