using DataBase.Entietes;
using DataBase.Extensions;
using DataBase.StaticFiles;
using Microsoft.Data.Sqlite;
using System.Net.Sockets;

namespace Autotest.Mvc.Repositories
{
    public class TicketRepository
    {
        private readonly SqliteConnection _connection;

        public TicketRepository()
        {
            _connection = new SqliteConnection(ConnectionString.DataBaseName);
            _connection.Open();
            CreateTicketTable();
        }

        private void CreateTicketTable()
        {
            var command = _connection.CreateCommand();
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS tickets_uzb (id INTEGER PRIMARY KEY AUTOINCREMENT, user_id TEXT, " +
                "questions_count INTEGER, start_index INTEGER, current_ticket_index INTEGER, date BIGINT)";
            command.ExecuteNonQuery();
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS ticket_answers_uzb (id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "ticket_id INTEGER, question_index INTEGER, choice_index INTEGER, correct_index INTEGER)";
            command.ExecuteNonQuery();


            command.CommandText =
                "CREATE TABLE IF NOT EXISTS tickets_rus (id INTEGER PRIMARY KEY AUTOINCREMENT, user_id TEXT, " +
                "questions_count INTEGER, start_index INTEGER, current_ticket_index INTEGER, date BIGINT)";
            command.ExecuteNonQuery();
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS ticket_answers_rus (id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "ticket_id INTEGER, question_index INTEGER, choice_index INTEGER, correct_index INTEGER)";
            command.ExecuteNonQuery();


            command.CommandText =
                "CREATE TABLE IF NOT EXISTS tickets_krill (id INTEGER PRIMARY KEY AUTOINCREMENT, user_id TEXT, " +
                "questions_count INTEGER, start_index INTEGER, current_ticket_index INTEGER, date BIGINT)";
            command.ExecuteNonQuery();
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS ticket_answers_krill (id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "ticket_id INTEGER, question_index INTEGER, choice_index INTEGER, correct_index INTEGER)";
            command.ExecuteNonQuery();
        }

        public void AddTicket(Ticket ticket,string language)
        {
            AddTicketUzb(ticket);
            AddTicketKrill(ticket);
            AddTicketRus(ticket);
        }

        private void AddTicketUzb(Ticket ticket)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO tickets_uzb(user_id , questions_count , start_index , current_ticket_index  , date) " +
                                  "VALUES (@user_id, @questions_count,@start_index, @current_ticket_index, @date)";
            command.Parameters.AddWithValue("user_id", ticket.UserId);
            command.Parameters.AddWithValue("questions_count", ticket.QuestionsCount);
            command.Parameters.AddWithValue("start_index", ticket.StartIndex);
            command.Parameters.AddWithValue("current_ticket_index", ticket.CurrentQuestionIndex);
            command.Parameters.AddWithValue("date", ticket.Date.Ticks);
            command.Prepare();
            command.ExecuteNonQuery();
        }
        private void AddTicketRus(Ticket ticket)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO tickets_rus(user_id , questions_count , start_index , current_ticket_index  , date) " +
                                  "VALUES (@user_id, @questions_count,@start_index, @current_ticket_index, @date)";
            command.Parameters.AddWithValue("user_id", ticket.UserId);
            command.Parameters.AddWithValue("questions_count", ticket.QuestionsCount);
            command.Parameters.AddWithValue("start_index", ticket.StartIndex);
            command.Parameters.AddWithValue("current_ticket_index", ticket.CurrentQuestionIndex);
            command.Parameters.AddWithValue("date", ticket.Date.Ticks);
            command.Prepare();
            command.ExecuteNonQuery();
        }
        private void AddTicketKrill(Ticket ticket)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO tickets_krill(user_id , questions_count , start_index , current_ticket_index  , date) " +
                                  "VALUES (@user_id, @questions_count,@start_index, @current_ticket_index, @date)";
            command.Parameters.AddWithValue("user_id", ticket.UserId);
            command.Parameters.AddWithValue("questions_count", ticket.QuestionsCount);
            command.Parameters.AddWithValue("start_index", ticket.StartIndex);
            command.Parameters.AddWithValue("current_ticket_index", ticket.CurrentQuestionIndex);
            command.Parameters.AddWithValue("date", ticket.Date.Ticks);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateTicket(Ticket ticket,string language)
        {
            switch (language)
            {
                case "krill":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "UPDATE tickets_krill SET current_ticket_index = @i, date = @d where WHERE id = @id ";
                    command.Parameters.AddWithValue("i", ticket.CurrentQuestionIndex);
                    command.Parameters.AddWithValue("d", ticket.Date.Ticks);
                    command.Parameters.AddWithValue("id", ticket.Id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
                case "rus":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "UPDATE tickets_rus SET current_ticket_index = @i, date = @d where WHERE id = @id ";
                    command.Parameters.AddWithValue("i", ticket.CurrentQuestionIndex);
                    command.Parameters.AddWithValue("d", ticket.Date.Ticks);
                    command.Parameters.AddWithValue("id", ticket.Id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
                default:
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "UPDATE tickets_uzb SET current_ticket_index = @i, date = @d where WHERE id = @id ";
                    command.Parameters.AddWithValue("i", ticket.CurrentQuestionIndex);
                    command.Parameters.AddWithValue("d", ticket.Date.Ticks);
                    command.Parameters.AddWithValue("id", ticket.Id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
            }
        }

        public void DeleteTicketAnswers(int ticketId,string language)
        {
            switch (language)
            {
                case "krill":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "DELETE FROM ticket_answers_krill WHERE ticket_id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
                case "rus":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "DELETE FROM ticket_answers_rus WHERE ticket_id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
                default:
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "DELETE FROM ticket_answers_uzb WHERE ticket_id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
            }
        }

        public Ticket? GeTicket(int ticketId,string language)
        {
            switch (language)
            {
                case "krill":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM tickets_krill WHERE id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var ticket = reader.GetTicket();
                        ticket.Answers = GetTicketAnswers(ticket.Id,language);

                        reader.Close();

                        return ticket;
                    }
                    reader.Close();
                    return null;
                }
                case "rus":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM tickets_rus WHERE id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var ticket = reader.GetTicket();
                        ticket.Answers = GetTicketAnswers(ticket.Id, language);

                        reader.Close();

                        return ticket;
                    }
                    reader.Close();
                    return null;
                }
                default:
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM tickets_uzb WHERE id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var ticket = reader.GetTicket();
                        ticket.Answers = GetTicketAnswers(ticket.Id, language);

                        reader.Close();

                        return ticket;
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public List<Ticket> GetTicketsList(string userId,string language)
        {
            switch (language)
            {
                case "krill":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM tickets_krill WHERE user_id = @id";
                    command.Parameters.AddWithValue("id", userId);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    var tickets = new List<Ticket>();
                    while (reader.Read())
                    {
                        var ticket = (reader.GetTicket());
                        ticket.Answers = GetTicketAnswers(ticket.Id, language);
                        tickets.Add(ticket);
                    }
                    reader.Close();
                    return tickets;
                }
                case "rus":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM tickets_rus WHERE user_id = @id";
                    command.Parameters.AddWithValue("id", userId);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    var tickets = new List<Ticket>();
                    while (reader.Read())
                    {
                        var ticket = (reader.GetTicket());
                        ticket.Answers = GetTicketAnswers(ticket.Id, language);
                        tickets.Add(ticket);
                    }
                    reader.Close();
                    return tickets;
                }
                default:
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM tickets_uzb WHERE user_id = @id";
                    command.Parameters.AddWithValue("id", userId);
                    command.Prepare();
                    var reader = command.ExecuteReader();
                    var tickets = new List<Ticket>();
                    while (reader.Read())
                    {
                        var ticket = (reader.GetTicket());
                        ticket.Answers = GetTicketAnswers(ticket.Id, language);
                        tickets.Add(ticket);
                    }
                    reader.Close();
                    return tickets;
                }
            }
        }
        public void AddTicketAnswer(TicketQuestionAnswer answer,string language)
        {
            switch (language)
            {
                case "krill":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "INSERT INTO ticket_answers_krill(ticket_id, question_index, choice_index, correct_index)" +
                                          " VALUES(@ticket_id, @question_index, @choice_index, @correct_index)";
                    command.Parameters.AddWithValue("ticket_id", answer.TicketId);
                    command.Parameters.AddWithValue("question_index", answer.QuestionIndex);
                    command.Parameters.AddWithValue("choice_index", answer.ChoiceIndex);
                    command.Parameters.AddWithValue("correct_index", answer.CorrectIndex);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
                case "rus":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "INSERT INTO ticket_answers_rus(ticket_id, question_index, choice_index, correct_index)" +
                                          " VALUES(@ticket_id, @question_index, @choice_index, @correct_index)";
                    command.Parameters.AddWithValue("ticket_id", answer.TicketId);
                    command.Parameters.AddWithValue("question_index", answer.QuestionIndex);
                    command.Parameters.AddWithValue("choice_index", answer.ChoiceIndex);
                    command.Parameters.AddWithValue("correct_index", answer.CorrectIndex);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
                default:
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "INSERT INTO ticket_answers_uzb(ticket_id, question_index, choice_index, correct_index)" +
                                          " VALUES(@ticket_id, @question_index, @choice_index, @correct_index)";
                    command.Parameters.AddWithValue("ticket_id", answer.TicketId);
                    command.Parameters.AddWithValue("question_index", answer.QuestionIndex);
                    command.Parameters.AddWithValue("choice_index", answer.ChoiceIndex);
                    command.Parameters.AddWithValue("correct_index", answer.CorrectIndex);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    break;
                }
            }
        }

        private List<TicketQuestionAnswer> GetTicketAnswers(int  ticketId,string language)
        {
            switch (language)
            {
                case "krill":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM ticket_answers_krill WHERE ticket_id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    var ticketAnswers = new List<TicketQuestionAnswer>();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var ticketAnswer = reader.GetTicketAnswer();
                        ticketAnswers.Add(ticketAnswer);
                    }
                    reader.Close();
                    return ticketAnswers;
                }
                case "rus":
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM ticket_answers_rus WHERE ticket_id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    var ticketAnswers = new List<TicketQuestionAnswer>();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var ticketAnswer = reader.GetTicketAnswer();
                        ticketAnswers.Add(ticketAnswer);
                    }
                    reader.Close();
                    return ticketAnswers;
                }
                default:
                {
                    var command = _connection.CreateCommand();
                    command.CommandText = "SELECT * FROM ticket_answers_uzb WHERE ticket_id = @id";
                    command.Parameters.AddWithValue("id", ticketId);
                    command.Prepare();
                    var ticketAnswers = new List<TicketQuestionAnswer>();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var ticketAnswer = reader.GetTicketAnswer();
                        ticketAnswers.Add(ticketAnswer);
                    }
                    reader.Close();
                    return ticketAnswers;
                }
            }
        }
    }
}
