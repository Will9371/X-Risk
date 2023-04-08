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
            SetAllInactive();
            List<int> validResponses = node.GetValidResponses();
            
            if (validResponses.Count == 0)
            {
                SetAllInactive();
                return;
            }

            var buttonSetting = buttonSettings[validResponses.Count - 1];
            //Debug.Log($"{validResponses.Count - 1}, {buttonSetting.anchoredPositions.Length}, {responses.Length}");
            for (int i = 0; i < buttonSetting.anchoredPositions.Length; i++)
            {
                responses[i].SetPosition(buttonSetting.anchoredPositions[i]);
                responses[i].SetSize(buttonSetting.boxSize);
            }
            
            foreach (var index in validResponses)
                responses[index].Activate(node.GetResponse(index));
        }
        
        void OnValidate()
        {
            for (int i = 0; i < responses.Length; i++)
                responses[i].responseId = i;
        }
    }

    [Serializable]
    public struct ButtonSettings
    {
        public Vector2[] anchoredPositions;
        public Vector2 boxSize;
    }
}
