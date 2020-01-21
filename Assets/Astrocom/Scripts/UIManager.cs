using Astrocom.Scripts.ARCoreScripts.ManipulationSystem;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// Manipulate standart UI which control objects and
/// can instantiate specified UI for each object
/// </summary>
public class UIManager : MonoBehaviour
{
    private Canvas MainCanvas;

    [SerializeField]
    private GameObject StandartUI;
    [SerializeField]
    private GameObject RemoveButton;

    private AstrocomObjectManager astrocomObjectManager;

    void Start()
    {
        MainCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        astrocomObjectManager = GameObject.FindObjectOfType<AstrocomObjectManager>();
        
        if(astrocomObjectManager != null)
        {
            astrocomObjectManager.AstrocomObjectSelected += OnAstrocomObjectSelected;
            astrocomObjectManager.AstrocomObjectDeselected += OnObjectDeselected;
            astrocomObjectManager.AstrocomObjectRemoved += OnObjectRemoved;
        }
        else
            Debug.LogWarning("Astrocom object manager not found in scene");

        StandartUI.SetActive(false);
        RemoveButton.SetActive(false);
    }

    public void OnAstrocomObjectSelected(AstrocomObject astrocomSelectedObject)
    {
        RemoveButton.SetActive(true);

        var individualUiObject = astrocomSelectedObject.IndividualUiObject;
        var instantiatedUi = astrocomSelectedObject.InstantiatedUi;
        var onlyIndividualUi = astrocomSelectedObject.OnlyIndividualUi;

        if(individualUiObject != null && instantiatedUi == null)
        {
            GameObject tempInstantiatedUi = Instantiate(astrocomSelectedObject.IndividualUiObject);
            tempInstantiatedUi.transform.SetParent(MainCanvas.transform, false);
            tempInstantiatedUi.SetActive(true);
            astrocomSelectedObject.InstantiatedUi = tempInstantiatedUi;
        }
        
        else if(instantiatedUi != null && onlyIndividualUi)
        {
            astrocomSelectedObject.InstantiatedUi.SetActive(true);
            StandartUI.SetActive(false);
        }
            
        else if(instantiatedUi == null && !onlyIndividualUi)
            StandartUI.SetActive(true);

        else if(instantiatedUi != null && !onlyIndividualUi)
        {
            astrocomSelectedObject.InstantiatedUi.SetActive(true);
            StandartUI.SetActive(true);
        }

        else if(instantiatedUi == null && onlyIndividualUi)
            StandartUI.SetActive(false);
    }

    //FIXME: When object deselected deactivate remove button (but it's doesn't work as suppose)
    public void OnObjectDeselected(AstrocomObject astrocomDeselectedObject)
    {
        // RemoveButton.SetActive(false)

        if(astrocomDeselectedObject.InstantiatedUi == null)
            StandartUI.SetActive(false);

        else
        {
            astrocomDeselectedObject.InstantiatedUi.SetActive(false);
            StandartUI.SetActive(false);
        }
    }

    public void OnObjectRemoved(AstrocomObject astrocomRemovedObject)
    {
        RemoveButton.SetActive(false);
        
        if(astrocomRemovedObject.InstantiatedUi == null)
            StandartUI.SetActive(false);

        else
        {
            astrocomRemovedObject.InstantiatedUi.SetActive(false);
            Destroy(astrocomRemovedObject.InstantiatedUi);
            astrocomRemovedObject.InstantiatedUi = null;
            StandartUI.SetActive(false);
        }
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
