namespace FashionStore.Models
{
    public class CartModel
    {
        public int Cart_Id { get; set; }    
        public string? Product_Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string? Useremail { get; set; }

    }
}
