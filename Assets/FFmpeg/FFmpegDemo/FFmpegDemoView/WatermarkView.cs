using UnityEngine;
using UnityEngine.UI;

namespace FFmpeg.Demo
{
    public class WatermarkView : MonoBehaviour
    {
        WatermarkData config = new WatermarkData();

        public Text imageScaleText;
        public Slider imageScaleSlider;

        public Text xPosNormalText;
        public Slider xPosNormalSlider;

        public Text yPosNormalText;
        public Slider yPosNormalSlider;

        //------------------------------

        void Awake()
        {
            OnImageScale(imageScaleSlider.value);
            OnXPositionNormalized(xPosNormalSlider.value);
            OnYPositionNormalized(yPosNormalSlider.value);
        }

        //------------------------------

        public void Open()
        {
            gameObject.SetActive(true);
        }

        //------------------------------

        public void OnVideoInputPath(string fullPath)
        {
            config.inputPath = fullPath;
        }

        public void OnImageInputPath(string fullPath)
        {
            config.imagePath = fullPath;
        }

        public void OnOutputPath(string fullPath)
        {
            config.outputPath = fullPath;
        }

        public void OnImageScale(float multiplier)
        {
            config.imageScale = multiplier;
            imageScaleText.text = "Image Scale: " + multiplier;
        }

        public void OnXPositionNormalized(float x)
        {
            config.xPosNormal = x;
            xPosNormalText.text = "Normalized X of image center: " + x;
        }

        public void OnYPositionNormalized(float y)
        {
            config.yPosNormal = y;
            yPosNormalText.text = "Normalized Y of image center: " + y;
        }

        //------------------------------

        public void OnWatermark()
        {
            FFmpegCommands.Watermark(config);
            gameObject.SetActive(false);
        }

        //------------------------------
    }
}