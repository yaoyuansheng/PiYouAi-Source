namespace Eason.Core.Users
{
    public enum LoginResultType
    {
        Success = 1,
        InvalidAccount = 2,
        InvalidPassword = 3,
        UserIsNotActive = 4,
        LoginOut = 5,
        LockedOut = 6,
        WechatLoginFail = 7
    }
}
