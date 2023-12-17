using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoccerSite2.Data;
using SoccerSite2.FixturesById;
using SoccerSite2.Models;

namespace SoccerSite2.Controllers
{
    public class UserBetsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserBetsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var userBetList = (IEnumerable<UserBet>)_db.UserBets.Where(x => x.UserId == User.Identity.GetUserId());

            UserBetsPageModel model = new UserBetsPageModel();

            model.Bets = userBetList;
            model.userAccount = _db.User.FirstOrDefault(x => x.UserMatch == User.Identity.GetUserId());


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(UserBetsPageModel x)
        {
            var betList = (IEnumerable<UserBet>)_db.UserBets;

            foreach (var bet in betList)
            {
                if (bet.Completed == false)
                {
                    Bet myBet = new Bet();
                    var client = new HttpClient();
                    var fixRequest = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"https://api-football-v1.p.rapidapi.com/v3/fixtures?id={bet.BetSearchId}&timezone=America%2FNew_York"),
                        Headers =
                        {
                            { "X-RapidAPI-Key", "2ba5bae8c0msh24bee634dc879acp149f9ajsndb1256776155" },
                            { "X-RapidAPI-Host", "api-football-v1.p.rapidapi.com" },
                        },
                    };
                    using (var fix = await client.SendAsync(fixRequest))
                    {
                        fix.EnsureSuccessStatusCode();
                        var fixBody = await fix.Content.ReadAsStringAsync();

                        FixtureIdEvent fixRespsonse = JsonConvert.DeserializeObject<FixtureIdEvent>(fixBody);

                        if (fixRespsonse.response[0].fixture.status.@short == "FT") {
                            if (fixRespsonse.response[0].teams.home.winner == true)
                            {
                                var getUserBet = (IEnumerable<UserBet>)_db.UserBets.Where(x => x.BetSearchId == bet.BetSearchId);
                                foreach(var n in getUserBet)
                                {
                                    n.Completed = true;
                                    if(n.TeamChoice == "Home Win")
                                    {
                                        n.Result = true;
                                        var winnerUser = _db.User.FirstOrDefault(x => x.UserMatch == n.UserId);
                                        winnerUser.Balance += n.Amount * n.Odds;
                                        _db.User.Update(winnerUser);
                                    }
                                    _db.UserBets.Update(n);
                                }
                            }
                            else if (fixRespsonse.response[0].teams.away.winner == true)
                            {
                                var getUserBet = (IEnumerable<UserBet>)_db.UserBets.Where(x => x.BetSearchId == bet.BetSearchId);
                                foreach (var n in getUserBet)
                                {
                                    n.Completed = true;
                                    if (n.TeamChoice == "Away Win")
                                    {
                                        n.Result = true;
                                        var winnerUser = _db.User.FirstOrDefault(x => x.UserMatch == n.UserId);
                                        winnerUser.Balance += n.Amount * n.Odds;
                                        _db.User.Update(winnerUser);
                                    }
                                    _db.UserBets.Update(n);
                                }
                            }
                            else
                            {
                                var getUserBet = (IEnumerable<UserBet>)_db.UserBets.Where(x => x.BetSearchId == bet.BetSearchId);
                                foreach (var n in getUserBet)
                                {
                                    n.Completed = true;
                                    if (n.TeamChoice == "Draw")
                                    {
                                        n.Result = true;
                                        var winnerUser = _db.User.FirstOrDefault(x => x.UserMatch == n.UserId);
                                        winnerUser.Balance += n.Amount * n.Odds;
                                        _db.User.Update(winnerUser);
                                    }
                                    _db.UserBets.Update(n);
                                }
                            }
                        }
                    }
                }
            }
            _db.SaveChanges();

            var userBetList = (IEnumerable<UserBet>)_db.UserBets.Where(x => x.UserId == User.Identity.GetUserId());

            UserBetsPageModel model = new UserBetsPageModel();

            model.Bets = userBetList;
            model.userAccount = _db.User.FirstOrDefault(x => x.UserMatch == User.Identity.GetUserId());


            return View(model);
        }
        
        public IActionResult GameStatsView(int fixtureId)
        {
            GameStats model = new GameStats();
            model.fixture = fixtureId;
            return View(model);
        }
    }
}
