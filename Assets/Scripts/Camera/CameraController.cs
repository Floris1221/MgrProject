using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace magister
{
    public class CameraController : MonoBehaviour
    {

        public Transform target;

        public float smoothSpeed = 8f;
        public Vector3 offset;


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
