using Domain.Models;

namespace Infrastructure.Services.CustomerService;

public interface ICustomerService
{
    Task<Customer> CreateCustomer(Customer customer);
    Task<List<Customer>?> DeleteCustomer(int id);
    Task<Customer> EditCustomer(Customer customer);
    Task<Customer?> GetCustomer(int id);
    Task<List<Customer>> GetCustomersList();
}