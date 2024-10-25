using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.WaypointSystem {
    public class TrackWaypoints : MonoBehaviour {
        #region Declarations
        public event EventHandler OnPlayerCorrectWaypoint;
        public event EventHandler OnPlayerWrongWaypoint;
        List<SingleWaypoint> waypointSingleList { get; set; }
        List<int> nextWaypointSingleIndexList { get; set; }
        int carIndex { get; set; }
        #endregion

        #region Unity Methods
        void Awake() {
            carIndex = 0;
            InitializeWaypoints();
            InitializeNextWaypointIndices();
        }

        void Start() {
            ShowFirstWaypoint();
        }
        #endregion

        #region Initialization
        void InitializeWaypoints() {
            waypointSingleList = new List<SingleWaypoint>();
            foreach (Transform child in transform) {
                AddWaypoint(child);
            }
        }

        void InitializeNextWaypointIndices() {
            nextWaypointSingleIndexList = new List<int> { 0 };
        }

        void AddWaypoint(Transform child) {
            var singleWaypoint = child.GetComponent<SingleWaypoint>();
            if (singleWaypoint is null) return;
            singleWaypoint.SetTrackWaypoints(this);
            singleWaypoint.Hide();
            waypointSingleList.Add(singleWaypoint);
        }
        #endregion

        #region Waypoint Handling
        void ShowFirstWaypoint() {
            waypointSingleList.FirstOrDefault()?.Show();
        }

        public void CarThroughWaypoint(SingleWaypoint singleWaypoint, Transform carTransform) {
            var nextWaypointSingleIndex = nextWaypointSingleIndexList[carIndex];
            var currentWaypointIndex = waypointSingleList.IndexOf(singleWaypoint);

            if (!currentWaypointIndex.Equals(nextWaypointSingleIndex)) {
                HandleWrongWaypoint(nextWaypointSingleIndex);
            } else {
                HandleCorrectWaypoint(nextWaypointSingleIndex);
            }
        }

        void HandleWrongWaypoint(int nextWaypointSingleIndex) {
            OnPlayerWrongWaypoint?.Invoke(this, EventArgs.Empty);
            ShowCorrectWaypoint(nextWaypointSingleIndex);
            ResetAllWaypoints();
        }

        void HandleCorrectWaypoint(int nextWaypointSingleIndex) {
            OnPlayerCorrectWaypoint?.Invoke(this, EventArgs.Empty);
            HideCurrentWaypoint(nextWaypointSingleIndex);
            UpdateNextWaypointIndex();
            ShowNextWaypoint();
        }
        #endregion

        #region Helper Methods
        void ShowCorrectWaypoint(int nextWaypointSingleIndex) {
            var correctSingleWaypoint = waypointSingleList[nextWaypointSingleIndex];
            correctSingleWaypoint.Show();
            correctSingleWaypoint.ResetTrigger();
        }

        void ShowNextWaypoint() {
            var nextWaypointSingle = waypointSingleList[nextWaypointSingleIndexList[carIndex]];
            nextWaypointSingle.Show();
        }

        void HideCurrentWaypoint(int nextWaypointSingleIndex) {
            var correctSingleWaypoint = waypointSingleList[nextWaypointSingleIndex];
            correctSingleWaypoint.Hide();
        }
        
        void ResetAllWaypoints() {
            foreach (var waypoint in waypointSingleList) {
                waypoint.ResetTrigger();
            }
        }
        void UpdateNextWaypointIndex() {
            nextWaypointSingleIndexList[carIndex] = (nextWaypointSingleIndexList[carIndex] + 1) % waypointSingleList.Count;
        }
        #endregion
    }
}