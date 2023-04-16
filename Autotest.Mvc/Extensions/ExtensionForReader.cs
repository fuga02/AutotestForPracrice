using DataBase.Entietes;
using Microsoft.Data.Sqlite;

namespace DataBase.Extensions;
    public static class ExtensionForReader
    {
        public static User GetUser(this SqliteDataReader reader)
        {
            return new User()
            {
                Id = reader.GetString(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Name = reader.GetString(3),
                PhotoPath = reader.GetString(4),
                CurrentTicketIndex = reader.GetInt32(5)
            };
        }
        public static Ticket GetTicket(this SqliteDataReader reader)
        {
            return new Ticket()
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetString(1),
                QuestionsCount = reader.GetInt32(2),
                StartIndex = reader.GetInt32(3),
                CurrentQuestionIndex = reader.GetInt32(4),
                Date = DateTime.FromFileTime(reader.GetInt64(5))
            };
        }
        public static TicketQuestionAnswer GetTicketAnswer(this SqliteDataReader reader)
        {
            return new TicketQuestionAnswer()
            {
                Id = reader.GetInt32(0),
                TicketId = reader.GetInt32(1),
                QuestionIndex = reader.GetInt32(2),
                ChoiceIndex = reader.GetInt32(3),
                CorrectIndex = reader.GetInt32(4),
            };
        }
    }
