using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

    public enum SaveMode
    {
        Save,
        Load
    }
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _canvas;

    [Header("Save system")] 
    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _saveMenu;
    [SerializeField] private GameObject _loadMenu;
    [SerializeField] private GameObject _saveContent;
    [SerializeField] private GameObject _saveSlotPrefab;
    [SerializeField] private GameObject _emptySaveSlotPrefab;
    [SerializeField] private EventSystem eventSystem;
    public Animator fadeAnimator;
    public SaveMode currentMode { get; private set; }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            SetMainMenuActive();
        }
        

    }
    
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (eventSystem == null)
        {
            eventSystem = FindFirstObjectByType<EventSystem>();
            DontDestroyOnLoad(eventSystem.gameObject);
        }
    }

    private void SetupContinueButton()
    {
        string path = Path.Combine(Application.persistentDataPath, "autosave.json");

        _continueButton.gameObject.SetActive(File.Exists(path));
    }

    public void SetMainMenuActive()
    {
        _mainMenu.SetActive(true);
    }

    public void OnContinueClicked()
    {
        GameManager.GameManagerInstance.LoadAutoSave();

        var lastClearedLevel = GameManager.GameManagerInstance.levelList.FindLast(l => l.Cleared);

        if (lastClearedLevel != null)
        {
            StartCoroutine(GameManager.GameManagerInstance.LoadLevel(GameManager.GameManagerInstance.levelList.IndexOf(lastClearedLevel)));
        }
        else
        {
            StartCoroutine(GameManager.GameManagerInstance.LoadLevel(1));
        }
        CloseWindow();
    }

    public void OnStartGameClicked()
    {
        StartCoroutine(GameManager.GameManagerInstance.LoadLevel(1));
        CloseWindow();
    }

    public void BackToTitleScreen()
    {
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _saveMenu.SetActive(false);
        _loadMenu.SetActive(false);
        StartCoroutine(GameManager.GameManagerInstance.LoadLevel(SceneManager.GetSceneByBuildIndex(0).buildIndex));
    }

    public void OpenLoadMenu()
    {
        currentMode = SaveMode.Load;
        _loadMenu.SetActive(true);
        LoadAllSaveFiles();
    }

    public void OpenSaveMenu()
    {
        currentMode = SaveMode.Save;
        _saveMenu.SetActive(true);
        LoadAllSaveFiles();
    }

    public void CloseWindow()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;

        if (clicked == null) return;

        Transform current = clicked.transform;

        while (current != null)
        {
            if (current.CompareTag("Window"))
            {
                current.gameObject.SetActive(false);
                return;
            }
            current = current.parent;
        }
    }

    private void LoadAllSaveFiles()
    {
        foreach (Transform child in _saveContent.transform)
        {
            Destroy(child.gameObject);
        }

        List<int> existingSlots = GameManager.GameManagerInstance.GetAvailableSaveSlots();
        existingSlots.Sort();

        foreach (int slot in existingSlots)
        {
            var (time, progress, deaths, collectibles) = GameManager.GameManagerInstance.PeekSaveSlot(slot);
            GameObject slotGO = Instantiate(_saveSlotPrefab, _saveContent.transform);
            SaveInitializer initializer = slotGO.GetComponent<SaveInitializer>();

            if (initializer != null)
            {
                initializer.Initialize(slot, time, progress, deaths, collectibles);
            }
        }

        if (currentMode == SaveMode.Save)
        {
            int newSlot = existingSlots.Count > 0 ? existingSlots[^1] + 1 : 0;
            
            GameObject emptySlotGO = Instantiate(_emptySaveSlotPrefab, _saveContent.transform);
            SaveInitializer emptyInitializer = emptySlotGO.GetComponent<SaveInitializer>();
            
            if (emptyInitializer != null)
            {
                emptyInitializer.InitializeEmpty(newSlot);
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        CloseWindow();
    }

    public void PauseUI()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }

    public void RefreshSaveSlots()
    {
        LoadAllSaveFiles();
    }
}
