using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace HillCipher.Controllers
{
    public static class XORCipherClass
    {
        private static string alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz ";

        private static int Mod(int a, int b)
        {
            if (a > 0 && b > 0)
            {
                return a % b;
            }
            else
            {
                while (a < 0)
                {
                    a += b;
                }
                return a;
            }
        }

        //Yt=( Yt-1+ Yt-3) mod 32  
        private static string GetGamma(string key, int n)
        {
            StringBuilder gamma = new StringBuilder(n);
            gamma.Append(key[0]);
            gamma.Append(key[1]);
            gamma.Append(key[2]);
            //create Yi
            for (int i=3;i<n;++i)
            {
                int index1 = alphabet.IndexOf(gamma[i-1]);
                int index2 = alphabet.IndexOf(gamma[i - 3]);
                gamma.Append(alphabet[Mod(index1+index2, alphabet.Length)]);
            }
            //create Zi
            for (int i = 1; i < n; ++i)
            {
                int index1 = alphabet.IndexOf(gamma[i]);
                int index2 = alphabet.IndexOf(gamma[i - 1]);
                gamma.Append(alphabet[Mod(index1 + index2, alphabet.Length)]);
            }

            return gamma.ToString();
        }

        public static string Encrypt(string text, string keyWord)
        {
            int n = text.Length;
            string gamma = GetGamma(keyWord, n);
            StringBuilder cipher = new StringBuilder(n);
            for (int i=0;i<n;++i)
            {
                int index1 = alphabet.IndexOf(text[i]);
                int index2 = alphabet.IndexOf(gamma[i]);
                cipher.Append(alphabet[Mod(index1+index2, alphabet.Length)]);
            }
            return cipher.ToString();
        }

        public static string Decrypt(string text, string keyWord)
        {
            int n = text.Length;
            string gamma = GetGamma(keyWord, n);
            StringBuilder cipher = new StringBuilder(n);
            for (int i = 0; i < n; ++i)
            {
                int index1 = alphabet.IndexOf(text[i]);
                int index2 = alphabet.IndexOf(gamma[i]);
                cipher.Append(alphabet[Mod(index1 + (alphabet.Length - index2), alphabet.Length)]);
            }
            return cipher.ToString();
        }
    }
}