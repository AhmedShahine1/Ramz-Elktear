using AutoMapper;
using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ramz_Elktear.core.Entities.Brands
{
    public class Brand
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        [ForeignKey(nameof(Logo))]
        public string LogoId { get; set; }
        public Images Logo { get; set; }
    }
}
