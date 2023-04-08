using System;
using UnityEngine;
using UnityEngine.Events;

namespace ZMD.Dialog
{
    [Serializable] class DialogNodeEvent : UnityEvent<DialogNode> { }

    public class DialogControllerMono : MonoBehaviour
    {
        public void Begin(DialogNode startingNode) => process.Begin(startingNode);
        public void DisplayOptions() => process.DisplayOptions();
        public DialogController process;
        
        void Awake() => process.onSetNode = OnSetNode;
        void OnSetNode(DialogNode value) => RelayNode.Invoke(value);

        [SerializeField] DialogNodeEvent RelayNode;
        [SerializeField] UnityEvent goBack;
        
        public void GoBack() 
        {
            process.GoBack();
            goBack.Invoke();
        }
    }
    
    [Serializable]
    public class DialogController
    {
        //NarrativeHub narrative => NarrativeHub.instance;
    
        [SerializeField] DialogNode ending;
        [SerializeField] DialogNode error;
        [SerializeField] ResponseButtons response;
        
        [SerializeField, ReadOnly]
        DialogNode _node;
        DialogNode node
        {
            get => _node;
            set
            {
                _node = value;
                onSetNode?.Invoke(value);
            }
        }
        public Action<DialogNode> onSetNode;
        
        bool inProgress;

        public void Begin(DialogNode startingNode)
        {
            if (inProgress) return;
            
            Initialize();
            node = startingNode;
            history.Add(startingNode);

            inProgress = true;
        }

        bool initialized;
        void Initialize()
        {
            if (initialized) return;
            response.onClick = Transition;
            initialized = true;
        }
        
        public Action<SO> onTriggerEvent;
        public Action onEventsComplete;
        public Action<OccasionInfo> onOccasion;
        
        void Transition(int index)
        {
            if (index >= node.responses.Length)
                return;
        
            var newNode = node.GetNode(index);
            if (newNode == null)
            {
                error.narrative = $"{node.name} missing response at index {index}";
                newNode = error;
            }
            node = newNode;
            history.Add(newNode);
            //foreach (var character in node.castChanges)
            //    narrative.SetActorImageActive(character.actor, character.active);
            
            ApplyEvents();
        }

        /// Events, Occasions, and Ending
        void ApplyEvents()
        {
            // UnityEvent based
            foreach (var item in node.events)
            {
                onTriggerEvent?.Invoke(item);
                //narrative.onOccasion?.Invoke();  // ???
            }

            // SO/C# event based
            foreach (var occasion in node.occasions)
            {
                occasion.Trigger();
                //narrative.MakeDecision(occasion);
                onOccasion?.Invoke(occasion);
            }
            
            if (node.occasions.Length > 0 || node.events.Length > 0)
                onEventsComplete?.Invoke();  
                
            if (node == ending)
                EndConversation();          
        }
        
        public Action onEndConversation;
        
        void EndConversation()
        {
            inProgress = false;
            //narrative.onEndConversation?.Invoke();
            onEndConversation?.Invoke(); 
        }
        
        public void DisplayOptions() => response.Refresh(node);
        
        [SerializeField]
        DialogHistory history = new();
        
        public void GoBack()
        {
            var newNode = history.GoBack();
            if (newNode == null) return;
            node = newNode;
        }
    }
}
