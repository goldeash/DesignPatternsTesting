namespace WebUITests.NUnit.Models
{
    public class User
    {
        public string Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public bool IsAdmin { get; private set; }

        private User() { }

        public class UserBuilder
        {
            private readonly User _user = new User();

            public UserBuilder WithId(string id)
            {
                _user.Id = id;
                return this;
            }

            public UserBuilder WithUsername(string username)
            {
                _user.Username = username;
                return this;
            }

            public UserBuilder WithPassword(string password)
            {
                _user.Password = password;
                return this;
            }

            public UserBuilder WithEmail(string email)
            {
                _user.Email = email;
                return this;
            }

            public UserBuilder IsAdmin(bool isAdmin)
            {
                _user.IsAdmin = isAdmin;
                return this;
            }

            public User Build()
            {
                return _user;
            }
        }
    }
}