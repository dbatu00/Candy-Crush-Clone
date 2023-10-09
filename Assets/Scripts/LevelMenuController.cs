using UnityEngine;
using UnityEngine.UI; 

public class LevelMenuController : MonoBehaviour
{  
    public Level[] levels;
    public GameObject levelDisplayPrefab; 
    public Transform levelDisplayContainer;  

    void Start()
    {
        PopulateLevelSelectionScreen();
    }

    private void PopulateLevelSelectionScreen()
    {
        Debug.Log($"Highest level: {UniversalVariables.Instance.highestLevel}");
        foreach (Level level in LevelDatabase.levelList)
        {        
            GameObject levelInfoObj = Instantiate(levelDisplayPrefab, levelDisplayContainer);
            Button button = levelInfoObj.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => GameManager.instance.changeLevel(level.level_number));
            
            if (UniversalVariables.Instance.highestLevel +1 < level.level_number) //lvl3 should be playable if highest level is 2
            {
                button.interactable = false;
                button.image.color = Color.gray;
            }
            
            if(level.grid_height == 0)
            {
                button.interactable = false;
                button.image.color = Color.red;
            }
           
            Text[] texts = levelInfoObj.GetComponentsInChildren<Text>(true);
           
            if (texts.Length >= 2)
            {
                texts[0].text = "Level: " + level.level_number + " - Moves: " + level.move_count;
                texts[1].text = "High Score: " + level.highScore;
            }
        }
    }
}
