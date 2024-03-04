using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace magister
{
    public enum InteractableType { Enemy, Item, NPC };

    public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Actor myActor { get; private set; }
        public InteractableType interactionType;

        [SerializeField] private ParticleSystem targetedClickEffect;

        public UnityEvent OnInteractablePointerEnter = new UnityEvent();
        public UnityEvent OnInteractablePointerExit = new UnityEvent();

        private void Awake()
        {
            if (interactionType == InteractableType.Enemy || interactionType == InteractableType.NPC)
            {
                myActor = GetComponent<Actor> ();
            }

        }

        public ParticleSystem GetTargetedClickEffect()
        {
            return targetedClickEffect;
        }

        public void InteractWithItem()
        {
            Destroy(gameObject);
        }

        private void OnMouseEnter()
        {
            OnInteractablePointerEnter.Invoke();
        }

        private void OnMouseExit() 
        {
            OnInteractablePointerExit.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
