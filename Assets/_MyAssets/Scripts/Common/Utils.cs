using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Utility
{
    /// <summary>
    /// どこからでも使う便利そうなメソッドをまとめておく
    /// </summary>
    public static class Utils
    {
        public static bool TryGetValue<T>(T[] array, int index, out T value)
        {
            if (IsIndexOutOfRange(array, index))
            {
                value = default;
                return false;
            }
            else
            {
                value = array[index];
                return true;
            }
        }
        public static bool IsIndexOutOfRange<T>(T[] array, int index)
        {
            return index < 0 || array.Length < index + 1;
        }

        public static bool IsIndexOutOfRange<T>(List<T> list, int index)
        {
            return index < 0 || list.Count < index + 1;
        }


        private static bool isOpenLoginBonus(DateTime nowDateTime, DateTime receivedDateTime)
        {
            // "1日0時間0分0秒"のTimeSpanを作成
            TimeSpan timeSpan = new TimeSpan(1, 0, 0, 0);
            return receivedDateTime + timeSpan <= nowDateTime;
        }

        /// <summary>
        /// </summary>
        /// <param name="fov"></param>
        /// <param name="aperture"></param>
        /// <returns></returns>
        public static float FocalLength(float fov, float aperture)
        {
            // FieldOfViewを2で割り、三角関数用にラジアンに変換しておく
            float nHalfTheFOV = fov / 2.0f * Mathf.Deg2Rad;

            // FocalLengthを求める
            float nFocalLength = (0.5f / (Mathf.Tan(nHalfTheFOV) / aperture));

            // Unityちゃんは画面高さ(Vertical)なFOVなので画面アスペクト比(縦/横)を掛けとく
            nFocalLength *= ((float)Screen.height / (float)Screen.width);

            return nFocalLength;
        }
        public static bool HasFlag(this Enum self, Enum flag)//拡張メソッド
        {
            if (self.GetType() != flag.GetType())
            {
                throw new ArgumentException("flag の型が、現在のインスタンスの型と異なっています。");
            }

            var selfValue = Convert.ToUInt64(self);
            var flagValue = Convert.ToUInt64(flag);

            return (selfValue & flagValue) == flagValue;
        }

        public static bool DoesActuallyDo(float percent)
        {
            //小数点以下の桁数
            int digitNum = 0;
            if (percent.ToString().IndexOf(".") > 0)
            {
                digitNum = percent.ToString().Split('.')[1].Length;
            }

            //小数点以下を無くすように乱数の上限と判定の境界を上げる
            int rate = (int)Mathf.Pow(10, digitNum);

            //乱数の上限と真と判定するボーダーを設定
            int randomValueLimit = 100 * rate;
            int border = (int)(rate * percent);

            return UnityEngine.Random.Range(0, randomValueLimit) < border;
        }

        public static bool IsIPad
        {
            get
            {
#if UNITY_IOS || UNITY_IPHONE

                return SystemInfo.deviceModel.Contains("iPad");
#else
                return false;
#endif
            }
        }
    }
}

