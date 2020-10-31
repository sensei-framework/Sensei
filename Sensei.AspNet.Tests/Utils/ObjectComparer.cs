using Newtonsoft.Json;

namespace Sensei.AspNet.Tests.Utils
{
    public static class ObjectComparer
    {
        public static bool Compare(this object listA, object listB)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var a = JsonConvert.SerializeObject(listA, Formatting.None, settings);
            var b = JsonConvert.SerializeObject(listB, Formatting.None, settings);
            return a == b;
        }
    }
}