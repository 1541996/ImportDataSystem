using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Extensions
{
    public static class MyExtension
    {
        public static string GetUniqueCode()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToUInt32(buffer, 12).ToString();
        }

        public static DateTime GetLocalTime(this DateTime utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time"));
        }

        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static List<string> SplitString(this string text, char separator)
        {
            List<string> trimmedSplits = new();

            var splits = text.Split(separator);

            foreach (var split in splits)
                trimmedSplits.Add(split.Trim());

            return trimmedSplits;

        }

        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //this function Convert to Decord your Password
        public static string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new(decoded_char);
            return result;
        }


        public static bool CheckTextGreaterThanLength(int len, string data)
        {
            if(data.Length > len)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckAmountIsDecimal(string amount)
        {
            decimal number;
            if (!Decimal.TryParse(amount, out number))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static bool CheckISOFormat(string codes)
        {
             var data = Regex.IsMatch(codes, @"^[A-Z]{3}$");
             return data;
        }

        public static bool CheckDateFormatValid(string date)
        {
            DateTime d;

            bool chValidity = DateTime.TryParseExact(
            date,
            "dd/MM/yyyy hh:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out d);

            return chValidity;

        }


        public static bool CheckXMLDateFormatValid(string date)
        {
            var data = Regex.IsMatch(date, @"^([0-9]{4})-([0-1][0-9])-([0-3][0-9])\s([0-1][0-9]|[2][0-3]):([0-5][0-9]):([0-5][0-9])$");
            return data;
           
        }

        

        public static string GetInvalidMessage(string field, string record)
        {
            var message = $"Invalid record '{field}' at no. {record}";
            return message;
        }


    }
}