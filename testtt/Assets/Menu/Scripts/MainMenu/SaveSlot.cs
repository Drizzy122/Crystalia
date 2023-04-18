using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject HasDataContent;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI deathCountText;

    [Header("Clear data button")]
    [SerializeField] private Button clearButton;

    public bool hasData { get; private set; } = false;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }
    public void SetData(GameData data)
    {
        // there's no data for this profileId
        if(data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            HasDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
        }
        // thre is data for this profileId
        else
        {
            hasData = true; 
            noDataContent.SetActive(false);
            HasDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);

            percentageCompleteText.text = data.GetPercentageComplete() + "% COMPLETE";
            deathCountText.text =" DEATH count: " + data.deathCount;
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearButton.interactable = interactable;
    }
}