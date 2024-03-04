using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace magister
{
    public class DialogWindow : Window
    {

        [Header("Target Information")]
        [SerializeField] TMP_Text targetNameObject;

        public UnityEvent OnDialogWindowShown;
        public UnityEvent OnDialogWindowClosed;

        private Personality dialogTargetPersonality;

        void Awake()
        {
            //Find Name
            if (targetNameObject == null)
            {
                targetNameObject = GetComponentInChildren<TMP_Text>();
            }
        }

        public void ShowWindow(Personality dialogTargetPersonality)
        {
            this.dialogTargetPersonality = dialogTargetPersonality;
            targetNameObject.text = dialogTargetPersonality.GetName();
            gameObject.SetActive(true);
            OnDialogWindowShown.Invoke();
        }

        public Personality GetPersonality()
        {
            return dialogTargetPersonality;
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
