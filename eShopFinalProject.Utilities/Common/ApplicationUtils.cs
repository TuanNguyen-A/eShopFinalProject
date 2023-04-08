using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.Common
{
    public static class ApplicationUtils
    {
        public static JwtSecurityToken ReadJwtToken(string authorization)
        {
            var jwtToken = authorization.Substring(0, 7).ToLower() == "bearer " ?
                authorization.ToString().Substring("bearer ".Length) : authorization;
            var tokenValidator = new JwtSecurityTokenHandler();
            return tokenValidator.ReadJwtToken(jwtToken);
        }
        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string Slugify(this string phrase)
        {
            string output = phrase.RemoveAccents().ToLower();
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");
            output = Regex.Replace(output, @"\s+", " ").Trim();
            output = Regex.Replace(output, @"\s", "-");

            return output;
        }
    }
}
