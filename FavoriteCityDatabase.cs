using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace MeteoApp
{
    public class FavoriteCityDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public FavoriteCityDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<FavoriteCity>().Wait();
        }

        public Task<List<FavoriteCity>> GetFavoriteCitiesAsync()
        {
            return _database.Table<FavoriteCity>().ToListAsync();
        }

        public Task<FavoriteCity> GetFavoriteCityAsync(int id)
        {
            return _database.Table<FavoriteCity>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveFavoriteCityAsync(FavoriteCity favoriteCity)
        {
            if (favoriteCity.ID != 0)
            {
                return _database.UpdateAsync(favoriteCity);
            }
            else
            {
                return _database.InsertAsync(favoriteCity);
            }
        }

        public Task<int> DeleteFavoriteCityAsync(FavoriteCity favoriteCity)
        {
            return _database.DeleteAsync(favoriteCity);
        }
    }
}
