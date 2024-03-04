using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace magister
{
    public class FloatingText : MonoBehaviour
    {
        Transform mainCamera;
        Transform unit;
        Transform worldSpaceCanvas;

        public Vector3 offset;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main.transform;
            unit = transform.parent;
            worldSpaceCanvas = GameObject.Find("WorldSpaceCanvas").transform;

            transform.SetParent(worldSpaceCanvas);
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(transform.position.x - mainCamera.transform.position.x, 0, transform.position.z - mainCamera.transform.position.z)); //look at the camera
            transform.position = unit.position + offset;
        }
    }
}
