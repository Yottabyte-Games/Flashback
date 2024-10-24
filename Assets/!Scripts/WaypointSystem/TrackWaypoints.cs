using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.WaypointSystem {
    public class TrackWaypoints : MonoBehaviour {
        // Events triggered when the player reaches the correct or wrong waypoint
        public event EventHandler OnPlayerCorrectWaypoint;
        public event EventHandler OnPlayerWrongWaypoint;

        // List of waypoints and indices for the next waypoint
        List<SingleWaypoint> waypointSingleList { get; set; }
        List<int> nextWaypointSingleIndexList { get; set; }
        int carIndex { get; set; }

        void Awake() {
            carIndex = 0;
            InitializeWaypoints();
            InitializeNextWaypointIndices();
        }
        void Start() {
            ShowFirstWaypoint();
        }

        #region Initialization-methods
        // Initialize the list of waypoints
        void InitializeWaypoints() {
            waypointSingleList = new List<SingleWaypoint>();
            foreach (Transform child in transform) {
                AddWaypoint(child);
            }
        }

        // Initialize the list of next waypoint indices
        void InitializeNextWaypointIndices() {
            nextWaypointSingleIndexList = new List<int> { 0 };
        }

        // Add a waypoint to the list
        void AddWaypoint(Transform child) {
            var singleWaypoint = child.GetComponent<SingleWaypoint>();
            if (singleWaypoint is null) return;
            singleWaypoint.SetTrackWaypoints(this);
            singleWaypoint.Hide();
            waypointSingleList.Add(singleWaypoint);
        }

        // Show the first waypoint
        void ShowFirstWaypoint() {
            if (waypointSingleList.Count > 0) {
                waypointSingleList[0].Show();
            }
        }
        #endregion

        #region Core logic
        // Called when a car goes through a waypoint
        public void CarThroughWaypoint(SingleWaypoint singleWaypoint, Transform carTransform) {
            var nextWaypointSingleIndex = nextWaypointSingleIndexList[carIndex];
            var currentWaypointIndex = waypointSingleList.IndexOf(singleWaypoint);
            Debug.Log($"Expected Waypoint Index: {nextWaypointSingleIndex}, Current Waypoint Index: {currentWaypointIndex}");

            if (!currentWaypointIndex.Equals(nextWaypointSingleIndex)) {
                HandleWrongWaypoint(nextWaypointSingleIndex);
            } else {
                HandleCorrectWaypoint(nextWaypointSingleIndex);
            }
        }
        
        
        // Handle the event when the car goes through the wrong waypoint
        void HandleWrongWaypoint(int nextWaypointSingleIndex) {
            Debug.Log("Wrong waypoint triggered");
            OnPlayerWrongWaypoint?.Invoke(this, EventArgs.Empty);
            ShowCorrectWaypoint(nextWaypointSingleIndex);
            ResetAllWaypoints();
        }

        // Handle the event when the car goes through the correct waypoint
        void HandleCorrectWaypoint(int nextWaypointSingleIndex) {
            Debug.Log("Correct waypoint triggered");
            OnPlayerCorrectWaypoint?.Invoke(this, EventArgs.Empty);
            HideCurrentWaypoint(nextWaypointSingleIndex);
            UpdateNextWaypointIndex();
            ShowNextWaypoint();
        }
        #endregion
        
        #region Helper-methods
        // Show the correct waypoint
        void ShowCorrectWaypoint(int nextWaypointSingleIndex) {
            var correctSingleWaypoint = waypointSingleList[nextWaypointSingleIndex];
            correctSingleWaypoint.Show();
            correctSingleWaypoint.ResetTrigger();
        }

        // Show the next waypoint
        void ShowNextWaypoint() {
            var nextWaypointSingle = waypointSingleList[nextWaypointSingleIndexList[carIndex]];
            nextWaypointSingle.Show();
        }

        // Update the index of the next waypoint
        void UpdateNextWaypointIndex() {
            nextWaypointSingleIndexList[carIndex] = (nextWaypointSingleIndexList[carIndex] + 1) % waypointSingleList.Count;
        }

        // Hide the current waypoint
        void HideCurrentWaypoint(int nextWaypointSingleIndex) {
            var correctSingleWaypoint = waypointSingleList[nextWaypointSingleIndex];
            correctSingleWaypoint.Hide();
        }

        // Reset all waypoints
        void ResetAllWaypoints() {
            foreach (var waypoint in waypointSingleList) {
                waypoint.ResetTrigger();
            }
        }
        #endregion
    }
}