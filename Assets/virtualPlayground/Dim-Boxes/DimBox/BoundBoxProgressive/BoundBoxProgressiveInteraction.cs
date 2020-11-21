using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimBoxes
{
    [RequireComponent(typeof(BoundBoxProgressive))]
    public class BoundBoxProgressiveInteraction : MonoBehaviour
    {
        public enum Animation_mode { none, stroke, centre_out, centre_in, centre_diag_out, centre_diag_in, corner_diag };
        public Animation_mode animation_mode;
        public float speed = 0.05f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void AnimationMode(int val)
        {
            animation_mode = (Animation_mode)val;
        }

        void OnMouseDown()
        {
            BoundBoxProgressive db = GetComponent<BoundBoxProgressive>();
            db.Animate(speed, (int)animation_mode);
        }

        public void Speed(float val)
        {
            speed = val;
        }
    }
}
