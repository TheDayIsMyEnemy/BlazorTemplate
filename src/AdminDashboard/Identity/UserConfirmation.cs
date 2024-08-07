using Microsoft.AspNetCore.Identity;

namespace AdminDashboard.Identity
{
    public class UserConfirmation : IUserConfirmation<User>
    {
        public Task<bool> IsConfirmedAsync(UserManager<User> manager, User user)
            => Task.FromResult(user.AccountStatus == UserAccountStatus.Enabled);
    }
}
