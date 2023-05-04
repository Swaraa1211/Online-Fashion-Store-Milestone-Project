namespace FashionStore.Models
{
    public class ProductsModel
    {
        public int Product_Id { get; set; }
        public string? Product_Name { get; set; }
        public string? Product_Description { get; set;}
        public string? Product_Image { get; set; }
        public string? Color{ get; set; }
        public string? Size { get; set;}
        public int Price { get; set; }
        //public string? Product_Image { get; set; }
        public CartModel? Cart { get; set; }


    }
}
