using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam.UI
{
	public class SelectType : MonoBehaviour, IInitializable, ICleanable
	{
		[SerializeField]
		private Button selectBtn;
		[SerializeField]
		private Text countTxt;
		[SerializeField]
		private int count;
		[SerializeField]
		private Grid.NodeType nodeType;

		GridNodeSelection nodeSelection;

		public void Initialize()
		{
            if (nodeType == null)
            {
                gameObject.SetActive(false);
                return;
            }
            else if (nodeType != null)//pour l'initialisation suite à l'instance du niveau
            {
                if (LevelManager.Instance != null)
                {
                    if (nodeType.myName == "Traffic Lights")
                    {
                        count = LevelManager.Instance.levelSelected.numberOfTrafficLights;
                    }
                    else if (nodeType.myName == "Traffic Circle")
                    {
                        count = LevelManager.Instance.levelSelected.numberOfTrafficCircles;
                    }
                    else if (nodeType.myName == "Right Priority Road")
                    {
                        count = LevelManager.Instance.levelSelected.numberOfTrafficPriority;
                    }
                }
            }

			if (count == -1)
			{
				countTxt.transform.parent.gameObject.SetActive(false);
			}

			selectBtn.onClick.AddListener(Select);
			nodeSelection = gameObject.GetComponentInParent<GridNodeSelection>();
			nodeSelection.AddOnOpenSelection(OnOpen);
		}

		public void Clean()
		{
			selectBtn.onClick.RemoveAllListeners();
			nodeSelection.RemoveOnOpenSelection(OnOpen);
		}

		public void Select()
		{
			if (count != -1 && count == nodeType.Count)
				return;
			nodeType.AddNode(nodeSelection.GridSelected);
		}

		public void AddCount(int amount)
		{
			count += amount;
		}

		private void OnOpen(bool b)
		{
			countTxt.text = (count - nodeType.Count).ToString();

			// revoir ces conditions ?
			if (nodeType.Contains(nodeSelection.GridSelected))
				selectBtn.interactable = false;
			else if (count != -1)
				selectBtn.interactable = nodeType ? count > nodeType.Count : false;
			else
				selectBtn.interactable = true;
		}
	}
}