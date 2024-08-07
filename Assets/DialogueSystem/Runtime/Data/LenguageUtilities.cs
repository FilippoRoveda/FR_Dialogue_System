using System;
using System.Collections.Generic;

namespace DS.Runtime.Data
{
    using Runtime.Enumerations;
    public static partial class LenguageUtilities
    {
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
    }
}

