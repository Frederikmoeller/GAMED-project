using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveInitializer : MonoBehaviour
{
    public TextMeshProUGUI saveTimeText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI deathCountText;
    public TextMeshProUGUI slotNumber;
    public TextMeshProUGUI collectibleText;

    private int _slot;

    public void Initialize(int slot, string time, float progress, int deaths, int collectible)
    {
        _slot = slot;
        saveTimeText.text = time;
        progressText.text = $"{progress}%";
        deathCountText.text = deaths.ToString();
        slotNumber.text = $"{slot}";
        collectibleText.text = $"{collectible}/{GameManager.GameManagerInstance.totalCollectibles}";
    }
    
    public void InitializeEmpty(int slot)
    {
        _slot = slot;
        slotNumber.text = slot.ToString();
        saveTimeText.text = "Empty Slot";
        progressText.text = "";
        deathCountText.text = "";
        collectibleText.text = "";
    }

    public void LoadGame()
    {
        GameManager.GameManagerInstance.LoadGame(_slot);

        List<Levels> levels = GameManager.GameManagerInstance.GetAllLevels();
        Levels lastLevel = null;

        for (int i = levels.Count - 1; i >= 0; i--)
        {
            if (levels[i].Cleared)
            {
                if (i <= levels.Count - 1)
                {
                    lastLevel = levels[i + 1];
                }
                else
                {
                    lastLevel = levels[i];
                }
            }
        }

        if (lastLevel == null && levels.Count > 0)
        {
            lastLevel = levels[0];
        }

        if (lastLevel != null)
        {
            SceneManager.LoadScene(lastLevel.SceneName);
        }
        else
        {
            Debug.LogWarning("No valid level found to load.");
        }
        
        UIManager.Instance.CloseWindow();
    }
    
    public void OnSaveSlotClicked(int slotNumber)
    {
        if (UIManager.Instance.currentMode == SaveMode.Save)
        {
            GameManager.GameManagerInstance.SaveGame(slotNumber);
            UIManager.Instance.RefreshSaveSlots();
        }
        else
        {
            GameManager.GameManagerInstance.LoadGame(slotNumber);
        }
    }

    public void OnDeleteSlotClicked()
    {
        GameManager.GameManagerInstance.DeleteSave(_slot);
        UIManager.Instance.RefreshSaveSlots();
    }
}
