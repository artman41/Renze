using UnityEngine;

namespace Assets.Scripts.Terrain {
    public class FieldAccess : MonoBehaviour {

        public Row[] Red;
        public Row[] Blue;

        // Use this for initialization
        void Start () {
            for (int i = 0; i < transform.childCount; i++) {
                var o = transform.GetChild(i).gameObject;
                if (o.tag == "Red") {
                    Red = new Row[o.transform.childCount];
                    for (int j = 0; j < o.transform.childCount; j++) {
                        Red[j] = new Row(o.transform.GetChild(j).gameObject);
                    }
                } else {
                    Blue = new Row[o.transform.childCount];
                    for (int j = 0; j < o.transform.childCount; j++) {
                        Blue[j] = new Row(o.transform.GetChild(j).gameObject);
                    }
                }
            }
        }
    }
}
