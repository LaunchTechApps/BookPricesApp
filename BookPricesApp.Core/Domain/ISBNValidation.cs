using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPricesApp.Core.Domain;
public class ISBNValidation
{
    public static bool IsValidISBN(string isbn)
    {
        isbn = isbn.Replace("-", "").Replace(" ", "");

        if (isbn.Length == 10)
        {
            return IsValidISBN10(isbn);
        }
        else if (isbn.Length == 13)
        {
            return IsValidISBN13(isbn);
        }

        return false;
    }

    private static bool IsValidISBN10(string isbn)
    {
        int checksum = 0;
        for (int i = 0; i < 9; i++)
        {
            if (!char.IsDigit(isbn[i]))
            {
                return false;
            }

            checksum += (i + 1) * int.Parse(isbn[i].ToString());
        }

        if (isbn[9] == 'X')
        {
            checksum += 10 * 10;
        }
        else if (!char.IsDigit(isbn[9]))
        {
            return false;
        }
        else
        {
            checksum += 10 * int.Parse(isbn[9].ToString());
        }

        return checksum % 11 == 0;
    }

    private static bool IsValidISBN13(string isbn)
    {
        int checksum = 0;
        for (int i = 0; i < 12; i++)
        {
            if (!char.IsDigit(isbn[i]))
            {
                return false;
            }

            checksum += i % 2 == 0 ? int.Parse(isbn[i].ToString()) : 3 * int.Parse(isbn[i].ToString());
        }

        if (!char.IsDigit(isbn[12]))
        {
            return false;
        }

        checksum += int.Parse(isbn[12].ToString());

        return checksum % 10 == 0;
    }
}
