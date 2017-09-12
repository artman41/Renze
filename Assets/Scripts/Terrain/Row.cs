using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Terrain {
    [Serializable]
    public class Row {

        public GameObject gameObject;
        public GameObject[] block;

        public Row(GameObject go) {
            gameObject = go;
            block = new GameObject[go.transform.childCount];
            for (int i = 0; i < go.transform.childCount; i++) {
                block[i] = go.transform.GetChild(i).gameObject;
            }
        }

    }
}
