using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialog : MonoBehaviour
{


    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject toActivate;
    [SerializeField] private Transform standingPoint;
    private Transform avatar;
    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            avatar = other.transform;

            //disable main cam, enable dialog cam
            mainCamera.SetActive(false);
            toActivate.SetActive(true);

            //disable player input
            avatar.GetComponent<PlayerInput>().enabled = false;

            await Task.Delay(50);

            //set position
            avatar.position = standingPoint.position;
            avatar.rotation = standingPoint.rotation;

            //display cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //recover
    public void Recover()
    {
        mainCamera.SetActive(true);
        toActivate.SetActive(false);

        avatar.GetComponent<PlayerInput>().enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
