namespace SoccerSite2.LeagueOddsById
{
    public class LeagueOddsEvent
    {
        public Response[] response { get; set; }
    }

    public class Response
    {
        public Fixture fixture { get; set; }
        public Bookmakers[] bookmakers { get; set; }
    }

    public class Fixture
    {
        public int id { get; set; }
        public string date { get; set; }
    }

    public class Bookmakers
    {
        public int id { get; set; }
        public string name { get; set; }

        public BookieBets[] bets { get; set; }
    }

    public class BookieBets
    {
        public int id { get; set; }
        public string name { get; set; }

        public Values[] values { get; set; }
    }

    public class Values
    {
        public string value { get; set; }
        public string odd { get; set; }
    }
}
