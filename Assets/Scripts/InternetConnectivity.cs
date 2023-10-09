using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class InternetConnectivity : MonoBehaviour
{
    private bool fail = false;
    public string[] levelUrls =
    {
        "https://row-match.s3.amazonaws.com/levels/RM_A11",
        "https://row-match.s3.amazonaws.com/levels/RM_A12",
        "https://row-match.s3.amazonaws.com/levels/RM_A13",
        "https://row-match.s3.amazonaws.com/levels/RM_A14",
        "https://row-match.s3.amazonaws.com/levels/RM_A15",
        "https://row-match.s3.amazonaws.com/levels/RM_B1",
        "https://row-match.s3.amazonaws.com/levels/RM_B2",
        "https://row-match.s3.amazonaws.com/levels/RM_B3",
        "https://row-match.s3.amazonaws.com/levels/RM_B4",
        "https://row-match.s3.amazonaws.com/levels/RM_B5",
        "https://row-match.s3.amazonaws.com/levels/RM_B6",
        "https://row-match.s3.amazonaws.com/levels/RM_B7",
        "https://row-match.s3.amazonaws.com/levels/RM_B8",
        "https://row-match.s3.amazonaws.com/levels/RM_B9",
        "https://row-match.s3.amazonaws.com/levels/RM_B10",
    }; // Array of URLs to download levels from

    private void Start()
    {
        CheckInternetConnection();
    }

    private void CheckInternetConnection()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                Debug.Log("No internet connection available.");
                break;

            case NetworkReachability.ReachableViaCarrierDataNetwork:
                Debug.Log("Connected to the internet via mobile data.");
                DownloadLevels();
                break;

            case NetworkReachability.ReachableViaLocalAreaNetwork:
                Debug.Log("Connected to the internet via Wi-Fi.");
                DownloadLevels();
                break;
        }
    }


    public void DownloadLevels()
    {
        Caching.ClearCache();
        StartCoroutine(DownloadAllLevels());
    }

    private IEnumerator DownloadAllLevels()
    {
        bool allDownloadsSuccessful = true; // Flag to track if all downloads were successful

        foreach (string url in levelUrls)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Save the downloaded level data to persistent storage
                SaveLevelData(url, request.downloadHandler.text);
            }
            else
            {
                allDownloadsSuccessful = false;
                Debug.LogError("Failed to download level from URL: " + url);
            }
        }

        if (allDownloadsSuccessful)
        {
            UniversalVariables.Instance.levelsDownloaded = true;
            Debug.Log("Levels downloaded.");
            LevelDatabase.Instance.populateLevelList(10);
        }
        else
        {
            UniversalVariables.Instance.levelsDownloaded = false;
            Debug.LogError("Some level downloads failed.");
        }
    }


    private void SaveLevelData(string url, string levelData)
    {
        int lastUnderscoreIndex = url.LastIndexOf('/');
        string levelCode = url.Substring(lastUnderscoreIndex+1);
        string filePath = Path.Combine(Path.Combine(Application.dataPath, "Levels"), levelCode);
        File.WriteAllText(filePath, levelData);
    }

}
