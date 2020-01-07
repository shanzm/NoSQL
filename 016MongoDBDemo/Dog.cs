using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _016MongoDBDemo
{
    class Dog
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public Person Master { get; set; }
    }
}