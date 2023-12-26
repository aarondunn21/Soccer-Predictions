using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoccerSite2.Data;
using SoccerSite2.FixturesById;
using SoccerSite2.LeagueOddsById;
using SoccerSite2.Models;
using System.ComponentModel;
using System.Net.Http.Headers;
using Microsoft.AspNet.Identity;
using System.Collections.Specialized;

namespace SoccerSite2.Controllers
{
    public class TodayBetsController : Controller
    {

        private readonly ApplicationDbContext _db;

        public TodayBetsController(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<ActionResult> Index()
        {
            Random ran = new Random();

            User theUser = _db.User.SingleOrDefault(x => x.UserMatch == User.Identity.GetUserId());
            if (theUser == null)
            {
                User myUser = new User();
                myUser.Balance = 10000;
                myUser.UserMatch = User.Identity.GetUserId();
                myUser.Id = ran.Next().ToString();
                _db.User.Add(myUser);
                _db.SaveChanges();
            }

            LeagueOddsEvent result = null;

            var isToday = _db.TodayBets.SingleOrDefault(c => c.CreatedDateTime == DateTime.Today);
            if (isToday == null)
            {
                _db.Bet.RemoveRange(_db.Bet);
                TodayBets today = new TodayBets();
                today.CreatedDateTime = DateTime.Today;
                /*                today.Id = random.Next();*/

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://api-football-v1.p.rapidapi.com/v3/odds?league=39&season=2023&timezone=America%2FNew_York&page=2"),
                    Headers =
                {
                    { "X-RapidAPI-Key", "2ba5bae8c0msh24bee634dc879acp149f9ajsndb1256776155" },
                    { "X-RapidAPI-Host", "api-football-v1.p.rapidapi.com" },
                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    LeagueOddsEvent oddRespsonse = JsonConvert.DeserializeObject<LeagueOddsEvent>(body);

                    foreach (var x in oddRespsonse.response)
                    {
                        Bet myBet = new Bet();
                        myBet.Fixture = x.fixture.id;
                        myBet.TodayBets = today;
                        myBet.HomeOdd = Convert.ToDouble(x.bookmakers[0].bets[0].values[0].odd);
                        myBet.DrawOdd = Convert.ToDouble(x.bookmakers[0].bets[0].values[1].odd);
                        myBet.AwayOdd = Convert.ToDouble(x.bookmakers[0].bets[0].values[2].odd);
                        myBet.date = DateTime.Parse(x.fixture.date);


                        var fixClient = new HttpClient();
                        var fixRequest = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri($"https://api-football-v1.p.rapidapi.com/v3/fixtures?id={myBet.Fixture}&timezone=America%2FNew_York"),
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

                            DateTime myDate = (DateTime)fixRespsonse.response[0].fixture.date;

                            myBet.BetName = $"{fixRespsonse.response[0].teams.home.name} vs {fixRespsonse.response[0].teams.away.name} " +
                                $"({myDate.ToString("f")})";

                            _db.Add(myBet);
                            _db.SaveChanges();
                        }
                    }

                }
            }

            var todaysList = _db.TodayBets.SingleOrDefault(c => c.CreatedDateTime == DateTime.Today);
            var responseList = (IEnumerable<Bet>)_db.Bet.Where(x => x.TodayBets.Id == todaysList.Id);
            var sortedList = responseList.OrderBy(x => x.date);
            var sortedDateList = sortedList.Where(u => u.date > DateTime.Now);


            return View(sortedDateList);
            }

        public IActionResult BetView(int id) { 
            Bet chosenBet = _db.Bet.FirstOrDefault(x => x.BetId == id);
                
            MainPageModel model = new MainPageModel();
            model.betModel = chosenBet;
            model.userModel = _db.User.FirstOrDefault(x => x.UserMatch == User.Identity.GetUserId());



            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BetView(MainPageModel chosenBet)
        {
            MainPageModel mpm = (MainPageModel)chosenBet;


            mpm.userBetModel.Result = false;
            mpm.userBetModel.Completed = false;
            mpm.userBetModel.Timestamp = DateTime.Now;
            mpm.userBetModel.UserId= User.Identity.GetUserId();
            mpm.userBetModel.TeamChoice = "Draw";


            Bet betChoice = _db.Bet.FirstOrDefault(x => x.Fixture == mpm.userBetModel.BetSearchId);

            if (betChoice.HomeOdd == mpm.userBetModel.Odds)
            {
                mpm.userBetModel.TeamChoice = "Home Win";
            }
            else if(betChoice.AwayOdd == mpm.userBetModel.Odds)
            {
                mpm.userBetModel.TeamChoice = "Away Win";
            }
            else
            {
                mpm.userBetModel.TeamChoice = "Draw";
            }



            _db.Add(mpm.userBetModel);
            
            User myUser = _db.User.FirstOrDefault(x => x.UserMatch == User.Identity.GetUserId());

            if ((myUser.Balance - mpm.userBetModel.Amount) >= 0)
            {
                myUser.Balance -= mpm.userBetModel.Amount;
            }
            else
            {
                return RedirectToAction("Index", "UserBets", new { area = "" });
            }
            


            myUser.Balance = Math.Round(myUser.Balance, 2);


            _db.User.Update(myUser);

            _db.SaveChanges();

            return RedirectToAction("Index", "UserBets", new {area = ""});
        }




        }
    }

