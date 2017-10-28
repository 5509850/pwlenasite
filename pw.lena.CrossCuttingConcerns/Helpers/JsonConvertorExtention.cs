using Newtonsoft.Json;

namespace pw.lena.CrossCuttingConcerns.Helpers
{
    public static class JsonConvertorExtention
    {
        public static string ToJsonString(this object self)
        {
            return JsonConvert.SerializeObject(self);
        }

        public static T FromJsonString<T>(this string self)
        {
            return JsonConvert.DeserializeObject<T>(self);
        }
    }
}
