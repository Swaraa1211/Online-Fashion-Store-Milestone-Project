using System.ComponentModel.DataAnnotations;

namespace FashionStoreAPI.Models
{
    public class SizeModel
    {
        [Key]
        public int Size_Id { get; set; }
        public string? Size_Name { get; set; }
    }
}
