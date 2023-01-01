using Application.Apis.Customer.Commands.CreateCustomer;
using Application.Apis.Customer.Commands.DeleteCustomer;
using Application.Apis.Customer.Commands.EditCustomer;
using Application.Apis.Customer.Queries.GetCustomer;
using Application.Apis.Customer.Queries.GetCustomersList;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/customer")]
[ApiController]
public class CustomerController : Controller
{
    private readonly IMediator _mediator;
    public CustomerController(IMediator mediator) => _mediator = mediator;

    
    [HttpGet]
    public async Task<ActionResult<ResponseModel>> GetCustomersList()
    {
        return Ok(await _mediator.Send(new GetCustomersListQuery()));
    }
    
    
    [HttpGet("{Id}")]
    public async Task<ActionResult<ResponseModel>> GetCustomerDetail([FromRoute] GetCustomerQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
    
    [HttpPost]
    public async Task<ActionResult<ResponseModel>> AddCustomer([FromForm] CreateCustomerCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPut]
    public async Task<ActionResult<ResponseModel>> EditCustomer([FromForm] EditCustomerCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpDelete("{Id}")]
    public async Task<ActionResult<ResponseModel>> DeleteCustomer([FromRoute] DeleteCustomerCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}