namespace Domain.Models;

public class ResponseModel
{
    public object? Data { get; set; }
    public int Status { get; set; }
    public string Message { get; set; }
}