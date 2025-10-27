//using DocumentFormat.OpenXml.EMMA;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.MyHelper
{
    public class Telegram
    {


        public dynamic SendNotification(Info InfoMsg, string flagTelegram = "0")
        {
            if (flagTelegram == "1")
            {
                try
                {
                    string link = "https://api.telegram.org/bot" + InfoMsg.BootApiKey + "/sendMessage?chat_id=" + InfoMsg.ChatId + "&text=" + InfoMsg.Msg + "&parse_mode=html";

                    var options = new RestClientOptions(link)
                    {
                        ThrowOnAnyError = true,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest(link);
                    var response = client.Get(request);

                    return response;
                }
                catch (Exception e)
                {
                    return e;
                }
            }
            else
            {
                return null;
            }

        }

        public class Info{
            public string? BootApiKey { get; set; } = "6179493068:AAFLdfhSkFCnYgPSuIqiu2XJ_QEO0kjX0Ds";
            public string? BotUsername { get; set; } = "thongbaoportal_bot";
            public string? ChatId { get; set; } = "-1001879634481";
            public string? Msg { get; set; } = "Chưa có nội dung";
        
        
        }

    }
}
