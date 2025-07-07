using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuButtons : MonoBehaviour
{
    public static MenuButtons sInstance { get; private set; }

    [SerializeField] List<LevelSelectButton> levelSelectButtons = new List<LevelSelectButton>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (sInstance != null && sInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sInstance = this;
        }
        //sInstance.gameObject.SetActive(false);
    }


    private void Start()
    {
        foreach (LevelSelectButton button in levelSelectButtons)
        {
            button.CheckButtonEnabled();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void UnlockLevelProgress()
    {
        SaveManager.sInstance.SetLevelUnlocked(0);
        foreach (LevelSelectButton button in levelSelectButtons)
        {
            button.CheckButtonEnabled();
        }
    }

    public void ClearLevelProgress()
    {
        Debug.Log("asd");
        SaveManager.sInstance.SetLevelUnlocked(-1);
        foreach (LevelSelectButton button in levelSelectButtons)
        {
            button.CheckButtonEnabled();
        }
    }



}
