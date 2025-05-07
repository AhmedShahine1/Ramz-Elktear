using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Color;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Offer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ramz_Elktear.core.Entities.Specificate;

public class Car
{
    public Car()
    {
        CarColors = new HashSet<CarColor>();
        CarImages = new HashSet<ImageCar>();
        CarOffers = new HashSet<CarOffer>();
        CarSpecifications = new HashSet<CarSpecification>();
    }

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string NameAr { get; set; }

    [Required]
    public string NameEn { get; set; }

    public string? DescrptionAr { get; set; }
    public string? DescrptionEn { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal InstallmentPrice { get; set; }
    public int QuantityInStock { get; set; }

    [ForeignKey("SubCategory")]
    public string? SubCategoryId { get; set; }
    public virtual SubCategory SubCategory { get; set; }

    public string? CarCode { get; set; }

    public string? CarSKU { get; set; }

    [ForeignKey("Image")]
    public string? ImageId { get; set; }
    public virtual Images? Image { get; set; }

    [ForeignKey("Brand")]
    public string BrandId { get; set; }
    public virtual Brand Brand { get; set; }

    [ForeignKey("Option")]
    public string OptionId { get; set; }
    public virtual Option Option { get; set; }

    [ForeignKey("TransmissionType")]
    public string TransmissionTypeId { get; set; }
    public virtual TransmissionType TransmissionType { get; set; }

    [ForeignKey("FuelType")]
    public string FuelTypeId { get; set; }
    public virtual FuelType FuelType { get; set; }

    [ForeignKey("EngineSize")]
    public string EngineSizeId { get; set; }
    public virtual EngineSize EngineSize { get; set; }

    [ForeignKey("Origin")]
    public string? OriginId { get; set; }  // Nullable string
    public virtual Origin? Origin { get; set; }  // Nullable navigation property (optional)

    [ForeignKey("ModelYear")]
    public string ModelYearId { get; set; }
    public virtual ModelYear ModelYear { get; set; }

    public int? Kilometers { get; set; }

    [ForeignKey("EnginePosition")]
    public string EnginePositionId { get; set; }
    public virtual EnginePosition EnginePosition { get; set; }

    public bool IsSpecial { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }

    [InverseProperty("Car")]
    public virtual ICollection<CarColor> CarColors { get; set; }

    [InverseProperty("Car")]
    public virtual ICollection<ImageCar> CarImages { get; set; }

    [InverseProperty("Car")]
    public virtual ICollection<CarOffer> CarOffers { get; set; }

    [InverseProperty("Car")]
    public virtual ICollection<CarSpecification> CarSpecifications { get; set; }
}
