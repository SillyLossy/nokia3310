using Nokia3310.Applications.Common;

namespace Nokia3310.Applications.Extensions
{
    public static class EnumHelper
    {
        public static T Random<T>()
        {
            var v = System.Enum.GetValues(typeof(T));
            return (T)v.GetValue(NokiaApp.Random.Next(v.Length));
        }
    }
}
