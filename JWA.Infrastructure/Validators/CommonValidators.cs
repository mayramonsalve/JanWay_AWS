using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JWA.Infrastructure.Validators
{
    public static class CommonValidators
    {
        public static bool ValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber,
                              @"(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2‌​-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9]‌​[02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})");
        }
        public static bool ValidMacAddress(string macAddress)
        {
            string beggining = "ATKMPID-";
            string end = "-&**";
            bool validBeggining = macAddress.StartsWith(beggining);
            bool validEnd = macAddress.EndsWith(end);
            if(validBeggining && validEnd)
            {
                string mAddress = macAddress.Replace(beggining, "").Replace(end, "");
                return Regex.IsMatch(mAddress, @"([0-9A-F]{2}[:-]){5}([0-9A-F]{2})$");
            }
            return false;
        }
    }
}
