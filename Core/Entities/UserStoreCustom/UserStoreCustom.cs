using Kurskcartuning.Server_v2.Core.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kurskcartuning.Server_v2.Core.Entities.UserStoreCustom;

public class UserStoreCustom : UserStore<ApplicationUser>
{
    public UserStoreCustom(ApplicationDbContext context, IdentityErrorDescriber describer = null)
        : base(context, describer)
    {

    }
    public virtual Task<ApplicationUser> FindByPhoneNumberAsync(string Phone, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return Users.FirstOrDefaultAsync(u => u.Phone == Phone, cancellationToken);
    }
}

