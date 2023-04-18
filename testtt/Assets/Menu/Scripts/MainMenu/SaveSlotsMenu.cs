using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Button")]
    [SerializeField] private Button backButton;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopUpMenu confirmPopUpMenu;
    
    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;
    
    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        //Disable all buttons
        DisableMenuButtons();

        // case - Loading game
        if(isLoadingGame)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveGameAndLoadScene();
        }
        // case - new game, but the save slot has data
        else if(saveSlot.hasData)
        {
            confirmPopUpMenu.ActivateMenu(
                "Starting a New Game with this slot will override the currently saved data. Are you sure?",
                // function to execute if we select "yes"
                () =>
                {
                    DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                    DataPersistenceManager.instance.NewGame();
                    SaveGameAndLoadScene();
                },
                // function to execute if we select "cancel"
                () =>
                {
                    this.ActivateMenu(isLoadingGame);
                }
            );
        }
        // Case - new game, and the save slot has no data
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            DataPersistenceManager.instance.NewGame();
            SaveGameAndLoadScene();
        }
    }
    private void SaveGameAndLoadScene()
    {
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();
        // load the scene 
        SceneManager.LoadSceneAsync("SampleScene");
    }
    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confirmPopUpMenu.ActivateMenu(
               "Are you sure you want to delete this saved data?",
               // function to execute if we select "yes"
               () =>
               {
                   DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
                   ActivateMenu(isLoadingGame);
               },
               // function to execute if we select "cancel"
               () =>
               {
                   ActivateMenu(isLoadingGame);
               }
           );
    }
    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }
    public void ActivateMenu(bool isLoadingGame)
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        //Set mode
        this.isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        // ensure the back button is enabled when we activate the menu
        backButton.interactable = true;

        //Loop through each save slot in the ui and set the content appropriately
        GameObject firsttSelected = backButton.gameObject;
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if(firsttSelected.Equals(backButton.gameObject))
                {
                    firsttSelected = saveSlot.gameObject;
                }
            }

            // set the first selected button
            Button firstSelectedButton = firsttSelected.GetComponent<Button>();
            this.SetfirstSelected(firstSelectedButton);
        }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}