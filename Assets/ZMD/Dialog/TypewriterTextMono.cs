using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace ZMD
{
    public class TypewriterTextMono : MonoBehaviour
    {
        public TypewriterText process;
        public void Input(string value) => process.Input(this, value);
        public void AutoComplete() => process.AutoComplete();
        
        void Start() => process.onComplete = OnComplete;
        void OnComplete() => onComplete.Invoke();
        [SerializeField] UnityEvent onComplete;
    }
    
    [Serializable]
    public class TypewriterText
    {
        [SerializeField] TMP_Text text;
        [Tooltip("In characters per second")]
        [SerializeField] float speed = 10;
        
        public Action onComplete;
        
        MonoBehaviour mono;
        IEnumerator typeRoutine;
        bool inProgress;

        public void Input(MonoBehaviour mono, string value) 
        {
            this.mono = mono;
            typeRoutine = TypeText(value);
            mono.StartCoroutine(typeRoutine);
        }
        
        public IEnumerator TypeText(string value)
        {
            if (string.IsNullOrEmpty(value))
                yield break;
        
            inProgress = true;
            text.text = value;

            for (int i = 0; i <= value.Length; i++)
            {
                text.maxVisibleCharacters = i;
                yield return new WaitForSeconds(1/speed);
            }
            
            onComplete?.Invoke();
            inProgress = false;
        }
        
        public void AutoComplete()
        {
            if (!inProgress) return;
            mono.StopCoroutine(typeRoutine);
            text.maxVisibleCharacters = text.text.Length;
            onComplete?.Invoke();
            inProgress = false;
        }
    }
}
