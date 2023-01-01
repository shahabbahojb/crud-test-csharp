using Application.Common.Validation;
using Domain.Models;
using Infrastructure;
using Infrastructure.Services.CustomerService;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Apis.Customer.Commands.EditCustomer
{
    public class EditCustomerCommandHandler : IRequestHandler<EditCustomerCommand, ResponseModel>
    {
        public ICustomerService CustomerService { get; set; }
        public AppDbContext _context { get; set; }

        public EditCustomerCommandHandler(ICustomerService customerService,AppDbContext dbContext)
        {
            CustomerService = customerService;
            _context = dbContext;
        }

        public async Task<ResponseModel> Handle(EditCustomerCommand request, CancellationToken _)
        {
            var validationResult = await new EditCustomerCommandValidator(_context,request.Id).StdValidateAsync(request, _);
            if (validationResult.Failed())
            {
                return new ResponseModel()
                {
                    Data = null,
                    Message = validationResult.Errors.First().ErrorMessage,
                    Status = StatusCodes.Status422UnprocessableEntity
                };
            }

            var customer=await CustomerService.EditCustomer(new Domain.Models.Customer()
            {
                Id = request.Id,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                BankAccountNumber = request.BankAccountNumber,
                DateOfBirth = request.DateOfBirth
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