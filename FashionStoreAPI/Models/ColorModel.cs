using System.ComponentModel.DataAnnotations;

namespace FashionStoreAPI.Models
{
    public class ColorModel
    {
        [Key]
        public int Color_Id { get; set; }
        public string? Color_Name { get; set; }
    }
}
