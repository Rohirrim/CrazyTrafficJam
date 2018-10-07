using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	[CreateAssetMenu(fileName = "GridNodeType", menuName = "GridNodeType")]
	public class GridNodeType : ScriptableObject
	{
		[SerializeField]
		private ENodeType nodeType;
		[SerializeField]
		private IGridBehaviour behaviour;
		public Color color;

		public ENodeType NodeType => nodeType;

		public void Update()
		{
			if (behaviour)
				behaviour.Behaviour();
		}
	}
}