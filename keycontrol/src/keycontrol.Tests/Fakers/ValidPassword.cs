using System.Text;

namespace keycontrol.Tests.Fakers;

public static class ValidPassword
{
    public static string Generate()
    {
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "0123456789";
        const string especials = "-_@#$%";
        var random = new Random();
        var password = new StringBuilder();
        password.Append(upperCase[random.Next(upperCase.Length)]);
        password.Append(lowerCase[random.Next(lowerCase.Length)]);
        password.Append(numbers[random.Next(numbers.Length)]);
        password.Append(especials[random.Next(especials.Length)]);

        var allCaracteres = lowerCase + upperCase + numbers + especials;

        for (var i = password.Length; i <= 8; i++)
        {
            password.Append(allCaracteres[random.Next(allCaracteres.Length)]);
        }

        var senhaEmbaralhada = password.ToString().OrderBy(c => random.Next()).ToArray();

        return new string(senhaEmbaralhada);
    }
}