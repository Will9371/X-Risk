using UnityEngine;
using TMPro;
using ZMD;
using ZMD.Dialog;

public class DisplayCitations : MonoBehaviour
{
    [SerializeField] DialogControllerMono dialog;
    [SerializeField] ItemListController list;
    [SerializeField] TMP_Text notification;
    [SerializeField] OpenClosePanel panel;
    
    void Start() 
    {
        dialog.onSetNode += SetNode;
        panel.onChangeOpen += RefreshTabText;
    }
    void OnDestroy() 
    { 
        if (dialog) dialog.onSetNode += SetNode; 
        if (panel) panel.onChangeOpen -= RefreshTabText;
    }
    
    void SetNode(DialogNode node)
    {
        list.Clear();
        citationCount = node.events.Length;
        gameObject.SetActive(citationCount > 0);
        
        foreach (var entry in node.events)
        {
            var citation = entry as Citation;
            if (citation == null) continue;
            var element = list.Create().GetComponent<CitationUI>();
            element.citation = citation;
        }
            
        RefreshTabText();
    }

    int citationCount;
    
    void RefreshTabText() => RefreshTabText(panel.isOpen);
    void RefreshTabText(bool isOpen)
    {
        var clickAction = panel.isOpen ? "(click to close)" : "(click to open)";
        notification.text = $"Citations: {citationCount} {clickAction}";        
    }
}
