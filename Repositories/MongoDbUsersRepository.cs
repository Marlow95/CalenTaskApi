using CalenTaskApi.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CalenTaskApi.Respositories
{
    public class MongoDbUsersRepository : IUsersRepository
    {
        private string databaseName = "calentask";
        private string collectionName = "users";
        private readonly IMongoCollection<Users> usersCollection;
        private readonly FilterDefinitionBuilder<Users> filterBuilder = Builders<Users>.Filter;
        public MongoDbUsersRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            usersCollection = database.GetCollection<Users>(collectionName);
        }

        public async Task <IEnumerable<Users>> GetUsersAsync()
        {
            return await usersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Users> GetUsersAsync(Guid id)
        {
            var filter = filterBuilder.Eq(users => users.Id, id);
            return await usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<Users> GetLoginUserAsync(string username)
        {
            var filter = filterBuilder.Eq(users => users.UserName, username);
            return await usersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task PostUsersAsync(Users users)
        {
            await usersCollection.InsertOneAsync(users);
        }

        public async Task UpdateUserAsync(Users user)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Id, user.Id);
            await usersCollection.ReplaceOneAsync(filter, user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var filter = filterBuilder.Eq(user => user.Id, id);
            await usersCollection.DeleteOneAsync(filter);
        }
    }
}