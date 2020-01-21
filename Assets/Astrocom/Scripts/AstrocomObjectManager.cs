using System.Collections;
using System.Collections.Generic;
using Astrocom.Scripts.ARCoreScripts.ManipulationSystem;
using UnityEngine;

public class AstrocomObjectManager : MonoBehaviour
{
    public delegate void AstrocomObjectSpawnedHandler(AstrocomObject spawnedAstrocomObject);
    public event AstrocomObjectSpawnedHandler AstrocomObjectSpawned;

    public delegate void AstrocomObjectSelectedHandler(AstrocomObject selectedAstrocomObject);
    public event AstrocomObjectSelectedHandler AstrocomObjectSelected;

    public delegate void AstrocomObjectDeselectedHandler(AstrocomObject deselectedAstrocomObject);
    public event AstrocomObjectDeselectedHandler AstrocomObjectDeselected;

    public delegate void AstrocomObjectRemovedHandler(AstrocomObject removedAstrocomObject);
    public event AstrocomObjectRemovedHandler AstrocomObjectRemoved;

    private ObjectManipulator objectManipulator;

    private void Start() 
    {
        ManipulationSystem.Instance.ObjectRemoved += OnObjectRemoved;
        objectManipulator = FindObjectOfType<ObjectManipulator>();

        if(objectManipulator != null)
            objectManipulator.ObjectSpawned += OnObjectSpawned;
        else
            Debug.LogWarning("Object manipulator not found in scene");
    }

    public void OnObjectSpawned(GameObject spawnedObject)
    {
        AstrocomObject astrocomSpawnedObject = spawnedObject.GetComponentInChildren<AstrocomObject>();
        
        if(astrocomSpawnedObject != null)
            AstrocomObjectSelected(astrocomSpawnedObject);
    }

    public void OnObjectSelected(GameObject selectedObject)
    {
        AstrocomObject astrocomSelectedObject = selectedObject.GetComponentInChildren<AstrocomObject>();
        
        if(astrocomSelectedObject != null)
            AstrocomObjectSelected(astrocomSelectedObject);
    }

    public void OnObjectDeselected(GameObject deselectedObject)
    {
        AstrocomObject astrocomDeselectedObject = deselectedObject.GetComponentInChildren<AstrocomObject>();
        
        if(astrocomDeselectedObject != null)
            AstrocomObjectDeselected(astrocomDeselectedObject);
    }

    public void OnObjectRemoved(GameObject removedObject)
    {
        AstrocomObject astrocomRemovedObject = removedObject.GetComponentInChildren<AstrocomObject>();
        
        if(astrocomRemovedObject != null)
            AstrocomObjectRemoved(astrocomRemovedObject);
    }
}
