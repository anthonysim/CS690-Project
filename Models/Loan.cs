namespace final_project.Models;

public class Loan
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int PatronId { get; set; }
    public DateTime CheckoutDate { get; set; }
    public DateTime DueDate { get; set; }
}
