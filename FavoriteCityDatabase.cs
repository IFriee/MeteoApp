using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace MeteoApp
{
    public class FavoriteCityDatabase
    {
        readonly SQLiteAsyncConnection _database;

        // Constructeur de la classe
        public FavoriteCityDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            // Créer la table FavoriteCity si elle n'existe pas déjà
            _database.CreateTableAsync<FavoriteCity>().Wait();
        }

        // Récupère toutes les villes préférées stockées dans la base de données
        public Task<List<FavoriteCity>> GetFavoriteCitiesAsync()
        {
            return _database.Table<FavoriteCity>().ToListAsync();
        }

        // Récupère une ville préférée spécifique à partir de son ID
        public Task<FavoriteCity> GetFavoriteCityAsync(int id)
        {
            return _database.Table<FavoriteCity>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        // Enregistre une ville préférée dans la base de données
        public Task<int> SaveFavoriteCityAsync(FavoriteCity favoriteCity)
        {
            if (favoriteCity.ID != 0)
            {
                // Mettre à jour une ville préférée existante
                return _database.UpdateAsync(favoriteCity);
            }
            else
            {
                // Ajouter une nouvelle ville préférée
                return _database.InsertAsync(favoriteCity);
            }
        }

        // Supprime une ville préférée de la base de données
        public Task<int> DeleteFavoriteCityAsync(FavoriteCity favoriteCity)
        {
            return _database.DeleteAsync(favoriteCity);
        }
    }
}
