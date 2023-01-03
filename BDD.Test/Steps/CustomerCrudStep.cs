using Application.Apis.Customer.Commands.CreateCustomer;
using Application.Apis.Customer.Commands.EditCustomer;
using Application.Apis.Customer.Queries.GetCustomer;
using Application.Apis.Customer.Queries.GetCustomersList;
using Domain.Models;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Services.CustomerService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using SpecFlow.Internal.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BDD.Test.Steps;

[Binding]
public class CustomerCrudStep
{
    private ICustomerService CustomerService { get; set; }
    private DbContextOptions<AppDbContext> DbContextOptions { get; set; }

    private readonly ScenarioContext _scenarioContext;

    public CustomerCrudStep(ScenarioContext scenarioContext)
    {
        DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "CustomersDataBase")
            .Options;

        
        _scenarioContext = scenarioContext;
    }


    [When(@"I create customer with following details")]
    public async Task WhenICreateCustomerWithFollowingDetails(Table table)
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);

        var customers = table.CreateSet<CreateCustomerCommand>();

        List<Customer> addedCustomers = new List<Customer>();

        foreach (CreateCustomerCommand customer in customers)
        {
            var addedCustomer = await CustomerService.CreateCustomer(new Customer()
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber,
                DateOfBirth = customer.DateOfBirth,
            });
            addedCustomers.Add(addedCustomer);
        }

        _scenarioContext.Add("CreatedCustomer", addedCustomers);
    }

    [Then(@"the customers are created successfully")]
    public async Task ThenTheCustomersAreCreatedSuccessfully()
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);

        var createdCustomer = _scenarioContext.Get<List<Customer>>("CreatedCustomer");

        foreach (var customer in createdCustomer)
        {
            var res = await CustomerService.GetCustomer((int) customer.Id);
            customer.Should().BeEquivalentTo(res);
        }
    }

    [Given(@"the following customers are in the system")]
    public void GivenTheFollowingCustomersAreInTheSystem(Table table)
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);

        var customers = table.CreateSet<CreateCustomerCommand>();

        List<Customer> addedCustomers = new List<Customer>();


        foreach (CreateCustomerCommand customer in customers)
        {
            var add = context.Customers.Add(new Customer()
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber,
                DateOfBirth = customer.DateOfBirth,
            });
            context.SaveChanges();

            addedCustomers.Add(add.Entity);
        }

        _scenarioContext.Add("OnSystemCustomers", addedCustomers);
    }

    [When(@"I these customers get deleted")]
    public void WhenITheseCustomersGetDeleted()
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);

        var onSystemCustomers = _scenarioContext.Get<List<Customer>>("OnSystemCustomers");

        foreach (var systemCustomer in onSystemCustomers)
        {
            CustomerService.DeleteCustomer((int) systemCustomer.Id);
        }
    }

    [Then(@"the customers are deleted successfully")]
    public void ThenTheCustomersAreDeletedSuccessfully()
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);

        this.CustomerService.GetCustomersList().Result.Count.Should().Be(0);
    }

    [When(@"I create customer with invalid input")]
    public async Task WhenICreateCustomerWithInvalidInput(Table table)
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);
        var customer = table.CreateSet<CreateCustomerCommand>().First();
        var commandHandler = new CreateCustomerCommandHandler(this.CustomerService, context);
        var addedCustomer = await commandHandler.Handle(new CreateCustomerCommand()
        {
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            BankAccountNumber = customer.BankAccountNumber,
            DateOfBirth = customer.DateOfBirth,
        }, CancellationToken.None);


        _scenarioContext.Add("InvalidDataResponse", addedCustomer);
    }

    [Then(@"Get Validation Error")]
    public void ThenGetValidationError()
    {
        var invalidDataResponse = _scenarioContext.Get<ResponseModel>("InvalidDataResponse");
        invalidDataResponse.Status.Should().Be(422);
    }

    [When(@"I get customers list")]
    public async Task WhenIGetCustomersList()
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);
        var queryHandler = new GetCustomersListQueryHandler(CustomerService);
        var response = await queryHandler.Handle(new GetCustomersListQuery(), CancellationToken.None);
        _scenarioContext.Add("GetCustomersList", response.Data);
    }

    [Then(@"the customers list data is correct")]
    public void ThenTheCustomersListDataIsCorrect()
    {
        var getCustomersList = _scenarioContext.Get<List<Customer>>("GetCustomersList");

        getCustomersList.Count.Should().Be(2);
    }

    [When(@"I update customer with these data")]
    public async Task WhenIUpdateCustomerWithTheseData(Table table)
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);
        var commandHandler = new EditCustomerCommandHandler(CustomerService,context);
        var customerData = table.CreateSet<EditCustomerCommand>().Single();

        var response = await commandHandler.Handle(customerData, CancellationToken.None);
        _scenarioContext.Add("EditedCustomer", customerData);

    }

    [Given(@"This Customer is in the system")]
    public void GivenThisCustomerIsInTheSystem()
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);
    }

    [Then(@"the customers updates correctly")]
    public async Task ThenTheCustomersUpdatesCorrectly()
    {
        var editedCustomer = _scenarioContext.Get<EditCustomerCommand>("EditedCustomer");

        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        this.CustomerService = new CustomerService(context);
        var queryHandler = new GetCustomerQueryHandler(CustomerService);
        var customer =await queryHandler.Handle(new GetCustomerQuery()
        {
            Id = editedCustomer.Id
        },CancellationToken.None);
        
    }

    [Given(@"the following customers are in the system for customer list")]
    public void GivenTheFollowingCustomersAreInTheSystemForCustomerList(Table table)
    {
        using var context = new AppDbContext(DbContextOptions, new HttpContextAccessor());
        var customers = table.CreateSet<Customer>();
        foreach ( var customer in customers)
        {
            var add = context.Customers.Add(new Customer()
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber,
                DateOfBirth = customer.DateOfBirth,
            });
            context.SaveChanges();

        }
    }
}