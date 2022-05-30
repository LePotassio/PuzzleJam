using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum LevelStatus { New, Started, Completed }
[System.Serializable]
public class SaveFileProgress
{
    // In addition to level progress, also want scene and location of savepoint, to be then saved to text doc (make temp to avoid save corruption)
    // Save after any progress, any screen change, savepoint?

    private string checkpointSceneName;
    private (float, float) checkpointSpawnLocation;

    private Dictionary<string, LevelStatus> levelCompletions;

    public string CheckpointSceneName
    {
        get { return checkpointSceneName; }
    }

    public (float, float) CheckpointSpawnLocation
    {
        get { return checkpointSpawnLocation; }
    }

    public Dictionary<string, LevelStatus> LevelCompletions
    {
        get { return levelCompletions; }
    }

    public void SetCheckpoint(string newCheckpointScene, Vector2 newSpawnLocation)
    {
        checkpointSceneName = newCheckpointScene;
        checkpointSpawnLocation = (newSpawnLocation.x, newSpawnLocation.y);
    }

    public SaveFileProgress()
    {
        levelCompletions = new Dictionary<string, LevelStatus>();
        checkpointSceneName = "Puzzle_Lobby_1";
        checkpointSpawnLocation = (-3.5f, -.5f);
        // Unless there is a save file... Or rather check loaded list from file for scene name... Or just do it after...
        /*for (int i = 0; i < GameManager.Instance.BuildSceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string[] split = scenePath.Split('/');
            string[] split2 = split[split.Length - 1].Split('.');
            levelCompletions.Add(split2[0], LevelStatus.New);
            // Debug.Log(split2[0]);
        }*/
    }

    public void SaveProgress(int saveSlot)
    {
        SaveSystem.SaveProgress(saveSlot);
    }

    public LevelStatus GetLevelStatus(string levelSceneName)
    {
        if (levelCompletions.ContainsKey(levelSceneName))
            return LevelCompletions[levelSceneName];
        else
            return LevelStatus.New;
    }

    public void SetLevelStatus(string levelSceneName, LevelStatus newStatus)
    {
        if (levelCompletions.ContainsKey(levelSceneName))
            levelCompletions[levelSceneName] = newStatus;
        else
            levelCompletions.Add(SceneManager.GetActiveScene().name, LevelStatus.Completed);
    }
}
