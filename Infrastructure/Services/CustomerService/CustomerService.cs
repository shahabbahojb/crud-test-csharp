using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CustomerService;

public class CustomerService : ICustomerService
{
    protected AppDbContext DbContext { get; }

    public CustomerService(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }


    public async Task<Customer> CreateCustomer(Customer customer)
    {
        try
        {
          var newCustomer=  await DbContext.Customers.AddAsync(new Customer()
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber,
                DateOfBirth = customer.DateOfBirth
            });

            await DbContext.SaveChangesAsync();

            return newCustomer.Entity;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<List<Customer>?> DeleteCustomer(int id)
    {
        var customer = await DbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);

        if (customer == null)
        {
            return null;
        }

        DbContext.Customers.Remove(customer);
        await DbContext.SaveChangesAsync();
        return await GetCustomersList();
    }

    public async Task<Customer> EditCustomer(Customer customer)
    {
        var editCustomer = await DbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

        if (editCustomer == null)
        {
            return null;
        }

        editCustomer.Email = customer.Email;
        editCustomer.FirstName = customer.FirstName;
        editCustomer.LastName = customer.LastName;
        editCustomer.PhoneNumber = customer.PhoneNumber;
        editCustomer.BankAccountNumber = customer.BankAccountNumber;
        editCustomer.DateOfBirth = customer.DateOfBirth;

        await DbContext.SaveChangesAsync();

        return editCustomer;
        return editCustomer;
    }

    public async Task<Customer?> GetCustomer(int id)
    {
        var customer = await DbContext.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (customer == null)
        {
            return null;
        }

        return customer;
    }

    public async Task<List<Customer>> GetCustomersList()
    {
        return await  DbContext.Customers.AsNoTracking().OrderByDescending(x => x.Id).ToListAsync();
    }
}