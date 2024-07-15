using System;
using System.Collections.Generic;

namespace DS.Runtime.Data
{
    using Enums;

    public static class LenguageUtilities
    {
        public static List<LenguageData<T>> InitLenguageDataSet<T>(T defaultData = null) where T : class
        {
            var dataList = new List<LenguageData<T>>();
            foreach (LenguageType lenguage in (LenguageType[])Enum.GetValues(typeof(LenguageType)))
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
            foreach (LenguageType lenguage in (LenguageType[])Enum.GetValues(typeof(LenguageType)))
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

        public static LenguageData<T> GetLenguageData<T>(this List<LenguageData<T>> dataList, LenguageType lenguageType) where T : class
        {
            LenguageData<T> data = dataList.Find(x => x.LenguageType == lenguageType);
            if(data == null)
            {
                dataList = UpdateLenguageDataSet(dataList);
                data = dataList.Find(x => x.LenguageType == lenguageType);
            }
            return data;
        }

        public static void SetLenguageData<T>(this List<LenguageData<T>> dataList, LenguageType lenguageType, T newDataValue) where T : class
        {
            T data = dataList.Find(x => x.LenguageType == lenguageType).Data;
            if (data == null)
            {
                dataList = UpdateLenguageDataSet(dataList);
                data = dataList.Find(x => x.LenguageType == lenguageType).Data;
            }
            data = newDataValue;
        }
    }
}

