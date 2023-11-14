using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Event.User;

namespace MShop.User.DataProvider.Repositories
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
            var result = await _collection.AsQueryable().FirstOrDefaultAsync(x => x.Username == user.Username);
            if (result != null)
            {
                return new UserCreated { ContactNo = result.ContactNo, EmailId = result.EmailId, Password = result.Password, Username = result.Username, UserId = result.UserId };
            }

            return new UserCreated();
            
        }

        public async Task<UserCreated> GetUserByUsername(string username)
        {
            var result = await _collection.AsQueryable().FirstOrDefaultAsync(x => x.Username == username);
            if (result != null)
            {
                return new UserCreated { ContactNo = result.ContactNo, EmailId = result.EmailId, Password = result.Password, Username = result.Username, UserId = result.UserId };
            }

            return new UserCreated();
        }
    }
}
