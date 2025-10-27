using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.USUsers;
using API.Areas.Admin.Models.LogsRequest;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Models.MyHelper
{
    public class SmartVoice { 
        public string? text { get; set; }
        public bool text_split { get; set; }=false;
        public string? model { get; set; } = "news";
        public string? region { get; set; } = "female_north";
        public double speed { get; set; } = 1;
        public string? audio_format { get; set; } = "mp3";
        public bool auto_silence { get; set; } = true;
    }

    public class SmartVoiceRes
    {
        public string? message { get; set; }
        public string? hash_text { get; set; }
        public string? length_text { get; set; }
        //public Object "object" { get; set; }

    }
    
    public class SmartVoicePlaylist
    {
        public string? audio_link { get; set; }
        public string? idx { get; set; }
        public string? text { get; set; }
        public string? text_len { get; set; }
        public string? total { get; set; }
        //public Object "object" { get; set; }

    }
     public class SmartVoiceCheckStatus
    {
        public string? text_id { get; set; }       
    }

    public class SmartVoiceService
    {
        public static string DownloadFileByTextIdEN(DMCoQuan ItemCoQuan, Articles Item, string textId)
        {
            string LinkFileSave = "";
            string LinkAPI = "https://api.idg.vnpt.vn/tts-service/v1/check-status";
            string LinkFile = "";

            var options = new RestClientOptions(LinkAPI)
            {
                ThrowOnAnyError = true,
                //Timeout = 360000
            };
            var client = new RestClient(options);
            var request = new RestRequest(LinkAPI);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ItemCoQuan.MetadataCV.SmartVoiceAccessToken);
            request.AddHeader("Token-id", ItemCoQuan.MetadataCV.SmartVoiceTokenID);
            request.AddHeader("Token-key", ItemCoQuan.MetadataCV.SmartVoiceTokenKey);

            API.Models.MyHelper.SmartVoiceCheckStatus ItemSnartVoice = new SmartVoiceCheckStatus()
            {
                text_id = textId,
            };

            request.AddParameter("application/json", ItemSnartVoice, ParameterType.RequestBody);

            try
            {
                var response = client.Post(request);
                if (response.StatusCode.ToString() == "OK")
                {

                    JToken DataSmartVoice = response.Content;

                    SmartVoiceRes SmartVoiceRes = JsonConvert.DeserializeObject<SmartVoiceRes>(response.Content);
                    var NguVoice = JsonConvert.DeserializeObject<JObject>(response.Content);
                    if ((string)NguVoice["message"] == "IDG-00000000")
                    {

                        var playlist = NguVoice["object"]["playlist"].ToList();
                        if (playlist.Count > 0)
                        {
                            LinkFile = (string)playlist[0]["audio_link"];

                            USUsersService.SaveUserLog(new USUsersLog() { Browser = "VI", IdUser = 0, Token = LinkFile, Platform = "Audio_" + Item.Id.ToString() });
                            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/uploads/" + ItemCoQuan.FolderUpload + "/audios/audio-en-" + Item.Id.ToString() + ".mp3"; 

                            LinkFileSave = "/uploads/" + ItemCoQuan.FolderUpload + "/audios/audio-en-" + Item.Id.ToString() + ".mp3";
                            API.Models.MsgSuccess FlagFileDownload = save_file_from_url(dirPath, LinkFile, "");

                            USUsersService.SaveUserLog(new USUsersLog()
                            {
                                Browser = "Local_VI",
                                IdUser = 0,
                                Token = LinkFile,
                                Platform = "Audio_" + Item.Id.ToString(),
                                Description = JsonConvert.SerializeObject(FlagFileDownload)
                            });
                        }
                    }
                    else
                    {
                        LogsRequest ItemLog = new LogsRequest()
                        {
                            Title = "DownloadFileByTextIdEN Status",
                            Introtext = "File audio chưa tạo xong" + Item.Id,
                            Description = response.Content,
                            IdCoQuan = Item.IdCoQuan
                        };
                        LogsRequestService.SaveItem(ItemLog);
                    }
                }
            }
            catch (Exception e)
            {
                LogsRequest ItemLog = new LogsRequest()
                {
                    Title = "DownloadFileByTextIdEN",
                    Introtext = "Lỗi khi tải file IdArticle:" + Item.Id,
                    Description = e.Message,
                    IdCoQuan = Item.IdCoQuan
                };
                LogsRequestService.SaveItem(ItemLog);
            }

            return LinkFileSave;
        }

        public static string DownloadFileByTextId(DMCoQuan ItemCoQuan, Articles Item, string textId)
        {
            string LinkFileSave = "";
            string LinkAPI = "https://api.idg.vnpt.vn/tts-service/v1/check-status";
            string LinkFile = "";
            
            var options = new RestClientOptions(LinkAPI)
            {
                ThrowOnAnyError = true,
                //Timeout = 360000
            };
            var client = new RestClient(options);
            var request = new RestRequest(LinkAPI);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ItemCoQuan.MetadataCV.SmartVoiceAccessToken);
            request.AddHeader("Token-id", ItemCoQuan.MetadataCV.SmartVoiceTokenID);
            request.AddHeader("Token-key", ItemCoQuan.MetadataCV.SmartVoiceTokenKey);

            API.Models.MyHelper.SmartVoiceCheckStatus ItemSnartVoice = new SmartVoiceCheckStatus()
            {
                text_id = textId,
            };

            request.AddParameter("application/json", ItemSnartVoice, ParameterType.RequestBody);

            try
            {
                var response = client.Post(request);
                if (response.StatusCode.ToString() == "OK")
                {

                    JToken DataSmartVoice = response.Content;

                    SmartVoiceRes SmartVoiceRes = JsonConvert.DeserializeObject<SmartVoiceRes>(response.Content);
                    var NguVoice = JsonConvert.DeserializeObject<JObject>(response.Content);
                    if ((string)NguVoice["message"] == "IDG-00000000")
                    {
                       
                        var playlist = NguVoice["object"]["playlist"].ToList();
                        if (playlist.Count > 0)
                        {
                            LinkFile = (string)playlist[0]["audio_link"];

                            USUsersService.SaveUserLog(new USUsersLog() { Browser = "VI", IdUser = 0, Token = LinkFile, Platform = "Audio_" + Item.Id.ToString() });
                            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/uploads/" + ItemCoQuan.FolderUpload + "/audios/audio-" + Item.Id.ToString() + ".mp3"; //G:\vnpt-daklak\portal-v2\API\wwwroot/documents/21554/0/

                            LinkFileSave = "/uploads/" + ItemCoQuan.FolderUpload + "/audios/audio-" + Item.Id.ToString() + ".mp3";
                            API.Models.MsgSuccess FlagFileDownload = save_file_from_url(dirPath, LinkFile, "");

                            USUsersService.SaveUserLog(new USUsersLog()
                            {
                                Browser = "Local_VI",
                                IdUser = 0,
                                Token = LinkFile,
                                Platform = "Audio_" + Item.Id.ToString(),
                                Description = JsonConvert.SerializeObject(FlagFileDownload)
                            });
                        }
                    }
                    else
                    {
                        LogsRequest ItemLog = new LogsRequest()
                        {
                            Title = "DownloadFileByTextId Status",
                            Introtext = "File audio chưa tạo xong" + Item.Id,
                            Description = response.Content,
                            IdCoQuan = Item.IdCoQuan
                        };
                        LogsRequestService.SaveItem(ItemLog);
                    }
                }
            }
            catch (Exception e)
            {
                LogsRequest ItemLog = new LogsRequest()
                {
                    Title = "DownloadFileByTextId",
                    Introtext = "Lỗi khi tạo dữ liệu DownloadFileByTextId IdArticle:" + Item.Id,
                    Description = e.Message,
                    IdCoQuan = Item.IdCoQuan
                };
                LogsRequestService.SaveItem(ItemLog);
            }

            return LinkFileSave;
        }

        public static string GetTextId(DMCoQuan ItemCoQuan, Articles Item)
        {
            string textId = "";
            string LinkAPI = "https://api.idg.vnpt.vn/tts-service/v1/standard";
            if (Item.FullText!=null && Item.FullText != "")
            {
                var options = new RestClientOptions(LinkAPI)
                {
                    ThrowOnAnyError = true,                    
                };
                var client = new RestClient(options);
                var request = new RestRequest(LinkAPI);

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", ItemCoQuan.MetadataCV.SmartVoiceAccessToken);
                request.AddHeader("Token-id", ItemCoQuan.MetadataCV.SmartVoiceTokenID);
                request.AddHeader("Token-key", ItemCoQuan.MetadataCV.SmartVoiceTokenKey);

                API.Models.MyHelper.SmartVoice ItemSnartVoice = new SmartVoice()
                {
                    text = Item.FullText,
                    region = ItemCoQuan.MetadataCV.SmartVoiceRegion,
                    speed = int.Parse(ItemCoQuan.MetadataCV.SmartVoiceSpeed.ToString()),
                };

                request.AddParameter("application/json", ItemSnartVoice, ParameterType.RequestBody);

                try
                {
                    var response = client.Post(request);
                    if (response.StatusCode.ToString() == "OK")
                    {

                        JToken DataSmartVoice = response.Content;

                        SmartVoiceRes SmartVoiceRes = JsonConvert.DeserializeObject<SmartVoiceRes>(response.Content);
                        var NguVoice = JsonConvert.DeserializeObject<JObject>(response.Content);
                        if ((string)NguVoice["message"] == "IDG-00000000")
                        {

                            if (NguVoice["object"]["code"].ToString() == "success")
                            {
                                textId = NguVoice["object"]["text_id"].ToString();
                                USUsersService.SaveUserLog(new USUsersLog()
                                {
                                    Browser = "Local_VI",
                                    IdUser = 0,
                                    Token = textId,
                                    Platform = "Audio_" + Item.Id.ToString(),
                                    Description = ""
                                });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogsRequest ItemLog = new LogsRequest()
                    {
                        Title = "GetTextId",
                        Introtext = "Lỗi khi tạo dữ liệu GetTextId IdArticle:" + Item.Id,
                        Description = e.Message,
                        IdCoQuan = Item.IdCoQuan
                    };
                    LogsRequestService.SaveItem(ItemLog);
                }
            }
            
            return textId;
        }


        public static string GetTextIdEN(DMCoQuan ItemCoQuan, Articles Item)
        {
            string textId = "";
            if (Item.FullText_EN!=null && Item.FullText_EN.Trim() != "")
            {                
                string LinkAPI = "https://api.idg.vnpt.vn/tts-service/v1/standard";
                var options = new RestClientOptions(LinkAPI)
                {
                    ThrowOnAnyError = true,                    
                };
                var client = new RestClient(options);
                var request = new RestRequest(LinkAPI);

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", ItemCoQuan.MetadataCV.SmartVoiceAccessToken);
                request.AddHeader("Token-id", ItemCoQuan.MetadataCV.SmartVoiceTokenID);
                request.AddHeader("Token-key", ItemCoQuan.MetadataCV.SmartVoiceTokenKey);

                API.Models.MyHelper.SmartVoice ItemSnartVoice = new SmartVoice()
                {
                    text = Item.FullText_EN,
                    region = ItemCoQuan.MetadataCV.SmartVoiceRegion,
                    speed = int.Parse(ItemCoQuan.MetadataCV.SmartVoiceSpeed.ToString()),
                };

                request.AddParameter("application/json", ItemSnartVoice, ParameterType.RequestBody);

                try
                {
                    var response = client.Post(request);
                    if (response.StatusCode.ToString() == "OK")
                    {

                        JToken DataSmartVoice = response.Content;

                        SmartVoiceRes SmartVoiceRes = JsonConvert.DeserializeObject<SmartVoiceRes>(response.Content);
                        var NguVoice = JsonConvert.DeserializeObject<JObject>(response.Content);
                        if ((string)NguVoice["message"] == "IDG-00000000")
                        {

                            if (NguVoice["object"]["code"].ToString() == "success")
                            {
                                textId = NguVoice["object"]["text_id"].ToString();
                                USUsersService.SaveUserLog(new USUsersLog()
                                {
                                    Browser = "Local_VI",
                                    IdUser = 0,
                                    Token = textId,
                                    Platform = "Audio_" + Item.Id.ToString(),
                                    Description = ""
                                });
                            }
                        }
                        else
                        {
                            LogsRequest ItemLog = new LogsRequest()
                            {
                                Title = "GetTextIdEN",
                                Introtext = "Lỗi khi tạo dữ liệu text_id EN IdArticle:" + Item.Id,
                                Description = response.Content,
                                IdCoQuan = Item.IdCoQuan
                            };
                            LogsRequestService.SaveItem(ItemLog);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogsRequest ItemLog = new LogsRequest()
                    {
                        Title = "GetTextIdEN",
                        Introtext = "Lỗi khi tạo dữ liệu EN IdArticle:" + Item.Id,
                        Description = e.Message,
                        IdCoQuan = Item.IdCoQuan
                    };
                    LogsRequestService.SaveItem(ItemLog);
                }

            }

            return textId;
        }
        public static string GetFileVoice(DMCoQuan ItemCoQuan, Articles Item)
        {
            

            string filePathAudio = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/uploads/" + ItemCoQuan.FolderUpload + "/audios/";

            if (!Directory.Exists(filePathAudio))
            {
                Directory.CreateDirectory(filePathAudio);                
            }
            

            string LinkFile = "";
            string LinkFileSave = "";
            string LinkAPI = "https://api.idg.vnpt.vn/tts-service/v2/standard";

            var options = new RestClientOptions(LinkAPI)
            {
                ThrowOnAnyError = true,
                //Timeout = 360000
            };
            var client = new RestClient(options);
            var request = new RestRequest(LinkAPI);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ItemCoQuan.MetadataCV.SmartVoiceAccessToken);
            request.AddHeader("Token-id", ItemCoQuan.MetadataCV.SmartVoiceTokenID);
            request.AddHeader("Token-key", ItemCoQuan.MetadataCV.SmartVoiceTokenKey);

            API.Models.MyHelper.SmartVoice ItemSnartVoice = new SmartVoice()
            {
                text = Item.FullText,
                region = ItemCoQuan.MetadataCV.SmartVoiceRegion,
                speed = int.Parse(ItemCoQuan.MetadataCV.SmartVoiceSpeed.ToString()),
            };

            request.AddParameter("application/json", ItemSnartVoice, ParameterType.RequestBody);
            
            try
            {
                var response = client.Post(request);
                if (response.StatusCode.ToString() == "OK")
                {

                    JToken DataSmartVoice = response.Content;

                    SmartVoiceRes SmartVoiceRes = JsonConvert.DeserializeObject<SmartVoiceRes>(response.Content);
                    var NguVoice = JsonConvert.DeserializeObject<JObject>(response.Content);
                    if ((string)NguVoice["message"] == "IDG-00000000")
                    {
                        //                    List<SmartVoicePlaylist> playlist =(List<SmartVoicePlaylist>)jObject(NguVoice["object"]["playlist"]);
                        //List<SmartVoicePlaylist> playlist = JsonConvert.DeserializeObject<List<SmartVoicePlaylist>>((string)NguVoice["object"]["playlist"]);

                        var playlist = NguVoice["object"]["playlist"].ToList();
                        if (playlist.Count > 0)
                        {
                            LinkFile = (string)playlist[0]["audio_link"];

                            USUsersService.SaveUserLog(new USUsersLog() { Browser = "VI", IdUser = 0, Token = LinkFile, Platform = "Audio_" + Item.Id.ToString() });
                            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/uploads/" + ItemCoQuan.FolderUpload + "/audios/audio-" + Item.Id.ToString() + ".mp3"; //G:\vnpt-daklak\portal-v2\API\wwwroot/documents/21554/0/

                            LinkFileSave = "/uploads/" + ItemCoQuan.FolderUpload + "/audios/audio-" + Item.Id.ToString() + ".mp3";
                            API.Models.MsgSuccess FlagFileDownload = save_file_from_url(dirPath, LinkFile, "");

                            USUsersService.SaveUserLog(new USUsersLog()
                            {
                                Browser = "Local_VI",
                                IdUser = 0,
                                Token = LinkFile,
                                Platform = "Audio_" + Item.Id.ToString(),
                                Description = JsonConvert.SerializeObject(FlagFileDownload)
                            });
                        }

                        //var a =  JsonConvert.DeserializeObject(NguVoice["object"]["playlist"]);
                    }
                }
            }
            catch (Exception e)
            {
                LogsRequest ItemLog = new LogsRequest()
                {
                    Title = "SmartVoice",
                    Introtext = "Lỗi khi tạo dữ liệu IdArticle:"+Item.Id,
                    Description = e.Message,
                    IdCoQuan = Item.IdCoQuan
                };
                LogsRequestService.SaveItem(ItemLog);
            }
            

            return LinkFileSave;
            
        }



        public static string GetFileVoiceEN(DMCoQuan ItemCoQuan, Articles Item)
        {


            string filePathAudio = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/uploads/" + ItemCoQuan.FolderUpload + "/audios/";

            if (!Directory.Exists(filePathAudio))
            {
                Directory.CreateDirectory(filePathAudio);
            }


            string LinkFileSave = "";
            string LinkFile = "";
            string LinkAPI = "https://api.idg.vnpt.vn/tts-service/v2/standard";

            var options = new RestClientOptions(LinkAPI)
            {
                ThrowOnAnyError = true,
                //Timeout = 1000
            };
            var client = new RestClient(options);
            var request = new RestRequest(LinkAPI);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ItemCoQuan.MetadataCV.SmartVoiceAccessToken);
            request.AddHeader("Token-id", ItemCoQuan.MetadataCV.SmartVoiceTokenID);
            request.AddHeader("Token-key", ItemCoQuan.MetadataCV.SmartVoiceTokenKey);

            if(Item.FullText_EN!=null && Item.FullText_EN != "")
            {
                try
                {
                    API.Models.MyHelper.SmartVoice ItemSnartVoice = new SmartVoice()
                    {
                        text = Item.FullText_EN,

                    };

                    request.AddParameter("application/json", ItemSnartVoice, ParameterType.RequestBody);

                    var response = client.Post(request);

                    if (response.StatusCode.ToString() == "OK")
                    {

                        JToken DataSmartVoice = response.Content;

                        SmartVoiceRes SmartVoiceRes = JsonConvert.DeserializeObject<SmartVoiceRes>(response.Content);
                        var NguVoice = JsonConvert.DeserializeObject<JObject>(response.Content);
                        if ((string)NguVoice["message"] == "IDG-00000000")
                        {
                            //                    List<SmartVoicePlaylist> playlist =(List<SmartVoicePlaylist>)jObject(NguVoice["object"]["playlist"]);
                            //List<SmartVoicePlaylist> playlist = JsonConvert.DeserializeObject<List<SmartVoicePlaylist>>((string)NguVoice["object"]["playlist"]);

                            var playlist = NguVoice["object"]["playlist"].ToList();
                            if (playlist.Count > 0)
                            {
                                LinkFile = (string)playlist[0]["audio_link"];
                                USUsersService.SaveUserLog(new USUsersLog() { Browser = "EN", IdUser = 0, Token = LinkFile, Platform = "Audio_" + Item.Id.ToString() });

                                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/uploads/" + ItemCoQuan.FolderUpload + "/audios/audio-en-" + Item.Id.ToString() + ".mp3"; //G:\vnpt-daklak\portal-v2\API\wwwroot/documents/21554/0/
                                LinkFileSave = "/uploads/" + ItemCoQuan.FolderUpload + "/audios/audio-en-" + Item.Id.ToString() + ".mp3";
                                API.Models.MsgSuccess FlagFileDownload = save_file_from_url(dirPath, LinkFile, "");

                                USUsersService.SaveUserLog(new USUsersLog()
                                {
                                    Browser = "Local_EN",
                                    IdUser = 0,
                                    Token = LinkFile,
                                    Platform = "Audio_" + Item.Id.ToString(),
                                    Description = JsonConvert.SerializeObject(FlagFileDownload)
                                });
                            }

                            //var a =  JsonConvert.DeserializeObject(NguVoice["object"]["playlist"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogsRequest ItemLog = new LogsRequest()
                    {
                        Title = "SmartVoice",
                        Introtext = "Lỗi khi tạo dữ liệu IdArticle:" + Item.Id,
                        Description = e.Message,
                        IdCoQuan = Item.IdCoQuan
                    };
                    LogsRequestService.SaveItem(ItemLog);
                }
                

            }
            return LinkFileSave;

        }

        public static API.Models.MsgSuccess save_file_from_url(string file_name, string url,string UserAgent)
        {
            if (UserAgent == null || UserAgent == "")
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";
            }

            byte[] content;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("User-Agent", UserAgent);
                //request.Headers.Add("Cookie", Cookie);

                WebResponse response = request.GetResponse();

                Stream stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    content = br.ReadBytes(70000000);
                    br.Close();
                }
                response.Close();

                FileStream fs = new FileStream(file_name, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    bw.Write(content);
                }
                finally
                {
                    fs.Close();
                    bw.Close();
                }
                return new API.Models.MsgSuccess() { };
            }
            catch (Exception e)
            {
                return new API.Models.MsgSuccess() { Success = false, Msg = e.ToString() };
            }


        }
    }

}
