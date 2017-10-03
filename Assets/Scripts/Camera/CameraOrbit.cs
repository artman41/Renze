using UnityEngine;

namespace Assets.Scripts.Camera {
    //[ExecuteInEditMode]
    public class CameraOrbit : MonoBehaviour {

        public GameObject OrbitCenter;
        public float RotateSpeed = 30f;
        public Vector3 Direction = Vector3.right;
        private Vector3 Center => OrbitCenter.GetComponent<Collider>().bounds.center;

        // Use this for initialization
        void Start () {
		    
        }
	
        // Update is called once per frame
        void Update () {
            transform.RotateAround(Center, Direction, Time.fixedDeltaTime * RotateSpeed);
        }
    }
}
