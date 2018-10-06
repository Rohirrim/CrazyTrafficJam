using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	[System.Serializable]
	public class CrossPoint : MonoBehaviour
	{
		public enum EType
		{
			Cross,
			Circle
		}

		[SerializeField]
		private int id;
		[SerializeField]
		private EType crossType;
		private int roundEnable;
		private List<Link> allLink;

		public int Id => id;
		public EType CrossType => crossType;
		public int Round => roundEnable;

		private void Awake()
		{
			id = -1;
		}

		public void Init(int id)
		{
			if (this.id != -1)
				return;
			this.id = id;
			name = "New cross road";
			allLink = new List<Link>();
		}

		#region Save&Load
		public SaveCrossPoint Save()
		{
			int[] linksId = new int[allLink.Count];
			int i = 0;

			foreach (Link l in allLink)
			{
				linksId[i++] = l.A == this ? l.B.id : l.A.id;
			}

			SaveCrossPoint save = new SaveCrossPoint() {
				name = name,
				id = id,
				type = crossType,
				round = roundEnable,
				position = transform.position,
				links = linksId
			};

			return save;
		}

		public void Load(CrossPointManager crossManager, SaveCrossPoint save)
		{
			name = save.name;
			crossType = save.type;
			roundEnable = save.round;
			UpdatePosition(save.position);

			foreach (int i in save.links)
			{
				CrossPoint linkCross = crossManager.GetCross(i);
				AddLink(linkCross);
			}
		}
		#endregion

		public void Delete()
		{
			while (allLink.Count > 0)
			{
				Link l = allLink[0];

				allLink.Remove(l);
				if (l.A == this)
					l.B.allLink.Remove(l);
				else
					l.A.allLink.Remove(l);
				Destroy(l.gameObject);
			}

			Destroy(gameObject);
		}

		public void UpdatePosition(Vector2 position)
		{
			transform.position = position;
			foreach (Link link in allLink)
				link.UpdateRenderer();
		}

		public void ChangeType(EType newType)
		{
			crossType = newType;
		}

		public void ChangeRound(int i)
		{
			roundEnable = i;
		}

		#region Link
		public void AddLink(CrossPoint crossPoint)
		{
			if (ContainLink(crossPoint))
				return;

			Link newLink = CoreManager.Instance.GetManager<LinkManager>().CreateLink(this, crossPoint);
			allLink.Add(newLink);
			crossPoint.allLink.Add(newLink);
		}

		public void RemoveLink(CrossPoint crossPoint)
		{
			Link toRemove = GetLink(crossPoint);
			if (toRemove == null)
				return;
			allLink.Remove(toRemove);
			crossPoint.allLink.Remove(toRemove);
		}

		public bool ContainLink(CrossPoint crossPoint)
		{
			return GetLink(crossPoint) != null;
		}

		private Link GetLink(CrossPoint crossLink)
		{
			foreach (Link link in allLink)
			{
				if (link.Contains(crossLink))
					return link;
			}

			return null;
		}
		#endregion
	}
}