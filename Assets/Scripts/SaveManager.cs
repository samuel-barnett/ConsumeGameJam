using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager sInstance { get; private set; }

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevelUnlocked(int levelToUnlock)
    {
        switch (levelToUnlock)
        {
            case -1:
                PlayerPrefs.SetInt("Level1Unlocked", 1);
                PlayerPrefs.SetInt("Level2Unlocked", 0);
                PlayerPrefs.SetInt("Level3Unlocked", 0);
                PlayerPrefs.SetInt("Level4Unlocked", 0);
                PlayerPrefs.SetInt("Level5Unlocked", 0);
                PlayerPrefs.SetInt("Level6Unlocked", 0);
                break;
            case 0:
                PlayerPrefs.SetInt("Level1Unlocked", 1);
                PlayerPrefs.SetInt("Level2Unlocked", 1);
                PlayerPrefs.SetInt("Level3Unlocked", 1);
                PlayerPrefs.SetInt("Level4Unlocked", 1);
                PlayerPrefs.SetInt("Level5Unlocked", 1);
                PlayerPrefs.SetInt("Level6Unlocked", 1);
                break;
            case 1:
                PlayerPrefs.SetInt("Level1Unlocked", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("Level2Unlocked", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("Level3Unlocked", 1);
                break;
            case 4:
                PlayerPrefs.SetInt("Level4Unlocked", 1);
                break;
            case 5:
                PlayerPrefs.SetInt("Level5Unlocked", 1);
                break;
            case 6:
                PlayerPrefs.SetInt("Level6Unlocked", 1);
                break;
        }

        
    }

    public int GetLevelUnlocked(int levelToCheck)
    {
        switch (levelToCheck)
        {
            case 1:
                return PlayerPrefs.GetInt("Level1Unlocked");
            case 2:
                return PlayerPrefs.GetInt("Level2Unlocked");
            case 3:
                return PlayerPrefs.GetInt("Level3Unlocked");
            case 4:
                return PlayerPrefs.GetInt("Level4Unlocked");
            case 5:
                return PlayerPrefs.GetInt("Level5Unlocked");
            case 6:
                return PlayerPrefs.GetInt("Level6Unlocked");
        }

        return -1;
    }

}
