using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionScript : MonoBehaviour
{
    [Header("A modifier")]
    public int pageNumber;
    [HideInInspector]
    public int myLevelInt;
    public LevelParameters myParameters = new LevelParameters();
    public bool blocked = true;

    [Header("Pas touche !")]
    Text myButtonText;

    private void Start()
    {
        myButtonText = transform.GetChild(0).GetComponent<Text>();

        GiveMeMyLevelInt();

        if (blocked)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    private void GiveMeMyLevelInt()
    {
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if(transform.parent.GetChild(i).gameObject == this.gameObject)
            {
                myLevelInt = i + (16 * pageNumber) + 1;//On ajoute 1 car on est dans une liste, on multiplie par 16 car il y a 16 niveaux par pages
            }
        }

        myButtonText.text = "Level " + myLevelInt.ToString();
    }

    public void ClickMe()
    {
        LevelManager.Instance.SelectThisLevel(myParameters);
    }
}
