using System.Collections.Generic;
using Sensei.AspNet.Models;

namespace Sensei.AspNet.Tests.Utils
{
    public static class ListComparer
    {
        public static bool Compare<T>(this List<T> listA, List<T> listB)
            where T : BaseModel
        {
            if (listA.Count != listB.Count)
                return false;

            for (var i = 0; i < listA.Count; i++)
            {
                var valA = listA[i];
                var valB = listB[i];

                if (valA.Id != valB.Id)
                    return false;
            }

            return true;
        }
    }
}