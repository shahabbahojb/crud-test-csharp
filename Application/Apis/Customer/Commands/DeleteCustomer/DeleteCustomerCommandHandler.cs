using Microsoft.AspNetCore.Http;
using Domain.Models;
using Infrastructure.Services.CustomerService;
using MediatR;

namespace Application.Apis.Customer.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ResponseModel>
    {
        public ICustomerService CustomerService { get; set; }
        public DeleteCustomerCommandHandler(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        public  async Task<ResponseModel> Handle(DeleteCustomerCommand request, CancellationToken _)
        {
            var customers = await CustomerService.DeleteCustomer(request.Id);

            if (customers==null)
            {
                return new ResponseModel()
                {
                    Data = null,
                    Status = StatusCodes.Status404NotFound,
                    Message = "not found"
                };
            }
            return new ResponseModel()
            {
                Data = customers,
                Message = "success",
                Status = StatusCodes.Status200OK
            };
        }
    }
}