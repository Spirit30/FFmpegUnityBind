using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FFmpeg.Demo
{
    public class DefaultFieldInitializer : MonoBehaviour
    {
		[System.Serializable]
		public class SetEvent : UnityEvent<string> { }

		public Text placeholder;
		public SetEvent setter;

        public void Awake()
        {
            setter.Invoke(placeholder.text);
        }
    }
}