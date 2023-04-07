using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZMD.Dialog
{
    public class ResponseButtons : MonoBehaviour
    {
        [SerializeField] ResponseButton[] responses;
        [SerializeField] ButtonSettings[] buttonSettings;
        
        void Start()
        {
            foreach (var response in responses)
                response.onClick = OnClick;
            
            SetAllInactive();
        }

        void OnClick(int id) 
        {
            SetAllInactive();
            onClick?.Invoke(id); 
        }
        public Action<int> onClick;
        
        void SetAllInactive()
        {
            foreach (var response in responses)
                response.gameObject.SetActive(false);
        }

        public void Refresh(DialogNode node)
        {
            List<int> validResponses = node.GetValidResponses();
            
            if (validResponses.Count == 0)
            {
                SetAllInactive();
                Debug.LogError($"{node.name} has no valid responses");
                return;
            }

            var buttonSetting = buttonSettings[validResponses.Count - 1];
            for (int i = 0; i < buttonSetting.anchoredPositions.Length; i++)
                responses[i].SetPosition(buttonSetting.anchoredPositions[i]);
            
            foreach (var index in validResponses)
                responses[index].Activate(node.GetResponse(index));
        }
    }
    
    [Serializable]
    public struct ButtonSettings
    {
        public Vector2[] anchoredPositions;
    }
}
