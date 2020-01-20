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
    private TextMeshProUGUI NameOfObjectText;
    [SerializeField]
    private GameObject StandartUI;
    [SerializeField]
    private GameObject RemoveButton;

    void Start()
    {
        MainCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        StandartUI.SetActive(false);
        RemoveButton.SetActive(false);
        ManipulationSystem.Instance.ObjectRemoved += OnObjectRemoved;
    }

    //FIXME: Adjust ui by Astrocom object specs
    public void OnObjectSelected(GameObject selectedObject)
    {
        RemoveButton.SetActive(true);
        AstrocomObject astrocomSelectedObject = selectedObject.GetComponentInChildren<AstrocomObject>();
        
        if(astrocomSelectedObject != null)
            ChangeUiIfSelectedNewObject(astrocomSelectedObject);
        else
            Debug.LogWarning("AstrocomObject not found on selected object");
    }

    private void ChangeUiIfSelectedNewObject(AstrocomObject selectedAstrocomObject)
    {
        var individualUiObject = selectedAstrocomObject.IndividualUiObject;
        var instantiatedUi = selectedAstrocomObject.InstantiatedUi;
        var onlyIndividualUi = selectedAstrocomObject.OnlyIndividualUi;

        if(individualUiObject != null && instantiatedUi == null)
        {
            GameObject tempInstantiatedUi = Instantiate(selectedAstrocomObject.IndividualUiObject);
            tempInstantiatedUi.transform.SetParent(MainCanvas.transform, false);
            tempInstantiatedUi.SetActive(true);
            selectedAstrocomObject.InstantiatedUi = tempInstantiatedUi;
        }
        
        else if(instantiatedUi != null && onlyIndividualUi)
        {
            selectedAstrocomObject.InstantiatedUi.SetActive(true);
            StandartUI.SetActive(false);
        }
            
        else if(instantiatedUi == null && !onlyIndividualUi)
            StandartUI.SetActive(true);

        else if(instantiatedUi != null && !onlyIndividualUi)
        {
            selectedAstrocomObject.InstantiatedUi.SetActive(true);
            StandartUI.SetActive(true);
        }
        else if(instantiatedUi == null && onlyIndividualUi)
            StandartUI.SetActive(false);
    }

    public void OnObjectDeselected(GameObject deselectedObject)
    {
        RemoveButton.SetActive(false);
        AstrocomObject astrocomDeselectedObject = deselectedObject.GetComponentInChildren<AstrocomObject>();

        if(astrocomDeselectedObject != null)
            ChangeUiIfObjectDeselected(astrocomDeselectedObject);
        else
            Debug.LogWarning("AstrocomObject not found on deselected object");
    }

    private void ChangeUiIfObjectDeselected(AstrocomObject astrocomDeselectedObject)
    {
        if(astrocomDeselectedObject.InstantiatedUi == null)
            StandartUI.SetActive(false);

        else
        {
            astrocomDeselectedObject.InstantiatedUi.SetActive(false);
            StandartUI.SetActive(false);
        }
    }

    public void OnObjectRemoved(GameObject removedObject)
    {
        RemoveButton.SetActive(false);
        AstrocomObject astrocomDeletedObject = removedObject.GetComponentInChildren<AstrocomObject>();
        
        NameOfObjectText.SetText("Object deleted");
        if(astrocomDeletedObject != null)
            ChangeUiIfObjectRemoved(astrocomDeletedObject);
        else
            Debug.LogWarning("AstrocomObject not found on deleted object");
    }

    private void ChangeUiIfObjectRemoved(AstrocomObject removedAstrocomObject)
    {
        NameOfObjectText.SetText("Object deleted");
        if(removedAstrocomObject.InstantiatedUi == null)
            StandartUI.SetActive(false);

        else
        {
            removedAstrocomObject.InstantiatedUi.SetActive(false);
            Destroy(removedAstrocomObject.InstantiatedUi);
            removedAstrocomObject.InstantiatedUi = null;
            StandartUI.SetActive(false);
        }
    }
    
    public void ExitApplication()
    {
        Application.Quit();
    }
}
