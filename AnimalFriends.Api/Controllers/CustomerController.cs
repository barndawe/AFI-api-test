using AnimalFriends.Api.RequestModels;
using AnimalFriends.Application.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AnimalFriends.Api.Controllers;

[Route("customers")]
[SwaggerResponse(500, "An internal server error has occurred")]
public class CustomerController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CustomerController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("")]
    [SwaggerOperation(
        Description = "Create a new customer",
        Summary = "Creates a new customer from the given details and returns the Id")]
    [SwaggerResponse(201, "The customer was created successfully", typeof(int))]
    [SwaggerResponse(400, "The input customer data is not valid")]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var command = _mapper.Map<CreateCustomerCommand>(request);

        var id = await _mediator.Send(command);

        return Created($"/customers/{id}", id);
    }
}