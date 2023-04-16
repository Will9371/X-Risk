using System;
using UnityEngine;

public class OpenClosePanel : MonoBehaviour
{
    [SerializeField] RectTransform box;
    [SerializeField] RectTransform list;
    [SerializeField] float moveSpeed = 1f;

    bool _isOpen;
    public bool isOpen
    {
        get => _isOpen;
        set
        {
            _isOpen = value;
            onChangeOpen?.Invoke(value);
        }
    }
    public Action<bool> onChangeOpen;
    
    float height 
    { 
        get => box.anchoredPosition.y;
        set => box.anchoredPosition = new Vector2(0, value);
    }
    float targetHeight => isOpen ? list.sizeDelta.y : 0;

    public void Toggle() => isOpen = !isOpen;
    
    void Update()
    {
        height = Mathf.MoveTowards(height, targetHeight, Time.deltaTime * moveSpeed);
    }
}
