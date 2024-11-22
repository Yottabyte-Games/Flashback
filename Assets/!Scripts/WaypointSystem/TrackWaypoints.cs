using System;
using System.Linq;
using System.Collections.Generic;
using EventHandler = System.EventHandler;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;


namespace _Scripts.WaypointSystem {
    public class TrackWaypoints : MonoBehaviour {
        #region Declarations
        [SerializeField] SceneReference sceneToLoad;
        [SerializeField] EventReference winConditionFmodEvent;

        List<SingleWaypoint> WaypointSingleList { get; set; }
        List<int> NextWaypointSingleIndexList { get; set; }
        int CarIndex { get; set; }

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
            ShowFirstWaypoint();
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
        void ShowFirstWaypoint() {
            WaypointSingleList.FirstOrDefault()?.Show();
        }

        public void CarThroughWaypoint(SingleWaypoint singleWaypoint, Transform carTransform) {
            var nextWaypointSingleIndex = NextWaypointSingleIndexList[CarIndex];
            var currentWaypointIndex = WaypointSingleList.IndexOf(singleWaypoint);

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
                ShowNextWaypoint();
            }
        }
        #endregion

        #region Helper Methods
        void ShowCorrectWaypoint(int nextWaypointSingleIndex) {
            var correctSingleWaypoint = WaypointSingleList[nextWaypointSingleIndex];
            correctSingleWaypoint.Show();
            correctSingleWaypoint.ResetTrigger();
        }

        void ShowNextWaypoint() {
            var nextWaypointSingle = WaypointSingleList[NextWaypointSingleIndexList[CarIndex]];
            nextWaypointSingle.Show();
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