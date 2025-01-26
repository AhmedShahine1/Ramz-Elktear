using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace Ramz_Elktear.core.Entities.Files
{
    [DebuggerDisplay("{Name,nq}")]
    public class Paths
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Images>? Images { get; set; }
    }
}
