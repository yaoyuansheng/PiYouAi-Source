using Aliyun.MNS;
using Aliyun.MNS.Model;
using Eason.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.Application.Implement
{
    public class MessageService: IMessageService
    {
        private const string _accessKeyId = "LTAI0jsL84N1Sdx4";
        private const string _secretAccessKey = "7yEUrYlHAfzFU1dBE0ec5vWYLuO507";
        private const string _reginEndPoint = "http://1126059839449720.mns.cn-hangzhou.aliyuncs.com/";
        private const string _topicName = "piyouai";
        private const string _freeSignName = "匹优爱";

        public void Register(string code, string telephone)
        {
            IMNS client = new Aliyun.MNS.MNSClient(_accessKeyId, _secretAccessKey, _reginEndPoint);
            Topic topic = client.GetNativeTopic(_topicName);
            MessageAttributes messageAttributes = new MessageAttributes();
            BatchSmsAttributes batchSmsAttributes = new BatchSmsAttributes();
            // 3.1 设置发送短信的签名：SMSSignName
            batchSmsAttributes.FreeSignName = _freeSignName;
            // 3.2 设置发送短信的模板SMSTemplateCode
            batchSmsAttributes.TemplateCode = "SMS_62610202";
            Dictionary<string, string> param = new Dictionary<string, string>();
            // 3.3 （如果短信模板中定义了参数）设置短信模板中的参数，发送短信时，会进行替换
            param.Add("code", code);
            param.Add("product", _freeSignName);

            // 3.4 设置短信接收者手机号码
            batchSmsAttributes.AddReceiver(telephone, param);

            messageAttributes.BatchSmsAttributes = batchSmsAttributes;
            PublishMessageRequest request = new PublishMessageRequest();
            request.MessageAttributes = messageAttributes;
            /**
             * Step 4. 设置SMS消息体（必须）
             *
             * 注：目前暂时不支持消息内容为空，需要指定消息内容，不为空即可。
             */
            request.MessageBody = "smsmessage";
            try
            {
                PublishMessageResponse resp = topic.PublishMessage(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Publish SMS message failed, exception info: " + ex.Message);
            }
        }
    }
}
