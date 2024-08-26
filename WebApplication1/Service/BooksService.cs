using MongoDB.Driver;
using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.Interface;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;


namespace BookStoreApi.Services
{
    public class BooksService : MongoRepository<Book>, IBooksService
    {

        public BooksService(IOptions<BookStoreDatabaseSettings> databaseSettings) : base(databaseSettings)
        {

        }


        public List<String> ListBookByAuthor()
        {
            List<String> result = new List<string>();
            var queryResult = _collection.AsQueryable().GroupBy(
                x => x.Author,
                x => x.BookName,
                (AuthorName, Names) => new { Author = AuthorName, BookName = Names.ToList() });

            foreach (var line in queryResult)
            {
                string booksFormatted = "[" + string.Join(", ", line.BookName.Select(name => $"{name}")) + "]";
                result.Add(line.Author + ": " + booksFormatted);
                Console.Write(line);
            }  

            return result;
        }
        
        public List<String> ListBookByCategory()
        {
            List<String> result = new List<string>();
            var queryResult = _collection.AsQueryable().GroupBy(
                x => x.Category,
                x => x.BookName,
                (CategoryName, Names) => new { Category = CategoryName, BookName = Names.ToList() });

            foreach (var line in queryResult)
            {
                string booksFormatted = "[" + string.Join(", ", line.BookName.Select(name => $"{name}")) + "]";
                result.Add(line.Category + ": " + booksFormatted);
                Console.Write(line);
            }

            return result;
        }

        public async Task<List<Book>> SearchBooks(string? title, string? author, string? category)
        {
            var filterBuilder = Builders<Book>.Filter;
            var filter = filterBuilder.Empty; // Start with an empty filter

            // Add conditions to the filter dynamically
            //if (!string.IsNullOrEmpty(title))
            //{
            filter &= filterBuilder.Where(x => (title == null || x.BookName == title)
                                            &&(author == null || x.Author == author)
                                            &&(category == null || x.Category == category));
            //}

            //if (!string.IsNullOrEmpty(author))
            //{
            //    filter &= filterBuilder.Eq(x => x.Author, author);
            //}

            //if (!string.IsNullOrEmpty(category))
            //{
            //    filter &= filterBuilder.Eq(x => x.Category, category);
            //}

            if (filter == filterBuilder.Empty)
            {
                return await GetAsync();
            }

            var results = await _collection.Find(filter).ToListAsync();

           
            return results;
        }

    }
}