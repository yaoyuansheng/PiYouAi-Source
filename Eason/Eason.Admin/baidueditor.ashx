<%@ WebHandler Language="C#" Class="UEditorHandler" %>
using System;
using System.Web;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using Eason.Admin.EditHandler;
public class UEditorHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        Handler action = null;
        switch (context.Request["action"])
        {
            case "config":
                action = new ConfigHandler(context);
                break;
            case "uploadimage":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = UeditConfig.GetStringList("imageAllowFiles"),
                    PathFormat = UeditConfig.GetString("imagePathFormat"),
                    SizeLimit = UeditConfig.GetInt("imageMaxSize"),
                    UploadFieldName = UeditConfig.GetString("imageFieldName")
                });
                break;
            case "uploadscrawl":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = new string[] { ".png" },
                    PathFormat = UeditConfig.GetString("scrawlPathFormat"),
                    SizeLimit = UeditConfig.GetInt("scrawlMaxSize"),
                    UploadFieldName = UeditConfig.GetString("scrawlFieldName"),
                    Base64 = true,
                    Base64Filename = "scrawl.png"
                });
                break;
            case "uploadvideo":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = UeditConfig.GetStringList("videoAllowFiles"),
                    PathFormat = UeditConfig.GetString("videoPathFormat"),
                    SizeLimit = UeditConfig.GetInt("videoMaxSize"),
                    UploadFieldName = UeditConfig.GetString("videoFieldName")
                });
                break;
            case "uploadfile":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = UeditConfig.GetStringList("fileAllowFiles"),
                    PathFormat = UeditConfig.GetString("filePathFormat"),
                    SizeLimit = UeditConfig.GetInt("fileMaxSize"),
                    UploadFieldName = UeditConfig.GetString("fileFieldName")
                });
                break;
            case "listimage":
                action = new ListFileManager(context, UeditConfig.GetString("imageManagerListPath"), UeditConfig.GetStringList("imageManagerAllowFiles"));
                break;
            case "listfile":
                action = new ListFileManager(context, UeditConfig.GetString("fileManagerListPath"), UeditConfig.GetStringList("fileManagerAllowFiles"));
                break;
            case "catchimage":
                action = new CrawlerHandler(context);
                break;
            default:
                action = new NotSupportedHandler(context);
                break;
        }
        action.Process();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}