using System;
using UnityEngine;

namespace FFmpeg
{
    /// <summary>
    /// This is the Helper class to get FFmpeg operation progress.
    /// </summary>
    public static class FFmpegProgressParser
    {
        const string FORMAT = "HH:mm:ss.ff";
        static readonly string[] durationSeparators = { "Duration: ", ", start:" };
        static readonly string[] timeSeparators = { " time=", " bitrate=" };
        static int lastDurationTokensLength;

        //PUBLIC INTERFACE
		//------------------------------

        public static void Parse(string log, ref float durationMiniSec, ref float progress) 
        {
            string[] durationTokens = log.Split(durationSeparators, StringSplitOptions.RemoveEmptyEntries);
            if(durationTokens.Length != lastDurationTokensLength)
            {
                UpdateDuration(durationTokens, ref durationMiniSec);
                lastDurationTokensLength = durationTokens.Length;
            }
            else if(durationMiniSec > 0) //When Duration is obtained
            {
                string timeToken = GetTimeToken(log, timeSeparators);
                if(timeToken != null)
                {
                    progress = GetMiliSec(timeToken) / durationMiniSec;
                }
            }
        }

		//------------------------------

        static void UpdateDuration(string[] tokens, ref float durationMiniSec)
		{
            durationMiniSec = 0;
            for (int t = 0; t < tokens.Length; ++t)
            {
                durationMiniSec += GetMiliSec(tokens[t]);
            }
		}

		static string GetTimeToken(string log, string[] separators)
        {
            string[] tokens = log.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if(tokens.Length > 2)
            {
                return tokens[tokens.Length - 2];
            }
            return null;
        }

        static float GetMiliSec(string token)
        {
            DateTime time;
            if (DateTime.TryParseExact(token, FORMAT, null, System.Globalization.DateTimeStyles.None, out time))
            {
                return (float)time.TimeOfDay.TotalMilliseconds;
            }
            return 0;
        }
	}
}