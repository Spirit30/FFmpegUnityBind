using UnityEngine;

public class RecMicAudio : MonoBehaviour, IRecAudio 
{
    public int maxLength = 60;
    AudioClip buffer;

    public void StartRecording()
    {
        buffer = Microphone.Start(null, false, maxLength, AudioSettings.outputSampleRate);
    }

    public void StopRecording(string savePath)
    {
        Microphone.End(null);
        SavWav.Save(savePath, buffer);
    }
}
