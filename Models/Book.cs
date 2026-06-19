namespace final_project.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    public string GetStatus()
    {
        if (TotalCopies == 0)
            return "Unavailable";

        return AvailableCopies > 0 ? "Available" : "Checked Out";
    }
}
