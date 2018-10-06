using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class UiCanvas : MonoBehaviour
	{
		public void View(bool active, Vector2 position)
		{
			gameObject.SetActive(active);
			transform.position = position;
		}
	}
}