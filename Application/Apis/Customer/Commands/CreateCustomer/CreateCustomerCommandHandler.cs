using Application.Common.Validation;
using Domain.Models;
using Infrastructure;
using Infrastructure.Services.CustomerService;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;


namespace Application.Apis.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ResponseModel>
    {
        protected ICustomerService CustomerService { get; }
        protected AppDbContext _context { get; }


        public CreateCustomerCommandHandler(ICustomerService customerService,AppDbContext dbContext)
        {
            CustomerService = customerService;
            _context = dbContext;
        }

        public async Task<ResponseModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new CreateCustomerCommandValidator(_context).StdValidateAsync(request,CancellationToken.None);
            if (validationResult.Failed())
            {
                return new ResponseModel()
                {
                    Data = null,
                    Message = validationResult.Errors.First().ErrorMessage,
                    Status = StatusCodes.Status422UnprocessableEntity
                };
            }

            var customer = await CustomerService.CreateCustomer(new Domain.Models.Customer()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                BankAccountNumber = request.BankAccountNumber,
                DateOfBirth = request.DateOfBirth,
            });


            return new ResponseModel()
            {
                Data = customer,
                Message = "success",
                Status = StatusCodes.Status200OK
            };
        }
    }
}