using UnityEngine;

namespace ZMD.Dialog
{
    public class SimpleDialogStart : MonoBehaviour
    {
        [SerializeField] DialogControllerMono dialog;
        [SerializeField] DialogNode startingNode;
        void Start() => dialog.Begin(startingNode);
    }
}
