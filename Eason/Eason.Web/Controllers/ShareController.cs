using Eason.Mvc;
using Eason.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Eason.Web.Controllers
{
    public class ShareController : Controller
    {
        public static AccessToken token { get; set; }

        public static jsapi_ticket ticket { get; set; }
        public static string appid { get; set; } = "wx717a73d4a56df983";
        public static string secret { get; set; } = "b0ad8b621f26345c1bc330a170bd49aa";

        public async System.Threading.Tasks.Task<AccessToken> Index()
        {
            if (token != null && token.expires > DateTime.Now)
            {
                return token;
            }
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            var result = await Eason.Utility.HttpUtils.HttpClientAsync(url, Utility.HttpMethod.GET, null);
            if (result.Contains("access_token"))
            {
                token = JsonConvert.DeserializeObject<AccessToken>(result);
                return token;
            }
            return null;
        }
        public async System.Threading.Tasks.Task<jsapi_ticket> Js()
        {
            if (ticket != null && ticket.expires > DateTime.Now)
            {
                return ticket;
            }

            var token = await Index();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token.access_token);
            var result = await Eason.Utility.HttpUtils.HttpClientAsync(url, Utility.HttpMethod.GET, null);
            ticket = JsonConvert.DeserializeObject<jsapi_ticket>(result);
            if (ticket != null && ticket.errcode == 0)
            {
                return ticket;
            }
            return null;
        }
        public static string markSignature(Dictionary<string, string> dic, string key, string method)
        {
            var result = dic.OrderBy(i => i.Key);
            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append(item.Key + "=" + item.Value + "&");
            }
            string compareresult = string.Empty;
            if (sb.Length > 0)
            {
                compareresult = sb.ToString().Substring(0, sb.ToString().Length - 1);
            }
            string tempStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(compareresult + key, method);
            return tempStr;
        }
        public async System.Threading.Tasks.Task<string> Config(string url)
        {
            var jsapi_t = await Js();
            long timestamp = Eason.Utility.DateUtils.GetTimeStamp();
            string nonceStr = Guid.NewGuid().ToString("N").Substring(0, 10);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("url", url);
            dic.Add("jsapi_ticket", jsapi_t.ticket);
            dic.Add("timestamp", timestamp.ToString());
            dic.Add("noncestr", nonceStr);
            string sign = markSignature(dic, "", "sha1");
            return appid + "," + timestamp + "," + url + "," + sign + "," + nonceStr;
        }

    }
}