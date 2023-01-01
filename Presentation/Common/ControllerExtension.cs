using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace Presentation.Common
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class ControllerExtension : Controller
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        
    }
}