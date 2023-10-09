using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Changes levels and updates highest level
/// MainMenu and LevelSelection also treated as levels
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int _level = -2; //mainmenu
    
    private int highestLevel = UniversalVariables.Instance.highestLevel;

    private void Awake()
    {
        instance = this;
    }

    public void changeLevel(int level)
    {
        if(highestLevel < level)
        {
            highestLevel = level;
            UniversalVariables.Instance.highestLevel = level;
            Debug.Log($"New highest level! {highestLevel}");
        }

        if(level == -2)//at mainMenu and want to navigate to levelselection
        {
            _level = -1;
            SceneManager.LoadScene("LevelSelection");
        }
        else if(level == -1)//at levelselection want to navigate to mainmenu
        {
            _level = -2;
            SceneManager.LoadScene("MainMenu");
        }
        else //level play buttons
        {
            _level = level;
            SceneManager.LoadScene("SampleGameplay");
        }
    }  
}
