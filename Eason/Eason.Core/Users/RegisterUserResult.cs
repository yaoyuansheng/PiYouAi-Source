using System;
using System.ComponentModel;

namespace Eason.Core.Users
{
    public class RegisterUserResult
    {
        public RegisterUserResult()
        {
           
        }
        public RegisterUserResultType status { get; set; }
        public string message { get; set; }
      //  public UserDto user { get; set; }
    }
}
