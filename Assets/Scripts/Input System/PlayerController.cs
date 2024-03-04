using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;
using UnityEngine.EventSystems;

namespace magister
{
    public class PlayerController : MonoBehaviour
    {
        const string IDLE = "Idle";
        const string WALK = "Walk";

        CustomActions input;

        NavMeshAgent agent;
        Animator animator;

        [Header("User Interface")]
        [SerializeField] UIManager UImanager;

        [Header("Movement")]
        [SerializeField] ParticleSystem clickEffect;
        [SerializeField] ParticleSystem clickEffectFinish;
        [SerializeField] LayerMask clickableLayers;
        [SerializeField] float destinationReachedThreshold = 0.1f;
        GameObject clickEffectGameObject; //temp object to store click effect particle game object

        [Header("Target")]
        [SerializeField] GameObject currentTarget;
        [SerializeField] float targetReachedDistanceThreshold = 1.5f;
        [SerializeField] float targetInteractionDelay = 0.1f;
        GameObject targetClickEffectGameObject; //temp object to store click effect particle game object

        //UI Elements
        private TargetWindow targetWindow;
        private DialogWindow dialogWindow;

        //Cursors
        private Texture2D npcDialogCursor;

        float lookRotationSpeed = 8f;

        bool playerBusy = false;
        bool targetFocused = false;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            targetWindow = UImanager.GetTargetWindow();
            targetWindow.OnWindowClosed.AddListener(LoseTarget);

            dialogWindow = UImanager.GetDialogWindow();

            npcDialogCursor = UImanager.GetComponent<CursorDefinition>().GetNPCDialogCursor();

            input = new CustomActions();
            AssignInputs();
        }

        private void AssignInputs()
        {
            input.Main.Move.performed += ctx => ClickToMove();
            input.Main.LoseTarget.performed += ctx => LoseTarget();
        }


        private void ClickToMove()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (clickEffectGameObject != null)
                {
                    Destroy(clickEffectGameObject);
                }

                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers))
                {

                    if (hit.transform.CompareTag("Interactable"))
                    {
                        if (currentTarget != null) {
                            if (currentTarget.gameObject == hit.transform.gameObject) targetFocused = true;
                            else targetFocused = false;
                        }
                        currentTarget = hit.transform.gameObject;
                        if (currentTarget == null) return;

                        Interactable interactable = currentTarget.GetComponent<Interactable>();
                        interactable.OnInteractablePointerEnter.AddListener(OnTargetPointerEnter);
                        interactable.OnInteractablePointerExit.AddListener(OnTargetPointerExit);
                        OnTargetPointerEnter();

                        targetWindow.ShowWindow(currentTarget.GetComponent<Personality>().GetName()); //Show target window

                        //Click effect
                        if (interactable.GetTargetedClickEffect() == null) return;

                        ParticleSystem targetClickEffect = currentTarget.GetComponent<Interactable>().GetTargetedClickEffect();
                        ParticleSystem clickEffectPS = Instantiate(targetClickEffect, currentTarget.transform.position += new Vector3(0, 0.1f, 0), targetClickEffect.transform.rotation);
                        if (targetClickEffectGameObject != null) Destroy(targetClickEffectGameObject); //destroy last target effect and get new that got created

                        targetClickEffectGameObject = clickEffectPS.gameObject;

                    }
                    else
                    {
                        targetFocused = false;
                        agent.destination = hit.point;
                        if (clickEffect != null)
                        {
                            ParticleSystem clickEffectPS = Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
                            clickEffectGameObject = clickEffectPS.gameObject;
                        }
                    }
                }
            }         
        }

        private void OnTargetPointerEnter()
        {
            switch(currentTarget.GetComponent<Interactable>().interactionType)
            {
                case InteractableType.NPC:
                    Cursor.SetCursor(npcDialogCursor, Vector2.zero, CursorMode.Auto); break;
            }
        }

        private void OnTargetPointerExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }


        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable(); 
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            FollowTarget();
            FaceTarget();
            SetAnimations();
        }

        private void ReachedDestination()
        {
            agent.SetDestination(transform.position);

            if (playerBusy) return;

            playerBusy = true;

            switch(currentTarget.GetComponent<Interactable>().interactionType)
            {
                case InteractableType.NPC:
                    dialogWindow.ShowWindow(currentTarget.GetComponent<Personality>());
                    Invoke(nameof(ResetBusyState), targetInteractionDelay);
                    targetFocused = false;
                    break;

                case InteractableType.Enemy: break;
                case InteractableType.Item: break;
            }

        }

        private void FollowTarget()
        {
            //Delete particle for agent destination
            if (agent.remainingDistance <= agent.stoppingDistance + destinationReachedThreshold && clickEffectGameObject != null)
            {
                Instantiate(clickEffectFinish, agent.destination += new Vector3(0, 0.1f, 0), clickEffectFinish.transform.rotation);
                Destroy(clickEffectGameObject);
            }

            if (currentTarget == null) return;
            if (!targetFocused) return;

            if (Vector3.Distance(currentTarget.transform.position, transform.position) <= targetReachedDistanceThreshold) { ReachedDestination(); }
            else agent.SetDestination(currentTarget.transform.position);

        }

        private void ResetBusyState()
        {
            playerBusy = false;
            SetAnimations();
        }

        private void LoseTarget()
        {
            if (currentTarget == null) return;

            Interactable interactable = currentTarget.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.OnInteractablePointerEnter.RemoveAllListeners();
                interactable.OnInteractablePointerExit.RemoveAllListeners();
            }

            currentTarget = null;
            if (targetClickEffectGameObject != null) Destroy(targetClickEffectGameObject);
            if (targetWindow.isActiveAndEnabled) targetWindow.CloseWindow();
        }

        private void SetAnimations()
        {
            if (playerBusy) return;

            if (agent.velocity == Vector3.zero)
            {
                animator.Play(IDLE);
            }
            else
            {
                animator.Play(WALK);
            }
        }

        private void FaceTarget()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Vector3 direction = (agent.destination - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
            }

        }

    }
}
