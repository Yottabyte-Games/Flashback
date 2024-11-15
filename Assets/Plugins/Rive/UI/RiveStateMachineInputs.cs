using System;
using System.Collections.Generic;
using System.Linq;
using Rive;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Plugins.Rive.UI
{
// https://rive.app/community/doc/text/docn2E6y1lXo

//! An example implementation to get/set Rive State Machine Inputs.
    [RequireComponent(typeof(RiveScreen))]
    public class RiveStateMachineInputs : MonoBehaviour
    {
        [Serializable]
        public struct SmiTriggerDescriptor
        {
            public string name;
            public bool trigger;
            public SMITrigger Reference;

            public SmiTriggerDescriptor(string name, SMITrigger reference)
            {
                this.name = name;
                this.Reference = reference;
                trigger = false;
            }
        }

        [Serializable]
        public struct SmiBoolDescriptor
        {
            public string name;
            public bool value;
            public SMIBool Reference;

            public SmiBoolDescriptor(string name, bool value, SMIBool reference)
            {
                this.name = name;
                this.value = value;
                this.Reference = reference;
            }
        }

        [Serializable]
        public class SmiNumberDescriptor
        {
            public string name;
            public float value;
            public SMINumber Reference;

            public SmiNumberDescriptor(string name, float value, SMINumber reference)
            {
                this.name = name;
                this.value = value;
                this.Reference = reference;
            }
        }

        StateMachine _mRiveStateMachine;

        
        [SerializeField]
        public List<SmiTriggerDescriptor> triggers;
        [SerializeField]
        public List<SmiBoolDescriptor> booleans;
        [SerializeField]
        public List<SmiNumberDescriptor> numbers;

        void Start()
        {
            
            var riveScreen = GetComponent<RiveScreen>();
            _mRiveStateMachine = riveScreen.stateMachine;

            booleans = new List<SmiBoolDescriptor>();
            triggers = new List<SmiTriggerDescriptor>();
            numbers = new List<SmiNumberDescriptor>();

            
            var inputs = _mRiveStateMachine.Inputs();
            foreach (var input in inputs)
            {
                switch (input)
                {
                    case SMITrigger smiTrigger:
                    {
                        var descriptor = new SmiTriggerDescriptor(smiTrigger.Name, smiTrigger);
                        triggers.Add(descriptor);
                        break;
                    }
                    case SMIBool smiBool:
                    {
                        var descriptor = new SmiBoolDescriptor(smiBool.Name, smiBool.Value, smiBool);
                        booleans.Add(descriptor);
                        break;
                    }
                    case SMINumber smiNumber:
                    {
                        var descriptor = new SmiNumberDescriptor(smiNumber.Name, smiNumber.Value, smiNumber);
                        numbers.Add(descriptor);
                        break;
                    }
                }
            }
        }

        void OnValidate()
        {
            // State machine triggers
            var triggerDidChange = false;
            foreach (var inspectorInput in triggers.Where(inspectorInput => inspectorInput.Reference is not null)
                         .Where(inspectorInput => inspectorInput.trigger))
            {
                inspectorInput.Reference.Fire();
                triggerDidChange = true;
            }

            if (triggerDidChange)
            {
                var updatedTriggers = triggers.Select(inspectorInput => new SmiTriggerDescriptor(inspectorInput.name, inspectorInput.Reference)).ToList();
                triggers = updatedTriggers;
            }

            // State machine booleans
            foreach (var inspectorInput in booleans.Where(inspectorInput => inspectorInput.Reference is not null)
                         .Where(inspectorInput => inspectorInput.value != inspectorInput.Reference.Value))
            {
                inspectorInput.Reference.Value = inspectorInput.value;
            }

            // State machine numbers
            foreach (var inspectorInput in numbers.Where(inspectorInput => inspectorInput.Reference is not null)
                         .Where(inspectorInput => !Mathf.Approximately(inspectorInput.value, inspectorInput.Reference.Value)))
            {
                inspectorInput.Reference.Value = inspectorInput.value;
            }
        }

    }

#if UNITY_EDITOR
// Creates a custom Label on the inspector.
// This also solves this issue when exiting play mode: https://forum.unity.com/threads/nullreferenceexception-serializedobject-of-serializedproperty-has-been-disposed.1431907/
    [CustomEditor(typeof(RiveStateMachineInputs))]
    public class TestOnInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                base.OnInspectorGUI();
            }
            GUILayout.Label ("Enter Play Mode to interact with available state machine inputs");
        }
    }
#endif
}