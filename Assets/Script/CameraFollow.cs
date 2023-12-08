using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;

        private Vector3 _offset;

        private Vector3 _targetPos;

        public Vector3 plusPos;

        [SerializeField]
        bool loop;

        // lastX < firstX のみ対応
        [SerializeField]
        float lastX;
        [SerializeField]
        float firstX;

        private void Start()
        {
            if (target == null) return;

            _offset = transform.position - target.position;
        }

        private void Update()
        {
            if (target == null) return;

            if (loop)
            {
                if (target.position.x < lastX)
                {
                    float dist = target.position.x - firstX;
                    target.position -= new Vector3(dist, 0, 0);
                    this.transform.position -= new Vector3(dist, 0, 0);
                }
            }

            _targetPos = target.position + _offset + plusPos;
            transform.position = Vector3.Lerp(transform.position, _targetPos, lerpSpeed * Time.deltaTime);

            
        }

        public void ChangePosition()
        {
            this.transform.position = new Vector2(this.transform.position.x + 128,0);
        }

        public void ChangeTarget(Transform _target) 
        {
            target = _target;
        }

        

        public void PlusPosition(Vector3 plus)
        {
            plusPos = plus;
        }

        public void PlusPositionX(float plusX)
        {
            plusPos.x = plusX;
        }

        public void PlusPositionY(float plusY)
        {
            plusPos.y = plusY;
        }

    }
}
