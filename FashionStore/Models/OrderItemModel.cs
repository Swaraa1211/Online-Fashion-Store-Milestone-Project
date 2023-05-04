namespace FashionStore.Models
{
    public class OrderItemModel
    {
        public int OrderItem_Id { get; set; }
        public int Order_Id { get; set; }
        //public int Cart_Id { get; set; }
        public int Product_Id { get; set; }
        public string? Product_Name { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string? User_Email { get; set; }

    }
}
