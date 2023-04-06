using System;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace ZMD.Dialog
{
    [CreateAssetMenu(menuName = "ZMD/Dialog/Node", fileName = "Dialog Node")]
    public class DialogNode : ScriptableObject
    {
        [Tooltip("Main text associated with this node")]
        public string narrative;
        
        public bool showConditionalResponses;
        
        [Tooltip("Options available to player in response to narrative")]
        public ResponseOptions[] responses;
        
        [Tooltip("Change which actors are on screen")]
        public CastChange[] castChanges;

        [Tooltip("Trigger change in actors' opinions of each other")]
        public OccasionInfo[] occasions;

        [Tooltip("Tied to actors.  Trigger miscellaneous events beyond the scope of the system's features.")]
        public SO[] events;
        
        public string GetResponse(int index) => responses[index].GetResponse();
        public DialogNode GetNode(int index) => responses[index].GetNode();
        
        public List<int> GetValidResponses()
        {
            List<int> result = new();
                
            for (int i = 0; i < responses.Length; i++)
            {
                var (requirementsMet, hasFallback) = responses[i].RequirementsMet();
                if (requirementsMet || hasFallback)
                    result.Add(i);
            }
                        
            return result;
        }
        
        void OnValidate()
        {
            for (int i = 0; i < responses.Length; i++)
                responses[i].showConditionalResponses = showConditionalResponses;
        }
        
        [Serializable] 
        public struct ResponseOptions
        {
            List<OccasionInfo> playerDecisions => Redirect.playerDecisions;
            
            [Tooltip("Next node if this option is selected")]
            public DialogNode node;
            
            [Tooltip("Button text")]
            public string response;
            
            [HideInInspector]
            public bool showConditionalResponses;
            
            [ConditionalField(nameof(showConditionalResponses))]
            public OccasionRequirement logicalRequirement;
            [ConditionalField(nameof(showConditionalResponses))]
            public RelationalRequirement relationalRequirement;
            public (bool, bool) RequirementsMet() => 
                (!showConditionalResponses || 
                logicalRequirement.IsMet(playerDecisions) && relationalRequirement.IsMet(), 
                fallbackNode != null);
            
            [ConditionalField(nameof(showConditionalResponses))]
            public DialogNode fallbackNode;
            [ConditionalField(nameof(showConditionalResponses))]
            public string fallbackResponse;
            
            public string GetResponse()
            {
                var (requirementsMet, hasFallback) = RequirementsMet();
                if (requirementsMet) return response;
                if (hasFallback) return fallbackResponse;
                Debug.LogError("Node requirements not met and no fallback");
                return "";
            }
            
            public DialogNode GetNode()
            {
                var (requirementsMet, hasFallback) = RequirementsMet();
                if (requirementsMet) return node;
                if (hasFallback) return fallbackNode;
                Debug.Log("Node requirements not met and no fallback");
                return null;
            }
        }
        
        [Serializable]
        public struct CastChange
        {
            public ActorInfo actor;
            public bool active;
        }
    }
}
