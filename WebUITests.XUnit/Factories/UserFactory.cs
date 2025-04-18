using WebUITests.XUnit.Models;

namespace WebUITests.XUnit.Factories
{
    public static class UserFactory
    {
        public static User CreateAdminUser()
        {
            return new User.UserBuilder()
                .WithUsername("adminUser")
                .WithPassword("adminPass123")
                .WithEmail("admin@example.com")
                .IsAdmin(true)
                .Build();
        }

        public static User CreateStandardUser()
        {
            return new User.UserBuilder()
                .WithUsername("standardUser")
                .WithPassword("userPass123")
                .WithEmail("user@example.com")
                .IsAdmin(false)
                .Build();
        }
    }
}