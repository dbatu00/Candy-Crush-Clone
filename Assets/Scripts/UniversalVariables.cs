using UnityEngine;

public class UniversalVariables : MonoBehaviour
{
    public static UniversalVariables Instance;

    public int highestLevel = 0;
    public bool placeholderLevelListPopulated = false;
    public bool levelListPopulated = false;
    public bool levelsDownloaded = false;
    public bool downloadedLevelsAdded = false;
    public bool playAnimation = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
