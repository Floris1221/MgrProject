using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace magister
{
    public class UIManager : MonoBehaviour
    {
        [Header("Windows")]
        [SerializeField] TargetWindow targetWindow;
        [SerializeField] DialogWindow dialogWindow;

        public TargetWindow GetTargetWindow()
        {
            return targetWindow;
        }

        public DialogWindow GetDialogWindow()
        {
            return dialogWindow;
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
