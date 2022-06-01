using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveProgress(int saveSlot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //string path = Application.persistentDataPath + "/save" + saveSlot + ".frz";
        string path = GameSettings.GetSaveFilePath(saveSlot);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, GameManager.Instance.SaveFileProgress);
        stream.Close();
    }

    public static SaveFileProgress LoadProgress(int saveSlot)
    {
        string path = Application.persistentDataPath + "/save" + saveSlot + ".frz";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveFileProgress progress = formatter.Deserialize(stream) as SaveFileProgress;

            stream.Close();

            return progress;
        }
        else
        {
            Debug.LogError("Save file in slot " + saveSlot + " not found in " + path);
            return null;
        }
    }
}
