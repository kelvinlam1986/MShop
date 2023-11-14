using MongoDB.Driver;
using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;

namespace MShop.User.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<CreateUser> _collection => _database.GetCollection<CreateUser>("users");

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<UserCreated> AddUser(CreateUser user)
        {
            await _collection.InsertOneAsync(user);
            return new UserCreated { ContactNo = user.ContactNo, EmailId = user.EmailId, Password = user.Password, Username = user.Username, UserId = user.UserId };
        }

        public async Task<UserCreated> GetUser(CreateUser user)
        {
            var result = _collection.AsQueryable().Where(x => x.Username == user.Username).FirstOrDefault();
            await Task.CompletedTask;
            return new UserCreated { ContactNo = result.ContactNo, EmailId = result.EmailId, Password = user.Password, Username = user.Username, UserId = user.UserId };

        }
    }
}
