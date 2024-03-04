using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace magister
{
    public class Window : MonoBehaviour
    {

        [Header("Close Window")]
        [SerializeField] Texture2D closeWindowButtonPointer;

        [SerializeField] protected Draggable draggable;

        //Init 
        private void Awake()
        {
            //Canvas should be parent of window object
            if (closeWindowButtonPointer == null)
            {
                Texture2D closeBtn = transform.parent.GetComponent<CursorDefinition>().GetCloseCursor();
                if (closeBtn != null)
                {
                    closeWindowButtonPointer = closeBtn;
                }
            }

            //Find draggable 
            draggable = GetComponent<Draggable>();
            if (draggable == null)
            {
                draggable = GetComponentInChildren<Draggable>();
            }


        }

        virtual public void ShowWindow()
        {
            gameObject.SetActive(true);
        }

        virtual public void CloseWindow()
        {
            gameObject.SetActive(false);
            if (draggable != null) draggable.ResetPosition();
            CloseWindowButtonExit();
        }

        public void CloseWindowButtonEnter()
        {
            if (closeWindowButtonPointer != null)
            {
                Cursor.SetCursor(closeWindowButtonPointer, Vector2.zero, CursorMode.Auto);
            }
        }
        public void CloseWindowButtonExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
