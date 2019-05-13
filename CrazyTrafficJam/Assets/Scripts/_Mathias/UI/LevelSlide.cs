using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct LevelParameters
{
    public IronSideStudio.CrazyTrafficJam.Grid.Pathern startingPathern;

    [Range(1, 100)]
    public float defeatPercentage;

    [Range(10, 100)]
    public int minimumCarsNumber;

    public int numberOfTrafficLights, numberOfTrafficCircles, numberOfTrafficPriority;
}

public class LevelSlide : MonoBehaviour
{
    [Header("Ouvertes pour modif")]
    public float transitionSpeed = 2f;
    int currentPage = 0;
    [Range(0.1f, 2)]
    public float clampAcceptance = 0.5f;

    [Header("Pas touche")]
    public List<GameObject> allLevelPannels = new List<GameObject>();
    public List<Vector3> allLevelPannelsPositions = new List<Vector3>();
    public bool rightClicked = false, leftClicked = false;

    public GameObject RightButton, LeftButton;

    private void Start()
    {
        InitialisePannelsPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (rightClicked)
        {
            RightClickTransition();
        }

        if (leftClicked)
        {
            LeftClickTransition();
        }

        if(currentPage == NumberOfPages() && !leftClicked && !rightClicked)
        {
            RightButton.SetActive(false);
            LeftButton.SetActive(true);
        }
        else if(currentPage == 0 && !leftClicked && !rightClicked)
        {
            LeftButton.SetActive(false);
            RightButton.SetActive(true);
        }
        else if(rightClicked || leftClicked)
        {
            RightButton.SetActive(false);
            LeftButton.SetActive(false);
        }
        else
        {
            RightButton.SetActive(true);
            LeftButton.SetActive(true);
        }
    }

    private void InitialisePannelsPositions()
    {
        for(int i = 0; i < allLevelPannels.Count; i++)
        {
            allLevelPannelsPositions.Add(allLevelPannels[i].transform.position);
        }
    }

    private void ActualisePannelsPosition()
    {
        for(int i = 0; i < allLevelPannelsPositions.Count; i++)
        {
            allLevelPannelsPositions[i] = allLevelPannels[i].transform.position;
        }
    }

    public void RightClick()
    {
        rightClicked = true;
        currentPage += 1;
    }

    private void RightClickTransition()
    {
        for (int i = 0; i < allLevelPannels.Count; i++)
        {
            if (i > 0)
            {
                allLevelPannels[i].transform.position = Vector3.Lerp(allLevelPannels[i].transform.position, allLevelPannelsPositions[i - 1], transitionSpeed * Time.deltaTime);

                if(Mathf.Abs(allLevelPannels[i].transform.position.x - allLevelPannelsPositions[i - 1].x) < clampAcceptance)//pour le clamp
                {
                    allLevelPannels[i].transform.position = allLevelPannelsPositions[i - 1];
                }
            }
            else
            {
                float Xdif = Mathf.Abs(allLevelPannelsPositions[0].x - allLevelPannelsPositions[1].x);
                Vector3 NewPos = new Vector3(allLevelPannelsPositions[0].x - Xdif, allLevelPannelsPositions[0].y, allLevelPannelsPositions[0].z);

                allLevelPannels[0].transform.position = Vector3.Lerp(allLevelPannels[0].transform.position, NewPos, transitionSpeed * Time.deltaTime);

                if(Mathf.Abs(allLevelPannels[0].transform.position.x - NewPos.x) < clampAcceptance)
                {
                    allLevelPannels[0].transform.position = NewPos;
                }
            }
        }

        //On prend le dernier pour check la fin du lerp
        if(allLevelPannels[allLevelPannels.Count - 1].transform.position == allLevelPannelsPositions[allLevelPannelsPositions.Count - 2])
        {
            rightClicked = false;
            ActualisePannelsPosition();
        }
    }

    public void LeftClick()
    {
        leftClicked = true;
        currentPage -= 1;
    }

    public void LeftClickTransition()
    {
        for (int i = 0; i < allLevelPannels.Count; i++)
        {
            if (i < allLevelPannels.Count - 1)
            {
                allLevelPannels[i].transform.position = Vector3.Lerp(allLevelPannels[i].transform.position, allLevelPannelsPositions[i + 1], transitionSpeed * Time.deltaTime);

                if (Mathf.Abs(allLevelPannels[i].transform.position.x - allLevelPannelsPositions[i + 1].x) < clampAcceptance)//pour le clamp
                {
                    allLevelPannels[i].transform.position = allLevelPannelsPositions[i + 1];
                }
            }
            else
            {
                float Xdif = Mathf.Abs(allLevelPannelsPositions[allLevelPannelsPositions.Count - 1].x - allLevelPannelsPositions[allLevelPannelsPositions.Count - 2].x);
                Vector3 NewPos = new Vector3(allLevelPannelsPositions[allLevelPannelsPositions.Count - 1].x + Xdif, allLevelPannelsPositions[allLevelPannelsPositions.Count - 1].y, allLevelPannelsPositions[allLevelPannelsPositions.Count - 1].z);

                allLevelPannels[allLevelPannels.Count - 1].transform.position = Vector3.Lerp(allLevelPannels[allLevelPannels.Count - 1].transform.position, NewPos, transitionSpeed * Time.deltaTime);

                if (Mathf.Abs(allLevelPannels[allLevelPannels.Count - 1].transform.position.x - NewPos.x) < clampAcceptance)
                {
                    allLevelPannels[allLevelPannels.Count - 1].transform.position = NewPos;
                }
            }
        }

        //On prend le premier pour check la fin du lerp
        if (allLevelPannels[0].transform.position == allLevelPannelsPositions[1])
        {
            leftClicked = false;
            ActualisePannelsPosition();
        }
    }

    public GameObject CurrentLevelWindow()
    {
        if(allLevelPannels.Count > 0)
        {
            return allLevelPannels[currentPage];
        }
        else
        {
            return null;
        }
    }

    public int NumberOfPages()
    {
        if(allLevelPannels.Count > 0)
        {
            return allLevelPannels.Count - 1;
        }
        else
        {
            return 0;
        }
    }
}
