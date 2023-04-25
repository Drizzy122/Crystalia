using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("First selected Button")]
    [SerializeField] private Button firstSelected;

    protected virtual void OnEnable()
    {
        SetfirstSelected(firstSelected);
    }

    public void SetfirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}
