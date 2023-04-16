using Autotest.Mvc.Models;
using Newtonsoft.Json;

namespace Autotest.Mvc.Services;

public class QuestionService
{

    public List<QuestionModel> Questions;

    public int TicketQuestionsCount => 10;
    public int TicketsCount => Questions.Count / TicketQuestionsCount;

    public QuestionService()
    {
        LoadJson("uz");

        Questions ??= new List<QuestionModel>();
    }

    public void LoadJson(string language)
    {
        var jsonPath = "uzlotin.json";

        switch (language)
        {
            case "uzb": jsonPath = "uzlotin.json"; break;
            case "krill": jsonPath = "uzkiril.json"; break;
            case "rus": jsonPath = "rus.json"; break;
        }

        var path = Path.Combine("JsonData", jsonPath);

        if (!File.Exists(path)) return;
        var json = System.IO.File.ReadAllText(path);
        Questions = JsonConvert.DeserializeObject<List<QuestionModel>>(json)!;
    }
}