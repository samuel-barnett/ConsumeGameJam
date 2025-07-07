using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [Header("How many levels must be completed to unlock this level?")]
    [SerializeField] int levelIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //CheckButtonEnabled();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void CheckButtonEnabled()
    {
        int levelSave = SaveManager.sInstance.GetLevelUnlocked(levelIndex);
        Debug.Log(levelIndex + ":" + levelSave);
        if (levelSave == 1)
        {
            Button button = GetComponent<Button>();
            button.enabled = true;
            Image image = GetComponent<Image>();
            image.color = Color.white;
            //Debug.Log("player can play this level");
        }
        else
        {
            Button button = GetComponent<Button>();
            button.enabled = false;
            Image image = GetComponent<Image>();
            image.color = Color.gray;
            //Debug.Log("player cannot play this level");
        }
    }

}
