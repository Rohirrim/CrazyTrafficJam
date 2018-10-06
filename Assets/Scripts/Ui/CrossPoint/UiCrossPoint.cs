using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam
{
	public class UiCrossPoint : UiCanvas
	{
		[SerializeField]
		private InputField nameField;
		[SerializeField]
		private Dropdown dropdownType;
		[SerializeField]
		private InputField roundField;
		[SerializeField]
		private Button btnDelete;

		private CrossPointManager crossManager;
		private CrossPoint currentSelection;

		private void Start()
		{
			crossManager = CoreManager.Instance.GetManager<CrossPointManager>();

			nameField.onEndEdit.AddListener((newName) => currentSelection.name = newName);
			dropdownType.onValueChanged.AddListener(SelectType);
			roundField.onEndEdit.AddListener(ChangeRound);
			btnDelete.onClick.AddListener(DeleteCross);

			dropdownType.AddOptions(System.Enum.GetNames(typeof(CrossPoint.EType)).ToList());
		}

		public void Select(CrossPoint crossRoad)
		{
			currentSelection = crossRoad;
			if (currentSelection == null)
				return;
			nameField.text = currentSelection.name;
			dropdownType.value = (int)currentSelection.CrossType;
			roundField.text = currentSelection.Round.ToString();
		}

		private void SelectType(int intType)
		{
			if (System.Enum.IsDefined(typeof(CrossPoint.EType), intType))
				currentSelection.ChangeType((CrossPoint.EType)intType);
		}

		private void ChangeRound(string round)
		{
			int r = 0;
			if (int.TryParse(round, out r))
			{
				if (r < 0)
				{
					roundField.text = "0";
					r = 0;
				}
				currentSelection.ChangeRound(r);
			}
		}

		private void DeleteCross()
		{
			crossManager.DeletePoint();
			gameObject.SetActive(false);
		}
	}
}