using TMPro;
using UnityEngine;
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

    public void LoadGame()
    {
        GameManager.GameManagerInstance.LoadGame(_slot);
    }
}
