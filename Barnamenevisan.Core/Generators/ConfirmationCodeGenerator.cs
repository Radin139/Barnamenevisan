namespace Barnamenevisan.Core.Generators;

public class ConfirmationCodeGenerator
{
    public static string GetCode()
    {
        Random random = new Random();
        int code = random.Next(10_000, 99_999);
        return code.ToString();
    }
}