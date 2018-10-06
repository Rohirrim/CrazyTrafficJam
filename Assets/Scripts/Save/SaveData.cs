using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace IronSideStudio.CrazyTrafficJam
{
	[XmlRoot("CrazyMap")]
	public class Save
	{
		[SerializeField]
		public SaveMap map;
	}

	public class SaveMap
	{
		[XmlElement("Name")]
		public string name;
		[XmlArray("CrossPoints"), XmlArrayItem("CrossPoint")]
		public SaveCrossPoint[] crossPoints;
	}

	public class SaveCrossPoint
	{
		[XmlElement("Name")]
		public string name;
		[XmlAttribute("Id")]
		public int id;
		[XmlAttribute("Type")]
		public CrossPoint.EType type;
		[XmlElement("Round")]
		public int round;
		[XmlElement("Position")]
		public Vector2 position;
		[XmlArray("Links"), XmlArrayItem("linkId")]
		public int[] links;
	}
}