using System;
using System.Collections.Generic;

namespace GAMO.Common
{
    public static class Ulti
    {
        public static string Base64Encode(string plainText)
        {
            byte[] inArray = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(inArray);
        }
        /// <summary>
        /// Decode a base 64 string data
        /// </summary>
        /// <param name="base64EncodedData">data encoded</param>
        /// <returns>data decoded</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            byte[] bytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// Map a json string to object class
        /// </summary>
        /// <typeparam name="T">class to map</typeparam>
        /// <param name="json">data to map to</param>
        /// <returns>data mapped object</returns>
        public static T JsonToObject<T>(string json)
        {
            return LitJson.JsonMapper.ToObject<T>(json);
        }

        public static string ToJson<T>(T obj)
        {
            return LitJson.JsonMapper.ToJson(obj);
        }

        //public static string ToJson<T>(this T obj) where T : class
        //{
        //    return LitJson.JsonMapper.ToJson(obj);
        //}
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            if (list == null)
            {
                return true;
            }
            else
            {
                if (list.Count > 0)
                    return false;
                return true;
            }
        }

        public static double StringToDouble(string str)
        {
            double result = 0;
            double.TryParse(str, out result);
            //result = Convert.ToDouble(str);
            return result;
        }

        //public static int StringToInt(string str)
        //{
        //    int result = 0;
        //    int.TryParse(str, out result);
        //    return result;
        //}

        public static int StringToInt(string str)
        {
            return (int)StringToDouble(str);
        }
        
        public static bool VersionIsUpToDate(string oldVer, string newVer)
        {
            // test
            //return false;
            if (string.IsNullOrEmpty(oldVer) || string.IsNullOrEmpty(newVer))
                return false;
            Version version1 = new Version(oldVer);
            Version version2 = new Version(newVer);

            int result = version1.CompareTo(version2);
            if (result >= 0)
                return true;
            else
                return false;
        }

        #region Time


        public static string UnixTimeStampToStringRemain(double timeStamp)
        {
            TimeSpan time = TimeSpan.FromMilliseconds((long)timeStamp);
            return TimeSpanToStringRemain(time);
        }

        //public static string TimeSpanToStringRemain(TimeSpan time)
        //{
        //    return time.Hours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        //}

        public static string TimeSpanToStringRemain(TimeSpan time)
        {
            return time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        }

        public static long DatetimeToUnixTime(DateTime dateTime)
        {
            var timeSpan = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (long)timeSpan.TotalMilliseconds;
        }
        #endregion
    }
}
