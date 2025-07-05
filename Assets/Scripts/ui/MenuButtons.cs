using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void CloseGame()
    {
        Application.Quit();
    }





}
