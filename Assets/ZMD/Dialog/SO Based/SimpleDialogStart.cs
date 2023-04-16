using UnityEngine;

namespace ZMD.Dialog
{
    public class SimpleDialogStart : MonoBehaviour
    {
        [SerializeField] DialogNode startingNode;
        [SerializeField] DialogNodeEvent onStart;
        void Start() => onStart.Invoke(startingNode);
    }
}