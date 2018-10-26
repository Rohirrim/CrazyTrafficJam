using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	[CreateAssetMenu(fileName = "Car", menuName = "Car")]
	public class Car : ScriptableObject
	{
		[SerializeField]
		private GameObject carPrefab;

		public GameObject Spawn()
		{
			return Instantiate(carPrefab);
		}
	}
}