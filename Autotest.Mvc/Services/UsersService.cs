using Autotest.Mvc.Models;
using Autotest.Mvc.Repositories;
using DataBase.Entietes;

namespace Autotest.Mvc.Services;

public  class UsersService
{
    private const string UserIdCookieKey = "user_id";
    public readonly TicketRepository TicketRepository;
    public readonly UserRepository UserRepository;
    private readonly QuestionService QuestionService;

    public UsersService(TicketRepository ticketRepository, UserRepository userRepository, QuestionService questionService)
    {
        TicketRepository = ticketRepository;
        UserRepository = userRepository;
        QuestionService = questionService;
    }

    public  void Register(CreateUserModel createUser, HttpContext httpContext)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = createUser.Name!,
            Password = createUser.Password!,
            Username = createUser.Username!,
            PhotoPath = SavePhoto(createUser.Photo!),
        };
        CreateUserTickets(user);
        user.CurrentTicket = user.CurrentTicketIndex == null ? null : TicketRepository.GeTicket(user.CurrentTicketIndex!.Value, user.Language);
        user.Tickets = TicketRepository.GetTicketsList(user.Id, user.Language);


        UserRepository.AddUser(user);

        httpContext.Response.Cookies.Append(UserIdCookieKey, user.Id);
    }

    public  bool LogIn(SignInUserModel signInUserModel, HttpContext httpContext)
    {
        var user = UserRepository.GetUserByUsername(signInUserModel.Username!);

        if (user == null || user.Password != signInUserModel.Password)
            return false;

        httpContext.Response.Cookies.Append(UserIdCookieKey, user.Id);

        return true;
    }

    public  User? GetCurrentUser(HttpContext context)
    {
        if (context.Request.Cookies.ContainsKey(UserIdCookieKey))
        {
            var userId = context.Request.Cookies[UserIdCookieKey];
            var user = UserRepository.GetUserById(userId!);

            return user;
        }

        return null;
    }

    public  bool IsLoggedIn(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey(UserIdCookieKey)) return false;

        var userId = context.Request.Cookies[UserIdCookieKey];
        var user = UserRepository.GetUserById(userId!);

        return user != null;
    }


    public  void LogOut(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(UserIdCookieKey);
    }

    private void CreateUserTickets(User user)
    {
        for (var i = 0; i < QuestionService.TicketsCount; i++)
        {
            var startIndex = i * QuestionService.TicketQuestionsCount + 1;
            TicketRepository.AddTicket(new Ticket()
            {
                Id = i,
                UserId = user.Id,
                CurrentQuestionIndex = startIndex,
                StartIndex = startIndex,
                QuestionsCount = QuestionService.TicketQuestionsCount
            },user.Language);
        }
    }

    private  string SavePhoto(IFormFile file)
    {
        if (!Directory.Exists("wwwroot/UserImages"))
            Directory.CreateDirectory("wwwroot/UserImages");

        var fileName = Guid.NewGuid() + ".jpg";
        var ms = new MemoryStream();
        file.CopyTo(ms);
        System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "UserImages", fileName), ms.ToArray());

        return "/UserImages/" + fileName;
    }
}