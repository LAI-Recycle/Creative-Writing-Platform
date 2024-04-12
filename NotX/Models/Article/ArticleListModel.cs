﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace NotX.Models.Article
{
    public class ArticleListModel
    {
        /// <summary>
        /// 文章清單
        /// </summary>
        public List<Article> ArticleList { get; set; }


        public class Article
        {
            public ObjectId Id { get; set; }
            public int ArticleId { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Content { get; set; }
            //0下架 1上架
            public int FavoriteNumber { get; set; }
            public int ClickNumber { get; set; }
            public int DisplayStatus { get; set; }
            public DateTime CreationTime { get; set; }
        }

        private readonly IMongoCollection<Article> _collection;

        public ArticleListModel()
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            string connectionString = "mongodb+srv://Lai:20240400@20240411lion.ncaf5nq.mongodb.net/?retryWrites=true&w=majority&appName=20240411Lion";
            string databaseName = "20240400NotX";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<Article>("Article");
        }

        public async Task<bool> GetArticleList()
        {
            ArticleList = await _collection.Find(new BsonDocument()).ToListAsync();

            return true;
        }
        /// <summary>
        /// 新增一篇文章
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddArticleDetail()
        {
            var articleId = await GetMaxArticleId();

            var article = new Article
            {
                Title = "南港",
                ArticleId = articleId,
                ClickNumber = 0,
                FavoriteNumber = 0,
                DisplayStatus = 1,
                Author = "Karla Chen",
                CreationTime = DateTime.Now,
                Content = "覺得看好來只是在"
            };

            await _collection.InsertOneAsync(article);

            return true;
        }
        /// <summary>
        /// 取得最新的ArticleId
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetMaxArticleId()
        {
            var maxArticle = await _collection.Find(new BsonDocument())
                                              .Sort(Builders<Article>.Sort.Descending(a => a.ArticleId))
                                              .Limit(1)
                                              .FirstOrDefaultAsync();

            if (maxArticle != null)
            {
                return maxArticle.ArticleId + 1;
            }
            else
            {
                return 1;
            }
        }
    }
}