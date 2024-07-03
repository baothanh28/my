using My.Application;
using My.Services.Identity.Entities;
using My.Services.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace My.Services.Identity.Commands.Users;

public class AddClaimCommand : ICommand
{
    public User User { get; set; }
    public UserClaim Claim { get; set; }
}

public class AddClaimCommandHandler : ICommandHandler<AddClaimCommand>
{
    private readonly IUserRepository _userRepository;

    public AddClaimCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(AddClaimCommand command, CancellationToken cancellationToken = default)
    {
        command.User.Claims.Add(command.Claim);
        await _userRepository.AddOrUpdateAsync(command.User);
        await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
