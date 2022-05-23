using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Timers;

namespace TgBotCours
{
    class Program
    {
        private static string token { get; set; } = "1971475561:AAFALIrkTCIksyhCAIvk3JoNDzJ3RAeDXOU";
        private static TelegramBotClient client;
        
        static string NameHero;
        static string answerOnJoke;
        static string answerHero;
        static string answerteam;
        static string answerOncompare;
        static string answer;
        static List<long> idMatch = new List<long>();

        public static long chatId;


        private static Timer aTimer;
        public static void Main(string[] args)
        {
            client = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(10)};
            var me = client.GetMeAsync().Result;
            Console.WriteLine($"Bot_Id: {me.Id} \nBot_Name: {me.FirstName} ");
            

            client.OnMessage += Bot_OnMessage ;
            client.StartReceiving();
            Console.ReadLine();
            client.StopReceiving();

        }
        
        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            chatId = message.Chat.Id;
            
            //Timer
            aTimer = new System.Timers.Timer();
            aTimer.Interval = /*300000*/10000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;


            var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
{
                    new [] // first row
                    {
                        InlineKeyboardButton.WithCallbackData("1","one" ),
                        InlineKeyboardButton.WithCallbackData("2","two"),
                    },
                    new [] // second row
                    {
                        InlineKeyboardButton.WithCallbackData("3","three"),
                        InlineKeyboardButton.WithCallbackData("4","four"),
                    }
                    });
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                if (message.Text == "/start")
                {
                    await client.SendTextMessageAsync(chatId, "Слава Україні!",replyMarkup: keyboard);
                    UserInfo(message);

                }

                if (message.Text == "/joke")
                {
                    Joke(chatId);
                    await client.SendTextMessageAsync(chatId, answerOnJoke);
                    UserInfo(message);
                    
                }
                
                if (message.Text.StartsWith("/herostats "))
                {
                    string[] Heroes_str = message.Text.Split(" ");
                    string NameHero = null;
                    List<string> HeroesList = new List<string>();
                    foreach (string items in Heroes_str)
                    {
                        HeroesList.Add(items);
                    }
                    HeroesList.Remove("/herostats");
                    foreach (string item in HeroesList)
                    {
                        NameHero = NameHero + item + " ";
                    }
                    HeroStats(NameHero);
                    await client.SendTextMessageAsync(chatId, answerHero);
                    UserInfo(message);
                }

                if (message.Text.StartsWith("/teamstats "))
                {
                    string[] Team_str = message.Text.Split(" ");
                    string NameTeam = null;
                    List<string> TeamList = new List<string>();
                    foreach (string items in Team_str)
                    {
                        TeamList.Add(items);
                    }
                    TeamList.Remove("/teamstats");
                    foreach (string item in TeamList)
                    {
                        NameTeam = NameTeam + item + " ";
                    }
                    TeamStats(NameTeam);
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }

                if (message.Text.StartsWith("/teamtop "))
                {
                    string[] Top_str = message.Text.Split(" ");
                    int TopCount = 0;
                    List<string> TopList = new List<string>();
                    foreach (string items in Top_str)
                    {
                        TopList.Add(items);
                    }
                    TopList.Remove("/teamtop");
                    foreach (string item in TopList)
                    {
                        TopCount = Int32.Parse(item);
                    }

                    TeamTop(TopCount);
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }

                if (message.Text.StartsWith("/tournamentslist"))
                {
                    TournamentList();
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }

                if (message.Text.StartsWith("/teamadd "))
                {
                    string[] Team_str = message.Text.Split(" ");
                    string NameTeam = null;
                    List<string> TeamList = new List<string>();
                    foreach (string items in Team_str)
                    {
                        TeamList.Add(items);
                    }
                    TeamList.Remove("/teamadd");
                    foreach (string item in TeamList)
                    {
                        NameTeam = NameTeam + item + " ";
                    }
                    TeamAdd(NameTeam);
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }

                if (message.Text.StartsWith("/teamdel "))
                {
                    string[] Team_str = message.Text.Split(" ");
                    string NameTeam = null;
                    List<string> TeamList = new List<string>();
                    foreach (string items in Team_str)
                    {
                        TeamList.Add(items);
                    }
                    TeamList.Remove("/teamdel");
                    foreach (string item in TeamList)
                    {
                        NameTeam = NameTeam + item + " ";
                    }
                    TeamDel(NameTeam);
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }

                if (message.Text.StartsWith("/teampast"))
                {
                    TeamPast();
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }

                if (message.Text.StartsWith("/teamlive"))
                {
                    TeamLive();
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }


                if (message.Text.StartsWith("/heroescompare "))
                {
                    string[] Heroes_str = message.Text.Split(" ");
                    string NameHeroes = null;
                    List<string> HeroesList = new List<string>();
                    foreach (string items in Heroes_str)
                    {
                        HeroesList.Add(items);
                    }
                    HeroesList.Remove("/heroescompare");
                    foreach (string item in HeroesList)
                    {
                        NameHeroes = NameHeroes + item + " ";
                    }
                    HeroCompare(NameHeroes);
                    await client.SendTextMessageAsync(chatId, answerOncompare);
                    UserInfo(message);
                }
            }
            
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Live();
            client.SendTextMessageAsync(chatId, answer);
        }
        private static void TeamTop(int CountofTeams)
        {

            string url = "https://localhost:7102/Dota/Teamtop/" + CountofTeams;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            answerteam = response;

        }
        private static void HeroStats(string NameHero)
        {
            
            string url = "https://localhost:7102/Dota/HeroByName/" + NameHero;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            HeroDotaResponse Hero = JsonConvert.DeserializeObject<HeroDotaResponse>(response);
            answerHero = $"Hero Name: {Hero.localized_name}\r\n" +
                         $"Attack type: {Hero.attack_type}\r\n" +
                         $"Healt: {Hero.base_health}\r\n" +
                         $"Mana: {Hero.base_mana}\r\n" +
                         $"Armor: {Hero.base_armor}\r\n" +
                         $"Move speed: {Hero.move_speed}\r\n" +
                         $"Attack damage: {Hero.base_attack_min}-{Hero.base_attack_max}";

        }

        private static void TeamPast()
        {

            string url = "https://localhost:7102/Dota/TeamsMatchesPast";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            answerteam = response;

        }
        private static void TournamentList()
        {

            string url = "https://localhost:7102/Dota/Tournaments";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            answerteam = response;

        }

        private static void TeamLive()
        {

            string url = "https://localhost:7102/Dota/TeamsMatchesLive";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            if (response == "")
            {
                answerteam = "Зара live матчей цієї команди немає";
            }
            else
            {
                answerteam = response;
            }

        }

        private static async void Live()
        {

            string url = "https://localhost:7102/Dota/TeamsMatchesLive";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            var MatchDota = JsonConvert.DeserializeObject<List<LiveMatches>>(response);
            bool inList = false;
            string LiveMatches = null;
            foreach (var match in MatchDota)
            {
                foreach (var id in idMatch)
                {
                    if(id == match.match_id)
                    {
                        inList = true;
                    }
                }
                if (inList != true)
                {
                    LiveMatches = $"{LiveMatches}\r\n" +
                                            $"LIVE!!!!\r\n" +
                                            $"Dire :{match.team_name_dire}\t{match.dire_score}\r\n" +
                                            $"Radiant :{match.team_name_radiant}\t{match.radiant_score}\r\n" +
                                            $"Game time :{(double)match.game_time / 60}";
                    idMatch.Add(match.match_id);
                }
            }
            answer = LiveMatches;

        }
        private static void TeamStats(string NameTeam)
        {

            string url = "https://localhost:7102/Dota/TeambyName/" + NameTeam;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            TeamDotaResponse TeamDota = JsonConvert.DeserializeObject<TeamDotaResponse>(response);
            answerteam = $"Name: {TeamDota.name}\r\n" +
                         $"Rating: {TeamDota.rating}\r\n" +
                         $"Wins: {TeamDota.wins}\r\n" +
                         $"Losses: {TeamDota.losses}\r\n" +
                         $"";

        }
        private static void TeamAdd(string NameTeam)
        {

            string url = "https://localhost:7102/Dota/AddTeamName/" + NameTeam;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            answerteam = response;

        }

        private static void TeamDel(string NameTeam)
        {

            string url = "https://localhost:7102/Dota/DelTeam/" + NameTeam;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method= "DELETE";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            answerteam = response;

        }

        private static void HeroCompare(string NameHeroes)
        {

            string url = "https://localhost:7102/Dota/CompareHeroes/" + NameHeroes;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            answerOncompare = response;

        }

        private static async void Joke(long chatId)
        {
            string url = "https://v2.jokeapi.dev/joke/Any?blacklistFlags=nsfw,religious,political,racist,sexist,explicit&type=single";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            JokeResponse JokeRND = JsonConvert.DeserializeObject<JokeResponse>(response);
            answerOnJoke = JokeRND.joke;

        }

        private static void UserInfo(Telegram.Bot.Types.Message message)
        {
            string user = message.Chat.FirstName + " " + message.Chat.Username + " " + message.Date + " " + message.Text;
            Console.WriteLine(user);
        }

       
    }
}
