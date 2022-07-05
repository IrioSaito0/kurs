using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Telegram.Bot.Types.InlineQueryResults;
using System.Linq;

//Ku-86_IbKCs2jtwmsxviOqeXE4_xQuq_WPhhtzHZ
//Console.WriteLine("Buncket: {0}", string.Join(" ", x));

namespace Botcode
{
    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("5578999856:AAHZW59WoqnyiM-sYjKf5W_RycMeUV_dmx0");
        static bool key = false;
        static int n;
        static string[] Positions = new string[] { "free", "categories", "locations", "rank", "date","Name","Category","Location","Labels","Status","Begin","End","Delete","Change1","Change2" };
        static byte CurrentPosition = 0;
        static List<string?> Get = new List<string?> { };
        static List<string?> Labels = new List<string?> { };
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {

                Message? message = update.Message;
                ReplyKeyboardMarkup Standart = new(Array.Empty<KeyboardButton>()) { ResizeKeyboard = false };
                if (message?.Text == null) return;
                if (message.Text == "/stop")
                {
                    await botClient.SendStickerAsync(message.Chat, "CAACAgIAAxkBAALd52K83bMZIP9HnziVFhfBGui6aI1iAAL4BgAC3XI5SJsAAW9tMPp38CkE");
                    await botClient.SendTextMessageAsync(message.Chat, "I stoped current operation");
                    CurrentPosition = 0;
                    return;
                }
                switch (Positions[CurrentPosition])
                {
                    case "free":
                        switch (message.Text.ToLower())
                        {
                        case "/start":
                            ReplyKeyboardMarkup KeyboardMarkup = new(new[] { new KeyboardButton[] { "/foundpublicevent", "/foundprivatevent" }, new KeyboardButton[] { "/createprivate", "/deleteprivate" }, }) { ResizeKeyboard = true };
                            await botClient.SendStickerAsync(message.Chat, "CAACAgIAAxkBAALdeWK7_7QS0lLTuNb2yd6ViO767P1OAAIZBgAC1OtRS17iSayPyjYXKQQ");
                            await botClient.SendTextMessageAsync(message.Chat, "Welcome! I can help You to found some public events to hang out or save data about Your events. Just choose a function below to begin", replyMarkup: KeyboardMarkup);
                            break;
                        case "/foundpublicevent":
                            ReplyKeyboardMarkup KeyboardMarkup2 = new(new[]{new KeyboardButton[] { "school-holidays", "public-holidays", "observances", "politics", "conferences", "expos", "concerts" }, new KeyboardButton[] { "festivals", "performing-arts", "community", "sports", "daylight-savings", "academic", "any" }, }) { ResizeKeyboard = true };
                            await botClient.SendStickerAsync(message.Chat, "CAACAgIAAxkBAALdnmK8Hh9KGO7ohY_iLlhP-tHd_4fcAAMGAAKIQHBL-IfpH39R7NwpBA");
                            await botClient.SendTextMessageAsync(message.Chat, "Well-well-well, searching events to hang out? just answer a few questions so I can find a suitable event for you");
                            await botClient.SendTextMessageAsync(message.Chat, "Which Categories You want to choose?", replyMarkup: KeyboardMarkup2);
                            CurrentPosition++;
                            break;
                        case "/foundprivatevent":
                                using (var httpClient = new HttpClient())
                                {
                                    string usr = message.From.Username;
                                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://localhost:7131/event/?Own=" + usr))
                                    {
                                        key = true;
                                        request.Headers.Add("Accept", "application/json");
                                        var response = await httpClient.SendAsync(request);
                                        await botClient.SendStickerAsync(message.Chat, "CAACAgIAAxkBAALd2WK8mvtKUAvJUEZFJTlw5MBX_5UTAALeCAACetZJS1a7oNDi48ZKKQQ");
                                        await botClient.SendTextMessageAsync(message.Chat, "Here is your events:");
                                            var json = await response.Content.ReadAsStringAsync();
                                            Events res = JsonConvert.DeserializeObject<Events>(json);
                                            string t = "\n\nName:" + res.Name + "\nCategory:" + res.Category + "\nLabels:" + string.Join(" ", res.Labels) + "\nLocation:" + res.Location + "\nStatus:" + res.Status + "\nStart:" + Convert.ToString(res.Begin) + "\nEnd:" + Convert.ToString(res.End);
                                            ReplyKeyboardMarkup KeyboardMarkup1 = new(new[] { new KeyboardButton[] { "/start" }, }) { ResizeKeyboard = true };
                                            await botClient.SendTextMessageAsync(message.Chat, t, replyMarkup: KeyboardMarkup1);
                                    } 
                                }
                                break;
                        case "/createprivate":
                                string usr1 = message.From.Username;
                                await botClient.SendStickerAsync(message.Chat, "CAACAgIAAxkBAALd2WK8mvtKUAvJUEZFJTlw5MBX_5UTAALeCAACetZJS1a7oNDi48ZKKQQ");
                                await botClient.SendTextMessageAsync(message.Chat, "Print Name of event:");
                                CurrentPosition = 5;
                                Get.Add(usr1);
                                break;
                            case "/deleteprivate":
                                await botClient.SendStickerAsync(message.Chat, "CAACAgIAAxkBAALd2WK8mvtKUAvJUEZFJTlw5MBX_5UTAALeCAACetZJS1a7oNDi48ZKKQQ");
                                await botClient.SendTextMessageAsync(message.Chat, "Print Name of event to delete:");
                                CurrentPosition = 12;
                                break;
                            //case "/endprivate":
                            //    await botClient.SendStickerAsync(message.Chat, "CAACAgIAAxkBAALd2WK8mvtKUAvJUEZFJTlw5MBX_5UTAALeCAACetZJS1a7oNDi48ZKKQQ", replyMarkup: KeyboardMarkup3);
                            //    await botClient.SendTextMessageAsync(message.Chat, "Here is your events:");
                                CurrentPosition = 12;
                                break;
                            default:
                            await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
                            break;
                        }
                        break;
                    case "categories":
                        Get.Add(message.Text);
                        if (Get[0] == "any") Get[0] = ""; else if (key == true) Get[0] = "&category=" + Get[0]; else { Get[0] = "category=" + Get[0]; key = true; };
                         CurrentPosition++;
                        ReplyKeyboardMarkup KeyboardMarkup00 = new(new[] { new KeyboardButton[] { "any" }, }) { ResizeKeyboard = true };
                        await botClient.SendTextMessageAsync(message.Chat, "Print a tag of country You want to choose.", replyMarkup: KeyboardMarkup00);
                        break;
                    case "locations":
                        Get.Add(message.Text);
                        if (Get[1] == "any") Get[1] = ""; else if (key == true) Get[1] = "&country=" + Get[1]; else { Get[1] = "?country=" + Get[1]; key = true; };
                        CurrentPosition++;
                        ReplyKeyboardMarkup KeyboardMarkup01 = new(new[] { new KeyboardButton[] { "1", "2","3","4","5", "any" }, }) { ResizeKeyboard = true };
                        await botClient.SendTextMessageAsync(message.Chat, "Which rank of event You want to choose?", replyMarkup: KeyboardMarkup01);
                        break;
                    case "rank":
                        Get.Add(message.Text);
                        if (Get[2] == "any") Get[2] = ""; else if (key == true) Get[2] = "&rank_level=" + Get[2]; else { Get[2] = "?rank_level=" + Get[2]; key = true; };
                        CurrentPosition++;
                        ReplyKeyboardMarkup KeyboardMarkup04 = new(new[] { new KeyboardButton[] { "any" }, }) { ResizeKeyboard = true };
                        await botClient.SendTextMessageAsync(message.Chat, "Which Data(yyyy-mm-dd) You want to choose?", replyMarkup: KeyboardMarkup04);
                        break;
                    case "date":
                        Get.Add(message.Text);
                        if (Get[3] == "any") Get[3] = ""; else if (key == true) Get[3] = "&category=" + Get[3]; else { Get[3] = "?category=" + Get[3]; key = true; };
                        CurrentPosition =0;

                        using (var httpClient = new HttpClient())
                        {
                            using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.predicthq.com/v1/events/?" + Get[0] + Get[1] + Get[2] + Get[3]))
                            {
                                key = true;
                                request.Headers.TryAddWithoutValidation("Authorization", "Bearer qVFE-hXsX00o6gBgf8xbKhpLvXIfKxyGmxDzjnq4");
                                request.Headers.Add("Accept", "application/json");
                                var response = await httpClient.SendAsync(request);
                                if (response.IsSuccessStatusCode)
                                {
                                    publicAPIData res = new publicAPIData();
                                    var json = await response.Content.ReadAsStringAsync();
                                    res = JsonConvert.DeserializeObject<publicAPIData>(json);
                                    string t = "";
                                    for (int n = 0; n < 10; n++)
                                    {
                                        t = t + "\n\nName:" + res.results[n].title + "\nDescription:" + res.results[n].description + "\nCategory:" + res.results[n].category + "\nLabels:" + string.Join(" ", res.results[n].labels) + "\nRank:" + res.results[n].rank + "\nStart:" + Convert.ToString(res.results[n].start) + "\nEnd:" + Convert.ToString(res.results[n].end) + "\nCountry:" + res.results[n].country;
                                    }
                                    ReplyKeyboardMarkup KeyboardMarkup = new(new[] { new KeyboardButton[] { "/start" }, }) { ResizeKeyboard = true };
                                    await botClient.SendTextMessageAsync(message.Chat, t, replyMarkup: KeyboardMarkup) ;
                                }
                            }
                        }

                        Get.Clear();

                        break;
                    case "Name":
                        Get.Add(message.Text);
                        CurrentPosition++;
                        await botClient.SendTextMessageAsync(message.Chat, "Print a Category.");
                        break;

                    case "Category":
                        Get.Add(message.Text);
                        CurrentPosition++;
                        await botClient.SendTextMessageAsync(message.Chat, "Print a Location.");
                        break;
                    case "Location":
                        Get.Add(message.Text);
                        CurrentPosition++;
                        await botClient.SendTextMessageAsync(message.Chat, "Print a labels. Print /end to finish labels");
                        break;
                    case "Labels":
                        Labels.Add(message.Text+",");
                        if (Labels[Labels.Count - 1] == "/end")
                        {
                            Labels.Add("event");
                            CurrentPosition++;
                            await botClient.SendTextMessageAsync(message.Chat, "Print a status of event.");
                        }
                        break;
                    case "Status":
                        Get.Add(message.Text);
                        CurrentPosition++;
                        await botClient.SendTextMessageAsync(message.Chat, "Print a start time of event.");
                        break;
                    case "Begin":
                        Get.Add(message.Text);
                        CurrentPosition++;
                        await botClient.SendTextMessageAsync(message.Chat, "Print a end time of event.");
                        break;
                    case "End":
                        Get.Add(message.Text);
                        CurrentPosition++;
                        using (var httpClient = new HttpClient())
                        {
                            string usr = message.From.Username;
                            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:7131/event"))
                            {
                                key = true;
                                request.Headers.Add("Accept", "application/json");
                                var t =(@"{""Owner:""{0}""Name:""{1}""Category:"" {2}""Labels:""{3} ""Location:"" {4} ""Status:"" {5} ""Start:"" {6} ""End:"" {7}",usr,Get[0], Get[0],string.Join(" ", Labels),Get[0],Get[0],Get[0],Get[0]);
                                var response = await httpClient.SendAsync(request);
                                ReplyKeyboardMarkup KeyboardMarkup1 = new(new[] { new KeyboardButton[] { "/start" }, }) { ResizeKeyboard = true };

                                await botClient.SendTextMessageAsync(message.Chat, "Deleted", replyMarkup: KeyboardMarkup1);
                            }
                        }
                        await botClient.SendTextMessageAsync(message.Chat, "Event saved.");
                        CurrentPosition = 0;
                        Get.Clear();
                        break;
                    case "Delete":

                            using (var httpClientd = new HttpClient())
                            {
                                string usr = message.From.Username;
                                using (var requestd = new HttpRequestMessage(new HttpMethod("DELETE"), "https://localhost:7131/event"))
                                {
                                    key = true;
                                    requestd.Headers.Add("Accept", "application/json");
                                    Events resd;
                                    using (var httpClient = new HttpClient())
                                    {
                                        using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://localhost:7131/event/?Own=" + usr))
                                        {
                                            key = true;
                                            request.Headers.Add("Accept", "application/json");
                                            var response = await httpClient.SendAsync(request);
                                            var json = await response.Content.ReadAsStringAsync();
                                            resd = JsonConvert.DeserializeObject<Events>(json);
                                        }
                                    }
                                    ReplyKeyboardMarkup KeyboardMarkup1 = new(new[] { new KeyboardButton[] { "/start" }, }) { ResizeKeyboard = true };
                                var data = resd;
                                    var responsed = await httpClientd.SendAsync(requestd);
                                    
                                }
                            }
                        
                        await botClient.SendTextMessageAsync(message.Chat, "Event deleted.");
                        CurrentPosition = 0;
                        break;
                    //case "Change1":
                    //    Get.Add(message.Text);
                    //    if (Get[0] == "any") Get[0] = ""; else if (key == true) Get[0] = "&category=" + Get[0]; else { Get[0] = "category=" + Get[0]; key = true; };
                    //    CurrentPosition++;
                    //    await botClient.SendTextMessageAsync(message.Chat, "Print a tag of country You want to choose.");
                    //    break;
                    //case "Change2":
                    //    Get.Add(message.Text);
                    //    if (Get[0] == "any") Get[0] = ""; else if (key == true) Get[0] = "&category=" + Get[0]; else { Get[0] = "category=" + Get[0]; key = true; };
                    //    CurrentPosition++;
                    //    await botClient.SendTextMessageAsync(message.Chat, "Print a tag of country You want to choose.");
                    //    break;
                }

                
                
            }
        }
    static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }

}