using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelParameters levelSelected = new LevelParameters();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SelectThisLevel(LevelParameters newParameters)
    {
        levelSelected = newParameters;

        SceneManager.LoadScene(2);
    }
}
