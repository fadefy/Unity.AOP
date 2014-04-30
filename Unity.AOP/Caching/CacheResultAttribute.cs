
namespace Unity.AOP.Caching
{
    public class CacheResultAttribute : GenericHandlerAttribute
    {
        public CacheResultAttribute()
            : base(typeof(CacheCallHandler))
        {
        }
    }
}
