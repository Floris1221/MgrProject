using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace magister
{
    public class Draggable : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
    {
        [Header("Canvas")]
        [SerializeField] Canvas canvas;

        [Header("Drag")]
        [SerializeField] RectTransform draggable;
        [SerializeField] Texture2D draggableCursor;

        private bool isDragging = false;
        Vector3 initialpos;


        //Initialize
        void Awake()
        {
            if (draggable == null)
            {
                draggable = transform.parent.GetComponent<RectTransform>();
            }
            if (canvas == null)
            {
                Transform testCanvasTransform = transform.parent;
                while (testCanvasTransform != null)
                {
                    canvas = testCanvasTransform.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        break;
                    }
                    testCanvasTransform = testCanvasTransform.parent;
                }
            }
            if (draggableCursor == null)
            {
                if (canvas != null)
                {
                    Texture2D cursor = canvas.GetComponent<CursorDefinition>().GetDragCursor();
                    if (cursor != null) draggableCursor = cursor;
                }
            }
        }

        void Start ()
        {
            initialpos = draggable.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (draggableCursor != null)
            {
                Cursor.SetCursor(draggableCursor, Vector2.zero, CursorMode.Auto);
            }
            isDragging = true;


        }

        public void OnDrag(PointerEventData eventData)
        {
            // Get the position in the canvas space
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 newPosition = draggable.anchoredPosition + eventData.delta / canvas.scaleFactor;

            // Calculate boundaries
            float minX = -canvasRect.sizeDelta.x / 2f + draggable.sizeDelta.x / 2f;
            float maxX = canvasRect.sizeDelta.x / 2f - draggable.sizeDelta.x / 2f;
            float minY = -canvasRect.sizeDelta.y / 2f + draggable.sizeDelta.y / 2f;
            float maxY = canvasRect.sizeDelta.y / 2f - draggable.sizeDelta.y / 2f;

            // Clamp position to boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            // Apply the new position
            draggable.anchoredPosition = newPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (draggableCursor != null)
            {
                Cursor.SetCursor(draggableCursor, Vector2.zero, CursorMode.Auto);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isDragging)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        //Move window to the top
        public void OnPointerDown(PointerEventData eventData)
        {
            draggable.SetAsLastSibling();
        }

        public void ResetPosition()
        {
            draggable.anchoredPosition = initialpos;
        }
    }
}
