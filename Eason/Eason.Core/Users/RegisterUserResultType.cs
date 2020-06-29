namespace Eason.Core.Users
{
    public enum RegisterUserResultType
    {
        Success = 1,
        InvalidAccount = 2,
        InvalidPassword = 3,
        ExistAccount = 4,
        ExistEmail = 5,
        ExistTelephone = 6,
        Fail = 7,
        InvalidCode = 8
    }
}
