using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Auth.SharedKernel
{
    public static class Extensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> elements, int? page, out int total)
        {
            total = elements.Count();
            return !page.HasValue ? elements.Take(total) : elements.Skip(10 * ((int)page - 1)).Take(10);
        }

        public static List<T> Paginate<T>(this List<T> elements, int? page, out int total)
        {
            total = elements.Count();
            return (List<T>) (!page.HasValue ? elements.Take(total) : elements.Skip(10 * ((int)page - 1)).Take(10));
        }

        // public static bool IsCNPJ(this long cnpj)
        // {
        //     var first = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        //     var second = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        //     var cnpjString = cnpj.CastCnpj();
        //     if (cnpjString.Length != 14)
        //         return false;
        //     var cnpjRange = cnpjString.Substring(0, 12);
        //     var sumValue = 0;
        //     for (var i = 0; i < 12; i++)
        //         sumValue += int.Parse(cnpjRange[i].ToString()) * first[i];
        //     var remnant = (sumValue % 11);
        //     if (remnant < 2)
        //         remnant = 0;
        //     else
        //         remnant = 11 - remnant;
        //     var digit = remnant.ToString();
        //     cnpjRange = cnpjRange + digit;
        //     sumValue = 0;
        //     for (var i = 0; i < 13; i++)
        //         sumValue += int.Parse(cnpjRange[i].ToString()) * second[i];
        //     remnant = (sumValue % 11);
        //     if (remnant < 2)
        //         remnant = 0;
        //     else
        //         remnant = 11 - remnant;
        //     digit += remnant;

        //     return cnpjString.EndsWith(digit);
        // }

        public static bool IsValidEMail(this string mail)
            => !string.IsNullOrEmpty(mail) && new Regex(@"^([\w-\.]+)@((\[[\d]{1,3}\.[\d]{1,3}\.[\d]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[\d]{1,3})(\]?)$")
                   .IsMatch(mail);

        public static bool IsValidPhone(this string phone)
            => !string.IsNullOrEmpty(phone) && new Regex(@"^(\([0-9]{2}\))\s([9]{1})?([0-9]{4})-([0-9]{4})$")
                   .IsMatch(phone);

        //public static string CastCnpj(this long value)
        //{
        //    var cpf = @"/^\d{3}\.\d{3}\.\d{3}\-\d{2}$/";
        //    var cnpj = @"/^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/";

        //    var stringValue = $"{value}";

        //    if (stringValue.RegexMatch(cpf, out var match))
        //    {
        //        return value.ToString();
        //    }
        //    else
        //    {
        //        stringValue.RegexMatch(cnpj, out match);
        //        return value.ToString("00000000000000");
        //    }
        //}

        public static long CastToLong(this string cnpj) => Convert.ToInt64(cnpj);

        public static DateTime DateWithOutHour(this DateTime date) => new DateTime(date.Year, date.Month, date.Day);

        public static DateTime DateWithOnlyHour(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);

        public static bool IsValidEnum<T>(this T value) => value != null && Enum.IsDefined(typeof(T), value);

        public static T DeserializeJson<T>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
        }

        public static T DeserializeJsonWithoutToken<T>(this HttpContent content)
        {
            var jsonString = content.DeserializeJson<ApiReturn>();
            var json = JsonConvert.SerializeObject(jsonString.data);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string DeserializeJsonError(this HttpContent content)
        {
            var jsonString = content.DeserializeJson<ApiReturn>();
            return jsonString.error;
        }

        public static byte[] CastBase64(this string image)
        {
            var index = image.IndexOf("base64", StringComparison.Ordinal);
            if(index == -1)
                return Convert.FromBase64String(image);

            var range = image.Substring(0, index + 7);
            image = image.Replace(range, "");

            return Convert.FromBase64String(image);
        }

        public static IDictionary<string, T> ToDictionary<T>(this object obj)
        {
            if (obj == null)
                return null;

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
                dictionary.Add(property.Name, (T)Convert.ChangeType(property.GetValue(obj), typeof(T)));
            return dictionary;
        }
    }

    public class ApiReturn
    {
        public string error { get; set; }
        public object data { get; set; }
        public string token { get; set; }
    }
}