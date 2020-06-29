using Eason.Utility;
using System.Text;

namespace Eason.Application
{
    /// <summary>
    /// 通行证平台密码管理中心规范
    /// </summary>
    public static class Password
    {
        /// <summary>
        /// 根据原通行证规范处理密码信息
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(this string password)
        {

            return CryptoUtils.DesEncrypt(password).ToUpper();
        }
    }
}