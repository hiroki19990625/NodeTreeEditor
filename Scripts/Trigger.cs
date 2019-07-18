using NodeTreeEditor.Contents;
using UnityEngine;

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
            OnTriggerExit,
            OnTriggerEnter2D,
            OnTriggerExit2D,
            Self
        } //TODO Add OtherType

        public Rect nodeEditorSize = new Rect(0, 0, 2000, 2000);

        public int windowX = 320;
        public int windowY = 150;

        public TriggerType triggerType;
        public GameObject target;

        [HideInInspector] public Start startContent;

        // Use this for initialization
        void Start()
        {
            startContent = GetComponent<Start>();

            if (triggerType == TriggerType.Start)
            {
                StartCoroutine(startContent.Invoke());
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Self()
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

        private void OnTriggerExit(Collider other)
        {
            if (triggerType == TriggerType.OnTriggerExit)
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

        private void OnTriggerExit2D(Collider2D other)
        {
            if (triggerType == TriggerType.OnTriggerExit2D)
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