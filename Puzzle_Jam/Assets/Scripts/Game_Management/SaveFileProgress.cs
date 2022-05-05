using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum LevelStatus { New, Started, Completed }
public class SaveFileProgress
{
    private Dictionary<string, LevelStatus> levelCompletions;

    public Dictionary<string, LevelStatus> LevelCompletions
    {
        get { return levelCompletions; }
    }

    public SaveFileProgress()
    {
        levelCompletions = new Dictionary<string, LevelStatus>();

        // Unless there is a save file... Or rather check loaded list from file for scene name... Or just do it after...
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string[] split = scenePath.Split('/');
            string[] split2 = split[split.Length - 1].Split('.');
            levelCompletions.Add(split2[0], LevelStatus.New);
            // Debug.Log(split2[0]);
        }
    }
}
