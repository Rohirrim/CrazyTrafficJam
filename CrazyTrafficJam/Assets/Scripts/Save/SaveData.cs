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
		public SaveGridNode[] crossPoints;
	}

	public class SaveGridNode
	{
		[XmlElement("Name")]
		public string name;
		[XmlAttribute("Type")]
		public ENodeType type;
		[XmlElement("Position")]
		public Vector3 position;
	}
}