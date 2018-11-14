using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	[CreateAssetMenu(fileName = "IntersectionNode", menuName = "GridNodeType/Intersection")]
	public class IntersectionNode : NodeType
	{
		public override void Initialize()
		{
			base.Initialize();
		}

		protected override void Behaviour(Node gridNode)
		{

		}
	}
}