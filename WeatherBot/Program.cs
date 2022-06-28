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
using Telegram.Bot.Types.Payments;

namespace TgBotCours
{
    class Program
    {
        private static string token { get; set; } = "1971475561:AAFALIrkTCIksyhCAIvk3JoNDzJ3RAeDXOU";
        private static string paytoken = "535936410:LIVE:1971475561_c25ea9f9-8eff-4463-acde-01bf9ce445de";
        private static TelegramBotClient client;
        private static Telegram.Bot.Args.CallbackQueryEventArgs ev;


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
            string info = "/start - Початок\r\n" +
                "/joke - Смішнявка\r\n" +
                "/herostats - Стата героя дота2 {/herostats назва_героя}\r\n " +
                "/teamstats - Стата команди дота2 {/teamstats назва_команди}\r\n " +
                "/teamtop - Топ команд за рейтенгом {/teamtop колово_місць}\r\n " +
                "/heroescompare - Порівняння 2 героїв {/heroescompare назва_гер1&назва_гер2}\r\n " +
                "/teamadd - Підписатися на команду {/teamadd назва_команди}\r\n " +
                "/teamdel - Відписатися від команди{/teamdel назва_команди}\r\n " +
                "/teamlive - Матчі команд лайв.\r\n " +
                "/tournamentslist - Список майбутніх турнірів\r\n" +
                "/donate\r\n";

            //Timer
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 10000;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
            {
                    new []
                    {
                        
                        InlineKeyboardButton.WithPayment("donate")
                    }
            }) ;
            List<LabeledPrice> prices = new List<LabeledPrice>() { new LabeledPrice("Donate", 10000) };
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                if (message.Text == "/start")
                {
                    await client.SendTextMessageAsync(chatId, "Слава Україні!");
                    UserInfo(message);

                }

                else if (message.Text == "/donate")
                {
                    await client.SendInvoiceAsync((int)chatId, "Hi", "Donate me pls", "abc", paytoken, "abc", "UAH", prices,replyMarkup: keyboard);
                    UserInfo(message);

                }

                else if (message.Text == "/joke")
                {
                    Joke(chatId);
                    await client.SendTextMessageAsync(chatId, answerOnJoke);
                    UserInfo(message);
                    
                }

                else if (message.Text.StartsWith("/herostats "))
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

                else if (message.Text.StartsWith("/teamstats "))
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

                else if (message.Text.StartsWith("/teamtop "))
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
                        try
                        {
                            TopCount = Int32.Parse(item);
                        }
                        catch(FormatException abc)
                        {
                            await client.SendTextMessageAsync(chatId, abc.Message);
                        }
                    }
                    if ((TopCount <= 0)||(TopCount>100))
                    {
                        await client.SendTextMessageAsync(chatId, "Козаче щось ти ввів не так. Cпробуй /info щоб зрозуміти");
                    }
                    else
                    {
                        TeamTop(TopCount);
                        await client.SendTextMessageAsync(chatId, answerteam);
                        UserInfo(message);
                    }
                }

                else if (message.Text.StartsWith("/tournamentslist"))
                {
                    TournamentList();
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }

                else if (message.Text.StartsWith("/teamadd "))
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

                else if (message.Text.StartsWith("/teamdel "))
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


                else if (message.Text.StartsWith("/teamlive"))
                {
                    TeamLive();
                    await client.SendTextMessageAsync(chatId, answerteam);
                    UserInfo(message);
                }


                else if (message.Text.StartsWith("/heroescompare "))
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

                else if (message.Text.StartsWith("/info"))
                {
                    await client.SendTextMessageAsync(chatId, info);
                }

                else
                {
                    await client.SendTextMessageAsync(chatId, "Козаче щось ти ввів не так. Cпробуй /info щоб зрозуміти");
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

            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/Teamtop/" + CountofTeams;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            if (response == null)
            {
                response = "Козаче щось ти ввів не так. Cпробуй /info щоб зрозуміти";
            }
            answerteam = response;

        }
        private static void HeroStats(string NameHero)
        {
            
            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/HeroByName/" + NameHero;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            HeroDotaResponse Hero = JsonConvert.DeserializeObject<HeroDotaResponse>(response);
            if (Hero != null)
            {
                answerHero = $"Hero Name: {Hero.localized_name}\r\n" +
                         $"Attack type: {Hero.attack_type}\r\n" +
                         $"Healt: {Hero.base_health}\r\n" +
                         $"Mana: {Hero.base_mana}\r\n" +
                         $"Armor: {Hero.base_armor}\r\n" +
                         $"Move speed: {Hero.move_speed}\r\n" +
                         $"Attack damage: {Hero.base_attack_min}-{Hero.base_attack_max}";
            }
            else
            {
                answerHero = "Козаче щось ти ввів не так. Cпробуй /info щоб зрозуміти";
            }
        }

        private static void TournamentList()
        {

            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/Tournaments";
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

            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/TeamsMatchesLive";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            if (response == "[]")
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

            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/TeamsMatchesLive";
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

            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/TeambyName/" + NameTeam;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            TeamDotaResponse TeamDota = JsonConvert.DeserializeObject<TeamDotaResponse>(response);
            if (TeamDota != null)
            {
                answerteam = $"Name: {TeamDota.name}\r\n" +
                         $"Rating: {TeamDota.rating}\r\n" +
                         $"Wins: {TeamDota.wins}\r\n" +
                         $"Losses: {TeamDota.losses}\r\n" +
                         $"";
            }
            else
            {
                answerteam = "Козаче щось ти ввів не так. Cпробуй /info щоб зрозуміти";
            }

        }
        private static void TeamAdd(string NameTeam)
        {

            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/AddTeamName/" + NameTeam;
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

            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/DelTeam/" + NameTeam;
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

            string url = "https://apitestcours20220628192556.azurewebsites.net/Dota/CompareHeroes/" + NameHeroes;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
            string response;
            using (StreamReader Streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = Streamreader.ReadToEnd();
            }
            if (response != "")
            {
                answerOncompare = response;
            }
            else
            {
                answerOncompare = "Козаче щось ти ввів не так. Cпробуй /info щоб зрозуміти";
            }

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
