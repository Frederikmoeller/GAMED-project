using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int deaths;
    public int collectibles;
    public int totalCollectibles;
    public int currentLevel;
    public List<Levels> levelList = new();
    public static GameManager GameManagerInstance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (GameManagerInstance == null)
        {
            GameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LoadLevel(int index)
    {
        UIManager.Instance.fadeAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(1);
        
        if (index >= 0 && index < levelList.Count)
        {
            SceneManager.LoadScene(levelList[index].SceneName);
            currentLevel = index;
            UIManager.Instance.fadeAnimator.SetTrigger("End");
        }
        else
        {
            Debug.LogWarning("Invalid level index");
        }
    }

    public void LoadLevelByWorldAndLevel(int world, int level)
    {
        var match = levelList.Find(l => l.World == world && l.Level == level);

        if (match != null)
        {
            SceneManager.LoadScene(match.SceneName);
        }
        else
        {
            Debug.LogWarning($"Level {level} in world {world} not found.");
        }
    }

    public void MarkLevelCleared(string sceneName)
    {
        var match = levelList.Find(l => l.SceneName == sceneName);

        if (match != null)
        {
            match.Cleared = true;
        }
    }

    public List<Levels> GetAllLevels() => levelList;

    public void SaveGame(int slot)
    {
        SaveData data = new SaveData
        {
            deaths = deaths,
            collectibles = collectibles,
            savedLevels = levelList,
            saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            progressPercent = CalculateProgress()
        };

        string json = JsonUtility.ToJson(data, true);
        string path = GetSavePath(slot);
        File.WriteAllText(path, json);
        Debug.Log($"Game saved to {path}");
    }

    public void LoadGame(int slot)
    {
        string path = GetSavePath(slot);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            deaths = data.deaths;
            collectibles = data.collectibles;
            levelList = data.savedLevels;
            
            Debug.Log("Game loaded.");
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }

    private string GetSavePath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, $"Save_slot_{slot}.json");
    }

    public List<int> GetAvailableSaveSlots()
    {
        List<int> slots = new List<int>();
        string[] files = Directory.GetFiles(Application.persistentDataPath, "Save_slot_*.json");

        foreach (string file in files)
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            if (filename.StartsWith("Save_slot_"))
            {
                string numberString = filename.Replace("Save_slot_", "");
                if (int.TryParse(numberString, out int slot))
                {
                    slots.Add(slot);
                }
            }
        }

        return slots;
    }

    public void AutoSave()
    {
        SaveData data = new SaveData
        {
            deaths = deaths,
            collectibles = collectibles,
            savedLevels = levelList,
            saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            progressPercent = CalculateProgress()
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetAutoSavePath(), json);
        Debug.Log("Auto-saved to: " + GetAutoSavePath());
    }

    public void LoadAutoSave()
    {
        string path = GetAutoSavePath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            deaths = data.deaths;
            collectibles = data.collectibles;
            levelList = data.savedLevels;
            
            Debug.Log("Auto-save loaded.");
        }
        else
        {
            Debug.LogWarning("No auto-save file found.");
        }
    }

    private string GetAutoSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "autosave.json");
    }

    private float CalculateProgress()
    {
        if (levelList == null || levelList.Count == 0)
        {
            return 0;
        }

        int clearedLevels = levelList.FindAll(l => l.Cleared).Count;

        int totalLevels = levelList.Count;
        int totalCollectibles = 100;

        float levelProgress = clearedLevels / (float)totalLevels;

        float collectibleProgress = collectibles / (float)totalCollectibles;

        float totalProgress = (levelProgress + collectibleProgress) / 2f;

        return Mathf.Round(totalProgress * 100f * 10f) / 10f;
    }

    public (string time, float progress, int deaths, int collectibles) PeekSaveSlot(int slot)
    {
        string path = GetSavePath(slot);

        if (!File.Exists(path))
        {
            return ("No Save", 0f, 0, 0);
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return (data.saveTime, data.progressPercent, data.deaths, data.collectibles);
    }

    public void DeleteSave(int slotToDelete)
    {
        string pathToDelete = Path.Combine(Application.persistentDataPath, $"Save_slot_{slotToDelete}.json");
        
        //Delete the actual file
        if (File.Exists(pathToDelete))
        {
            File.Delete(pathToDelete);
        }

        int currentSlot = slotToDelete + 1;

        while (true)
        {
            string currentPath = Path.Combine(Application.persistentDataPath, $"Save_slot_{currentSlot}.json");
            string newPath = Path.Combine(Application.persistentDataPath, $"Save_slot_{currentSlot - 1}.json");

            if (File.Exists(currentPath))
            {
                try
                {
                    File.Move(currentPath, newPath);
                    currentSlot++;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error renaming file {currentPath} to {newPath}: {e.Message}");
                    break;
                }

            }
            else
            {
                break;
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        UIManager.Instance.PauseUI();
    }
}

[Serializable]
public class Levels
{
    public int World;
    public int Level;
    public string SceneName;
    public bool Cleared;

    public Levels(int world, int level, string sceneName, bool cleared)
    {
        World = world;
        Level = level;
        SceneName = sceneName;
        Cleared = cleared;
    }
}

[Serializable]
public class SaveData
{
    public int deaths;
    public int collectibles;
    public List<Levels> savedLevels;
    public string saveTime;
    public float progressPercent;
}
