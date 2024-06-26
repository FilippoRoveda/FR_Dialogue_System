using System;
using System.Collections.Generic;

namespace DS.Utilities
{
    using Data;
    using Enumerations;
    using System.Diagnostics;

    public static class DS_LenguageUtilities
    {
        public static List<LenguageData<T>> InitLenguageDataSet<T>(T defaultData = null) where T : class
        {
            var dataList = new List<LenguageData<T>>();
            foreach (DS_LenguageType lenguage in (DS_LenguageType[])Enum.GetValues(typeof(DS_LenguageType)))
            {
                dataList.Add(new LenguageData<T>
                {
                    LenguageType = lenguage,
                    Data = defaultData
                });
            }
            return dataList;
        }
        public static List<LenguageData<T>> UpdateLenguageDataSet<T>(List<LenguageData<T>> dataList, T defaultData = null) where T : class
        {
            foreach (DS_LenguageType lenguage in (DS_LenguageType[])Enum.GetValues(typeof(DS_LenguageType)))
            {
                if (dataList.Find(x => x.LenguageType == lenguage) == null)
                {
                    dataList.Add(new LenguageData<T>
                    {
                        LenguageType = lenguage,
                        Data = defaultData
                    });
                }
            }
            return dataList;
        }

        public static LenguageData<T> GetLenguageData<T>(this List<LenguageData<T>> dataList, DS_LenguageType lenguageType) where T : class
        {
            LenguageData<T> data = dataList.Find(x => x.LenguageType == lenguageType);
            if(data == null)
            {
                dataList = UpdateLenguageDataSet(dataList);
                data = dataList.Find(x => x.LenguageType == lenguageType);
            }
            return data;
        }

        public static void SetLenguageData<T>(this List<LenguageData<T>> dataList, DS_LenguageType lenguageType, T newDataValue) where T : class
        {
            T data = dataList.Find(x => x.LenguageType == lenguageType).Data;
            if (data == null)
            {
                dataList = UpdateLenguageDataSet(dataList);
                data = dataList.Find(x => x.LenguageType == lenguageType).Data;
                Logger.Error(data.ToString(), UnityEngine.Color.red);
            }
            data = newDataValue;
        }
    }
}
