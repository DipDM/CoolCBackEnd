using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UserCleanupService : IHostedService, IDisposable, IUserCleanupService
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public UserCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Run the cleanup process every 10 minutes
        _timer = new Timer(DeleteUnconfirmedUsers, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    // Now this method is public, as required by the interface
    public void DeleteUnconfirmedUsers(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var thirtyMinutesAgo = DateTime.UtcNow.AddMinutes(-30);

            var unconfirmedUsers = userManager.Users
                .Where(u => !u.EmailConfirmed && u.CreationTime <= thirtyMinutesAgo)
                .ToList(); // This should now work

            foreach (var user in unconfirmedUsers)
            {
                userManager.DeleteAsync(user).Wait();
            }
        }
    }


    public void Dispose()
    {
        _timer?.Dispose();
    }
}
