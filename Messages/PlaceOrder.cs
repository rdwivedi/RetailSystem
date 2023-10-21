namespace Messages;

public class PlaceOrder : ICommand
{
    public string OrderID { get; set; }
}
