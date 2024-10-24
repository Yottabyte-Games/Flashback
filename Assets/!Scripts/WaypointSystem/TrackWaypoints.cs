using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.WaypointSystem
{
    public class TrackWaypoints : MonoBehaviour {

        public event EventHandler OnPlayerCorrectWaypoint;
        public event EventHandler OnPlayerWrongWaypoint;

        //TODO: If we are implementing multiple cars, we need to update this and a list of transform for each car instead
        int _carIndex;
        List<SingleWaypoint> _waypointSingleList;
        List<int> _nextWaypointSingleIndexList;

        void Awake() {
            // Initialize the list of waypoints
            _waypointSingleList = new List<SingleWaypoint>();
            foreach (Transform child in transform) {
                var singleWaypoint = child.GetComponent<SingleWaypoint>();
                if (singleWaypoint is null) continue;
                singleWaypoint.SetTrackWaypoints(this);
                singleWaypoint.Hide(); // Hide all waypoints initially
                _waypointSingleList.Add(singleWaypoint);
            }

            // Initialize the list of next waypoint indices for each car
            _nextWaypointSingleIndexList = new List<int> { 0 }; // Assuming one car for simplicity
        }

        void Start()
        {
            // Show the first waypoint if there are any waypoints
            if (_waypointSingleList.Count <= 0) return;
            _waypointSingleList[0].Show();
        }

        public void CarThroughWaypoint(SingleWaypoint singleWaypoint, Transform carTransform) {
            // Get the next waypoint index for the current car
            var nextWaypointSingleIndex = _nextWaypointSingleIndexList[_carIndex];
            var currentWaypointIndex = _waypointSingleList.IndexOf(singleWaypoint);
            Debug.Log($"Expected Waypoint Index: {nextWaypointSingleIndex}, Current Waypoint Index: {currentWaypointIndex}");

            if (currentWaypointIndex != nextWaypointSingleIndex)
            {
                // Wrong waypoint
                Debug.Log("Wrong waypoint triggered");
                OnPlayerWrongWaypoint?.Invoke(this, EventArgs.Empty);

                // Show the correct waypoint
                var correctSingleWaypoint = _waypointSingleList[nextWaypointSingleIndex];
                correctSingleWaypoint.Show();
                correctSingleWaypoint.ResetTrigger(); // Reset the trigger flag

                // Reset all waypoints to ensure they are interactable
                foreach (var waypoint in _waypointSingleList)
                {
                    waypoint.ResetTrigger();
                }
            }
            else
            {
                // Correct waypoint 
                Debug.Log("Correct waypoint triggered");
                OnPlayerCorrectWaypoint?.Invoke(this, EventArgs.Empty);

                // Hide the current waypoint
                var correctSingleWaypoint = _waypointSingleList[nextWaypointSingleIndex];
                correctSingleWaypoint.Hide();

                // Update the next waypoint index for the current car. Using modulo to loop back to the first waypoint.
                _nextWaypointSingleIndexList[_carIndex] = (nextWaypointSingleIndex + 1) % _waypointSingleList.Count;

                // Show the next waypoint 
                var nextWaypointSingle = _waypointSingleList[_nextWaypointSingleIndexList[_carIndex]];
                nextWaypointSingle.Show();
            }
        }
    }
}