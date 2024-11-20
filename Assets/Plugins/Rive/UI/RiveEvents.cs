using Rive;
using UnityEngine;

//! An example implementation to access Rive Events.

namespace Plugins.Rive.UI
{
    [RequireComponent(typeof(RiveScreen))]
    public class RiveEvents : MonoBehaviour
    {
        RiveScreen _mRiveScreen;
        void Start()
        {
            _mRiveScreen = GetComponent<RiveScreen>();
            _mRiveScreen.OnRiveEvent += RiveScreen_OnRiveEvent;
        }

        void RiveScreen_OnRiveEvent(ReportedEvent reportedEvent)
        {
            Debug.Log($"Event received, name: \"{reportedEvent.Name}\", secondsDelay: {reportedEvent.SecondsDelay}");

            // Access specific event properties
            if (reportedEvent.Name.StartsWith("rating"))
            {
                var rating = reportedEvent["rating"];
                var message = reportedEvent["message"];
                Debug.Log($"Rating: {rating}");
                Debug.Log($"Message: {message}");
            }
        }

        void OnDisable()
        {
            _mRiveScreen.OnRiveEvent -= RiveScreen_OnRiveEvent;
        }

    }
}
