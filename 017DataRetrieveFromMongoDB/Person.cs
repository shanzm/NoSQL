using MongoDB.Bson;

namespace _017DataRetrieveFromMongoDB
{
    internal class Person
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
    }
}