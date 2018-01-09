using UnityEngine;
using System.Collections;

using NodeTreeEditor.Contents;


namespace NodeTreeEditor
{
    /// <summary>
    /// Trigger.
    /// </summary>
    [RequireComponent(typeof(Start), typeof(End)), AddComponentMenu("NodeTreeEditor/EventTrigger/Trigger")]
    public class Trigger : MonoBehaviour
    {

        public enum TriggerType
        {
            Start,
            OnTriggerEnter,
            OnTriggerEnter2D,
            Self
        }//TODO Add OtherType

        public TriggerType triggerType;
        public GameObject target;

        public Start startContent;

        // Use this for initialization
        void Start()
        {
            if (triggerType == TriggerType.Start)
            {
                StartCoroutine(startContent.Invoke());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Self()
        {
            if (triggerType == TriggerType.Self)
            {
                StartCoroutine(startContent.Invoke());
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (triggerType == TriggerType.OnTriggerEnter)
            {
                if (target == null)
                {
                    StartCoroutine(startContent.Invoke());
                }
                else if (target.name == other.gameObject.name)
                {
                    StartCoroutine(startContent.Invoke());
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (triggerType == TriggerType.OnTriggerEnter2D)
            {
                if (target == null)
                {
                    StartCoroutine(startContent.Invoke());
                }
                else if (target.name == other.gameObject.name)
                {
                    StartCoroutine(startContent.Invoke());
                }
            }
        }
    }
}