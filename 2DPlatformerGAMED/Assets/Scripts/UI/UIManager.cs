using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _canvas;
    [Header("Save system")]
    [SerializeField] private GameObject _saveMenu;
    [SerializeField] private GameObject _saveContent;
    [SerializeField] private GameObject _saveSlotPrefab;
    

    private void Start()
    {
        
    }

    public void MainMenuLoadGame()
    {
        _saveMenu.SetActive(true);
        LoadAllSaveFiles();
    }

    public void CloseWindow()
    {
        
    }

    private void LoadAllSaveFiles()
    {
        foreach (Transform child in _saveContent.transform)
        {
            Destroy(child.gameObject);
        }

        List<int> slots = GameManager.GameManagerInstance.GetAvailableSaveSlots();

        foreach (int slot in slots)
        {
            var (time, progress, deaths, collectibles) = GameManager.GameManagerInstance.PeekSaveSlot(slot);
            GameObject slotGO = Instantiate(_saveSlotPrefab, _saveContent.transform);
            SaveInitializer initializer = slotGO.GetComponent<SaveInitializer>();

            if (initializer != null)
            {
                initializer.Initialize(slot, time, progress, deaths, collectibles);
            }
        }
    }
    
}
