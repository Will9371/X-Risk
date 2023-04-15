using System;
using UnityEngine;
using TMPro;

public class CitationUI : MonoBehaviour
{
    [SerializeField] TMP_Text tmpText;

    Citation _citation;
    public Citation citation
    {
        get => _citation;
        set
        {
            _citation = value;
            tmpText.text = value.message;
        }
    }

    public void OnClick() => Application.OpenURL(citation.hyperlink);
}
