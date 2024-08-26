using MongoDB.Driver;
using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.Interface;
using Microsoft.Extensions.Options;

namespace BookStoreApi.Services
{
    public class MagazineService : MongoRepository<Magazine>, IMagazineService
    {
        public MagazineService(IOptions<BookStoreDatabaseSettings> databaseSettings) : base(databaseSettings)
        {

        }
    }
}