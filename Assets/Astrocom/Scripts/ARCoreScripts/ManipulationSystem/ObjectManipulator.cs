//-----------------------------------------------------------------------
// <copyright file="PawnManipulator.cs" company="Google">
//
// Copyright 2019 Google LLC All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace Astrocom.Scripts.ARCoreScripts.ManipulationSystem
{
    using GoogleARCore;
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Controls the placement of objects via a tap gesture.
    /// </summary>
    public class ObjectManipulator : Manipulator
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// Manipulator prefab to attach placed objects to.
        /// </summary>
        public GameObject ManipulatorPrefab;

        //TODO: Change GameObject to abstraction with other properties
        // [SerializeField]
        // private List<AstrocomObject> ObjectsToSpawn;

        [SerializeField]
        private List<GameObject> ObjectsToSpawn;

        public delegate void ObjectSpawnedEventHandler(GameObject spawnedObject);
        public event ObjectSpawnedEventHandler ObjectSpawned;

        public delegate void ObjectSelectedEventHandler(GameObject selectedObject);
        public event ObjectSelectedEventHandler ObjectSelected;

        public int SelectedPrefab { get; set; } = -1;

        /// <summary>
        /// Assign selected prefab property when button pressed
        /// </summary>
        /// <param name="prefabNumber">Chosen prefab by pressed button</param>
        public void ChoosePrefab(int prefabNumber)
        {
            SelectedPrefab = prefabNumber;
            Debug.Log($"Prefab №{ prefabNumber } is selected to spawn");
        }

        private int _currentCountOfObjects;
        public int CurrentCountOfObjects
        {
            get => _currentCountOfObjects; 
            set
            {
                _currentCountOfObjects = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }

        public List<GameObject> ObjectsToDisable { get; set; }


        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
                return true;

            return false;
        }

        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
                return;

            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
                return;


            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(
                gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Instantiate game object at the hit pose.
                    var gameObject = Instantiate(ObjectsToSpawn[SelectedPrefab], hit.Pose.position, hit.Pose.rotation);
                    OnObjectSpawned(gameObject);

                    // Instantiate manipulator.
                    var manipulator =
                        Instantiate(ManipulatorPrefab, hit.Pose.position, hit.Pose.rotation);

                    // Make game object a child of the manipulator.
                    gameObject.transform.parent = manipulator.transform;
                    gameObject.transform.SetAsFirstSibling();

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make manipulator a child of the anchor.
                    manipulator.transform.parent = anchor.transform;

                    // Select the placed object.
                    manipulator.GetComponent<Manipulator>().Select();

                    SelectedPrefab = -1;
                    CurrentCountOfObjects += 1;
                }
            }
        }

        protected virtual void OnObjectSpawned(GameObject spawnedObject)
        {
            if(ObjectSpawned != null)
                ObjectSpawned(spawnedObject);
        }
    }
}
