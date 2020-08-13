using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo2.Script {
    public class AngleOffsetScript : MonoBehaviour {
        public static Dictionary<String, Point> AngleOffset;
        public static Dictionary<String, Point> LastAngleStore;

        public class Point {
            public float x;
            public float y;
            public float z;

            public Point(float x, float y, float z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public Point() { }

            public static Point operator +(Point p1, Point p2) => new Point(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
            public static Point operator -(int a, Point p) => new Point(a - p.x, a - p.y, a - p.z);

            public override string ToString() {
                return "x:" + this.x + ", y:" + this.y + ", z:" + this.z;
            }
        }

        void Start() {
            AngleOffset = new Dictionary<string, Point>();
            LastAngleStore = new Dictionary<string, Point>();
        }

        public void SetOffset() {
            foreach (var v in LastAngleStore) {
                // if (AngleOffset.ContainsKey(v.Key)) {
                //     AngleOffset[v.Key] = 0 - (v.Value + AngleOffset[v.Key]);
                // } else {
                AngleOffset[v.Key] = 0 - v.Value;
                Debug.Log("UnityDebug_offset:" + v.Key + ":" + AngleOffset[v.Key]);
                // }
            }
        }
    }
}
