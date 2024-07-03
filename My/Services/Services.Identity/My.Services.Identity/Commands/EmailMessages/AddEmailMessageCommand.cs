using My.Application;
using My.Infrastructure.Grpc;
using My.Services.Identity.DTOs;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading;
using System.Threading.Tasks;

namespace My.Services.Identity.Commands.EmailMessages;

public class AddEmailMessageCommand : ICommand
{
    public EmailMessageDTO EmailMessage { get; set; }
}

public class AddEmailMessageCommandHandler : ICommandHandler<AddEmailMessageCommand>
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddEmailMessageCommandHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task HandleAsync(AddEmailMessageCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
