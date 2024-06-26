﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace NotX.Models.User
{
    public class EditUserPageModel
    {
        public int InputMemberID { get; set; }
        public User UserData { get; set; }

        public User EditUserData { get; set; }

        public class User
        {
            public ObjectId Id { get; set; }
            public int MemberID { get; set; }
            public string Name { get; set; }
            public string Account { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
            public string Introduction { get; set; }
            //Visitor = 0, Member = 1, Admin = 2
            public int Authentication { get; set; }
            public DateTime CreationTime { get; set; }
        }

        private readonly IMongoCollection<User> _collection;

        public EditUserPageModel()
        {
            string username = ConfigurationManager.AppSettings["Username"];
            string password = ConfigurationManager.AppSettings["Password"];
            string connectionString = $"mongodb+srv://{username}:{password}@20240411lion.ncaf5nq.mongodb.net/?retryWrites=true&w=majority&appName=20240411Lion";
            string databaseName = "20240400NotX";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<User>("Member");
        }

        public async Task<bool> GetUserDetail()
        {
            var filter = Builders<User>.Filter.Eq("MemberID", InputMemberID);
            UserData = await _collection.Find(filter).FirstOrDefaultAsync();

            return true;
        }
        
        public async Task<bool> UpdateUserDetail()
        {
            var filter = Builders<User>.Filter.Eq("MemberID", InputMemberID) ;
            var update = Builders<User>.Update
                .Set("Introduction", EditUserData.Introduction)
                .Set("PhoneNumber", EditUserData.PhoneNumber);

            await _collection.UpdateOneAsync(filter, update);

            return true;
        }

    }
}