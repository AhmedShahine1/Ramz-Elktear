using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ramz_Elktear.core.Entities
{
    public class Setting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ImageType { get; set; }
        public Images ImageUrl { get; set; }
        public string ImageUrlId { get; set; }
    }
}
