using DataBase.Entietes;
using DataBase.Extensions;
using DataBase.StaticFiles;
using Microsoft.Data.Sqlite;

namespace Autotest.Mvc.Repositories
{
    public class UserRepository
    {
        private readonly SqliteConnection Connection;
        private readonly  TicketRepository _ticketRepository;
        public UserRepository(TicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
            Connection = new SqliteConnection(ConnectionString.DataBaseName);
            Connection.Open();
            CreateUserTable();
        }

        private void CreateUserTable()
        {
            var command = Connection.CreateCommand();
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS users(id TEXT UNIQUE, username TEXT NOT NULL, password TEXT, name TEXT," +
                " photo_url TEXT, current_ticket_index INTEGER PRIMARY KEY AUTOINCREMENT DEFAULT 0,language TEXT)";
            command.ExecuteNonQuery();
        }

        public void AddUser(User user)
        {
            var command = Connection.CreateCommand();
            command.CommandText =
                "INSERT INTO users(id,username,password,name,photo_url, language) " +
                "VALUES (@id,@username,@password,@name,@photo_url,@language)";
            command.Parameters.AddWithValue("id", user.Id);
            command.Parameters.AddWithValue("username", user.Username);
            command.Parameters.AddWithValue("password", user.Password);
            command.Parameters.AddWithValue("name",user.Name);
            command.Parameters.AddWithValue("photo_url", user.PhotoPath);
            command.Parameters.AddWithValue("language", user.Language);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public User? GetUserById(string id)
        {
            return GetUser("id", id);
        }
        public User? GetUserByUsername(string username)
        {
            return GetUser("username", username);
        }

        public void UpdateUser(User user)
        {
            var command = Connection.CreateCommand();
            command.CommandText =
                "UPDATE users SET username = @username, password = @password, name = @name, " +
                "photo_url = @photo_url, current_ticket_index = @index WHERE id = @id";
            command.Parameters.AddWithValue("id", user.Id);
            command.Parameters.AddWithValue("username", user.Name);
            command.Parameters.AddWithValue("password", user.Password);
            command.Parameters.AddWithValue("name", user.Name);
            command.Parameters.AddWithValue("photo_url", user.PhotoPath);
            command.Parameters.AddWithValue("index", user.CurrentTicketIndex);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateUserLanguage(User user,string language)
        {
            var command = Connection.CreateCommand();
            command.CommandText = "UPDATE users SET language = @language WHERE id = @id";
            command.Parameters.AddWithValue("id", user.Id);
            command.Parameters.AddWithValue("language", language);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        private User? GetUser(string paramName, string paramValue)
        {
            var command = Connection.CreateCommand();
            command.CommandText = $"SELECT *FROM users WHERE {paramName} = @p";
            command.Parameters.AddWithValue("p", paramValue);
            command.Prepare();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = reader.GetUser();
                user.CurrentTicket = user.CurrentTicketIndex == null ? null : _ticketRepository.GeTicket(user.CurrentTicketIndex!.Value,user.Language);
                user.Tickets = _ticketRepository.GetTicketsList(user.Id,user.Language);
                reader.Close();
                return user;
            }
            reader.Close();
            return null;
        }

        public List<User>? GetUsers()
        {
            var command = Connection.CreateCommand();
            command.CommandText = $"SELECT *FROM users ORDER BY id DESC LIMIT 10";
            List<User> users = new List<User>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = reader.GetUser();

                user.CurrentTicket = user.CurrentTicketIndex == null ? null : _ticketRepository.GeTicket(user.CurrentTicketIndex!.Value, user.Language);
                user.Tickets = _ticketRepository.GetTicketsList(user.Id, user.Language);
                 users.Add(user);
            }
            reader.Close();
            return users;
        }
    }
}
