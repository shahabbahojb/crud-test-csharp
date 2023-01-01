using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Apis.Customer.Commands.CreateCustomer;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Services.CustomerService;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Test.Steps;

[Binding]
public sealed class CustomerManagement
{
    
    private ICustomerService _customerService;

    public CustomerManagement(ICustomerService customerService)
    {
        _customerService = customerService;
    }


    [When(@"I create customer with following details")]
    public void WhenICreateCustomerWithFollowingDetails(Table table)
    {
        var customers = table.CreateSet<CreateCustomerCommand>();

        foreach (CreateCustomerCommand customer in customers)
        {
            _customerService.CreateCustomer(new Customer()
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber,
                DateOfBirth = customer.DateOfBirth,
            });
        }
    }

    [Then(@"the customers are created successfully")]
    public void ThenTheCustomersAreCreatedSuccessfully()
    {
        _customerService.GetCustomersList().Result.Count.Should().Be(2);
    }

    [Given(@"the following customers are in the system")]
    public void GivenTheFollowingCustomersAreInTheSystem(Table table)
    {
        var customers = table.CreateSet<CreateCustomerCommand>();

        foreach (CreateCustomerCommand customer in customers)
        {
            _customerService.CreateCustomer(new Customer()
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber,
                DateOfBirth = customer.DateOfBirth,
            });
        }
    }

    [When(@"I thise customers get deleted")]
    public void WhenIThiseCustomersGetDeleted()
    {
        var customers = _customerService.GetCustomersList().Result;

        foreach (Customer customer in customers)
        {
            _customerService.DeleteCustomer((int) customer.Id);
        }
        
        _customerService.GetCustomersList().Result.Count.Should().Be(0);
        
    }

    [Then(@"the customers are deleted successfully")]
    public void ThenTheCustomersAreDeletedSuccessfully()
    {
        _customerService.GetCustomersList().Result.Count.Should().Be(0);

    }
}