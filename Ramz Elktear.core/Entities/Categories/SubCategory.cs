using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Categories;

public class SubCategory
{
    public SubCategory()
    {
        Cars = new HashSet<Car>();
    }

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string NameAr { get; set; }
    public string NameEn { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime LastModifiedDate { get; set; }

    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }

    [ForeignKey("Category")]  // ForeignKey attribute for Category
    public string CategoryId { get; set; }

    [ForeignKey("Brand")]  // ForeignKey attribute for Brand
    public string BrandId { get; set; }

    public Category Category { get; set; }

    public Brand Brand { get; set; }

    public ICollection<Car> Cars { get; set; }
}
