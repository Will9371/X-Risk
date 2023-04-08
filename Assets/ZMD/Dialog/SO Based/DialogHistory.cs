using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZMD.Dialog
{
    [Serializable]
    public class DialogHistory
    {
        [SerializeField] Button backButton;
        void RefreshButton() => backButton.gameObject.SetActive(backAvaialble);
        bool backAvaialble => history.Count >= 2;

        [SerializeField, ReadOnly] 
        List<DialogNode> history = new();
        
        public void Add(DialogNode newNode) 
        {
            history.Add(newNode);
            RefreshButton();
        }
        
        public DialogNode GoBack()
        {
            if (!backAvaialble) return null;
            history.RemoveAt(history.Count - 1);
            RefreshButton();
            return history[history.Count - 1];
        }
    }
}
