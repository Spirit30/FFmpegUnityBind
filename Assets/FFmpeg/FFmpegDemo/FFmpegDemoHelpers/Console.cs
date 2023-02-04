using UnityEngine;
using UnityEngine.UI;

namespace FFmpeg.Demo
{
    /// <summary>
    /// This class role is to avoid overcome of vertex limit 
    /// on Output Text component if text is too big.
    /// </summary>
    public class Console : MonoBehaviour
    {
        static Console self;
        public Text output;
        public Scrollbar vertical;
        const int  UNITY_VERTS_LIMIT = 65000;
        const int CHAR_MIN = 2048, CHAR_MAX = UNITY_VERTS_LIMIT / 4 - 1;
        [Range(CHAR_MIN, CHAR_MAX)]
        public int outputCharLimit =

#if UNITY_ANDROID && !UNITY_EDITOR
            CHAR_MAX / 5;
#else
            CHAR_MAX;
#endif

        void Awake()
        {
            self = this;
        }

        public static void Print(string msg)
        {
            if(msg.Length > self.outputCharLimit)
            {
                msg = msg.Remove(0, msg.Length - self.outputCharLimit);
            }
            self.output.text = msg;
            self.vertical.value = 0;
        }
    }
}