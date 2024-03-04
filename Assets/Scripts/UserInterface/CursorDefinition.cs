using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace magister
{
    public class CursorDefinition : MonoBehaviour
    {
        [Header("Drag")]
        [SerializeField] private Texture2D dragCursor;

        [Header("Close")]
        [SerializeField] private Texture2D closeCursor;

        [Header("NPC Dialog")]
        [SerializeField] private Texture2D npcDialogCursor;

        public Texture2D GetDragCursor()
        {
             return dragCursor;
        }

        public Texture2D GetCloseCursor()
        {
            return closeCursor;
        }

        public Texture2D GetNPCDialogCursor()
        {
            return npcDialogCursor;
        }

    }
}
