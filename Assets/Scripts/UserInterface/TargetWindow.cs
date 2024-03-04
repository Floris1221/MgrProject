using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace magister
{
    public class TargetWindow : Window
    {

        [Header("Target Information")]
        [SerializeField] TMP_Text targetNameObject;

        /*private Draggable draggable;*/

        public UnityEvent OnWindowClosed = new UnityEvent();

        //Init 
        void Awake()
        {

            //Find Name
            if (targetNameObject == null)
            {
                targetNameObject = GetComponentInChildren<TMP_Text>();
            }

        }

        public void ShowWindow(string targetName)
        {
            targetNameObject.text = targetName;
            gameObject.SetActive(true);
        }

        override
        public void CloseWindow()
        {
            gameObject.SetActive(false);
            if (draggable != null) draggable.ResetPosition();
            CloseWindowButtonExit();

            //Invoke event
            OnWindowClosed.Invoke();
        }

    }
}
