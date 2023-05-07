namespace FashionStore.Models
{
    public class OrderModel
    {
        public int Order_Id { get; set; }
        public DateTime Order_Date { get; set; }    
        public int Total_amount { get; set; }
        public string? Paid { get; set; }
        //public bool Payment { get; set; }
        public string? User_Email { get; set; }
        //public ProductsModel? Products { get; set; }
        //public OrderItemModel? OrderItem { get; set; }
    }
}
