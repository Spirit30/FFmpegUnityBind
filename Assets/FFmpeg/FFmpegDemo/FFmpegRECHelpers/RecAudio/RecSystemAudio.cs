using System.Collections.Generic;
using UnityEngine;

public class RecSystemAudio : MonoBehaviour, IRecAudio 
{
    List<float> audioData = new List<float>();
    float startTime;
    int channelsCount;

	void Awake()
	{
        enabled = false;
	}

	public void StartRecording()
    {
        enabled = true;
        startTime = Time.time;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        audioData.AddRange(data);
        channelsCount = channels;
    }

    public void StopRecording(string savePath)
    {
        enabled = false;
        int durationInSec = Mathf.CeilToInt(Time.time - startTime);

        //Create file
        AudioClip buffer = 
            AudioClip.Create(
                "SystemSound", 
                AudioSettings.outputSampleRate * channelsCount * durationInSec, 
                channelsCount, 
                AudioSettings.outputSampleRate, 
                false);
        
        buffer.SetData(audioData.ToArray(), 0);

        SavWav.Save(savePath, buffer);
    }
}
