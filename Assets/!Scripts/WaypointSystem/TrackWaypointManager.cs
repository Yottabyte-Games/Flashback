using System;
using System.Linq;
using System.Collections.Generic;
using EventHandler = System.EventHandler;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

namespace _Scripts.WaypointSystem {
    public class TrackWaypointManager : MonoBehaviour {
        #region Declarations
        [SerializeField] SceneReference sceneToLoad;
        [SerializeField] EventReference winConditionFmodEvent;
        [SerializeField] int waypointsToShow = 2;

        public List<SingleWaypoint> WaypointSingleList { get; set; }
        public List<int> NextWaypointSingleIndexList { get; set; }
        public int CarIndex { get; set; }

        public event EventHandler OnPlayerCorrectWaypoint;
        public event EventHandler OnPlayerWrongWaypoint;
        #endregion

        #region Unity Methods
        void Awake() {
            CarIndex = 0;
            InitializeWaypoints();
            InitializeNextWaypointIndices();
        }

        void Start() {
            ShowFirstWaypoints();
        }
        #endregion

        #region Initialization
        void InitializeWaypoints() {
            WaypointSingleList = new List<SingleWaypoint>();
            foreach (Transform child in transform) {
                AddWaypoint(child);
            }
        }

        void InitializeNextWaypointIndices() {
            NextWaypointSingleIndexList = new List<int> { 0 };
        }

        void AddWaypoint(Transform child) {
            var singleWaypoint = child.GetComponent<SingleWaypoint>();
            if (singleWaypoint is null) return;
            singleWaypoint.SetTrackWaypoints(this);
            singleWaypoint.Hide();
            WaypointSingleList.Add(singleWaypoint);
        }
        #endregion

        #region Waypoint Handling
        void ShowFirstWaypoints() {
            for (int i = 0; i < waypointsToShow && i < WaypointSingleList.Count; i++) {
                WaypointSingleList[i].Show();
            }
        }

        public void CarThroughWaypoint(SingleWaypoint singleWaypoint, Transform carTransform) {
            int nextWaypointSingleIndex = NextWaypointSingleIndexList[CarIndex];
            int currentWaypointIndex = WaypointSingleList.IndexOf(singleWaypoint);

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
            if (NextWaypointSingleIndexList[CarIndex] == 0) {
                WinConditionReached();
            } else {
                ShowNextWaypoints();
            }
        }
        #endregion

        #region Helper Methods
        void ShowCorrectWaypoint(int nextWaypointSingleIndex) {
            var correctSingleWaypoint = WaypointSingleList[nextWaypointSingleIndex];
            correctSingleWaypoint.Show();
            correctSingleWaypoint.ResetTrigger();
        }

        void ShowNextWaypoints() {
            int nextIndex = NextWaypointSingleIndexList[CarIndex];
            for (int i = waypointsToShow - 1; i >= 0; i--) {
                int index = (nextIndex + i) % WaypointSingleList.Count;
                WaypointSingleList[index].Show();
            }
        }

        void HideCurrentWaypoint(int nextWaypointSingleIndex) {
            var correctSingleWaypoint = WaypointSingleList[nextWaypointSingleIndex];
            correctSingleWaypoint.Hide();
        }

        void ResetAllWaypoints() {
            foreach (var waypoint in WaypointSingleList) {
                waypoint.ResetTrigger();
            }
        }

        void UpdateNextWaypointIndex() {
            NextWaypointSingleIndexList[CarIndex] = (NextWaypointSingleIndexList[CarIndex] + 1) % WaypointSingleList.Count;
        }

        void WinConditionReached() {
            Debug.Log("Win Condition Reached!");
            RuntimeManager.PlayOneShot(winConditionFmodEvent, transform.position);
            SceneManager.LoadScene(sceneToLoad.Name);
        }
        #endregion
    }
}