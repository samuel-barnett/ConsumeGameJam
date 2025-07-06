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

    public void SetLevelsUnlocked(int newLevelsUnlocked)
    {
        PlayerPrefs.SetInt("LevelsUnlocked", PlayerPrefs.GetInt("LevelsUnlocked") + newLevelsUnlocked);
    }

    public int GetLevelsUnlocked()
    {
        return PlayerPrefs.GetInt("LevelsUnlocked");
    }

}
