using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApplication1.Tools {
    public static class StringHelper {
        public static bool HasBlank(this string key) {
            int strLen = key.Length;
            //key.Replace(" ", "");
            string trim = Regex.Replace(key, @"\s", "");
            int strLen2 = trim.Length;
            if (strLen != strLen2)
                return true;
            else
                return false;
        }

        public static bool LegalLength(this string key) {
            int strLen = key.Length;
            if (strLen > 7 && strLen <= 32)
                return true;
            else
                return false;
        }

        public static bool HasNum(this string key) {
            //bool findANum = 0;
            Regex r = new Regex(@"\d+");
            if (r.IsMatch(key))
                return true;
            else
                return false;
        }
        public static bool HasLetter(this string key) {
            Regex r = new Regex(@"[a-z]+");
            if (r.IsMatch(key))
                return true;
            else
                return false;
        }
        public  static bool HasCapLetter(this string  key) {
            Regex r = new Regex(@"[A-Z]+");
            if (r.IsMatch(key))
                return true;
            else
                return false;
        }
        public  static bool HasSpecial(this string  key) {
            Regex r = new Regex(@"[A-Z]+");
            if (r.IsMatch(key))
                return true;
            else
                return false;
        }
    }
}
