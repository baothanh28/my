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


namespace My.Services.Identity.Commands.SmsMessages;

public class AddSmsMessageCommand : ICommand
{
    public SmsMessageDTO SmsMessage { get; set; }
}

public class AddSmsMessageCommandHandler : ICommandHandler<AddSmsMessageCommand>
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddSmsMessageCommandHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task HandleAsync(AddSmsMessageCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
