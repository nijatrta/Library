using System;

public static class IsbnHelper
{
    public static bool IsValidIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return false;

        isbn = isbn.Replace("-", "").Replace(" ", "");

        if (isbn.Length == 10)
            return IsIsbn10(isbn);

        if (isbn.Length == 13)
            return IsIsbn13(isbn);

        return false;
    }

    private static bool IsIsbn10(string isbn)
    {
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            if (!char.IsDigit(isbn[i]))
                return false;

            sum += (i + 1) * (isbn[i] - '0');
        }

        char lastChar = isbn[9];
        if (lastChar == 'X')
            sum += 10 * 10;
        else if (char.IsDigit(lastChar))
            sum += 10 * (lastChar - '0');
        else
            return false;

        return sum % 11 == 0;
    }

    private static bool IsIsbn13(string isbn)
    {
        int sum = 0;

        for (int i = 0; i < 13; i++)
        {
            if (!char.IsDigit(isbn[i]))
                return false;

            int digit = isbn[i] - '0';
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        return sum % 10 == 0;
    }
}
