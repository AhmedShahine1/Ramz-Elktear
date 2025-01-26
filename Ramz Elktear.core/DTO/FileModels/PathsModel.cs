using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ramz_Elktear.core.DTO.FileModels
{
    public class PathsModel
    {
        [Required(ErrorMessage = "Should Enter Name Path"), DisplayName("Name Path"), StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Should Enter Description Path"), DisplayName("Description Path"), StringLength(250)]
        public string Description { get; set; }
    }

}
