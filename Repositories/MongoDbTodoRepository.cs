using CalenTaskApi.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CalenTaskApi.Respositories
{
    public class MongoDbTodoRepository : ITodoRepository
    {
        private string databaseName = "calentask";
        private string collectionName = "todo";
        private readonly IMongoCollection<Todo> todoCollection;
        private readonly FilterDefinitionBuilder<Todo> filterBuilder = Builders<Todo>.Filter;
        public MongoDbTodoRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            todoCollection = database.GetCollection<Todo>(collectionName);
        }

        public async Task <IEnumerable<Todo>> GetTodoAsync()
        {
            return await todoCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Todo> GetTodoAsync(Guid id)
        {
            var filter = filterBuilder.Eq(todo => todo.Id, id);
            return await todoCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task PostTodoAsync(Todo todo)
        {
            await todoCollection.InsertOneAsync(todo);
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            var filter = filterBuilder.Eq(existingTodo => existingTodo.Id, todo.Id);
            await todoCollection.ReplaceOneAsync(filter, todo);
        }

        public async Task DeleteTodoAsync(Guid id)
        {
            var filter = filterBuilder.Eq(todo => todo.Id, id);
            await todoCollection.DeleteOneAsync(filter);
        }

    }
}