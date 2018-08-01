using Microsoft.AspNetCore.Identity;

namespace S4Sales.Identity
{
    public class S4LookupNormalizer : ILookupNormalizer
    {
        public virtual string Normalize(string key)
        {
            if (key == null) { return null; }
            return key.Normalize().ToLowerInvariant();
        }
    }
}
