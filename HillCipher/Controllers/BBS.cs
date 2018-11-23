using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Text;

namespace HillCipher.Controllers
{
    public static class BBS
    {
        private static bool Prime(int n)///Перевірка на простоту
        {
            int ns = (int)Math.Ceiling(Math.Sqrt(n));
            for (int i = 2; i <= ns; i++)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }

        private static int RNPG(int a, int b)///рандомна генерація простого числа з діапазону [a,b]
        {
            List<int> arr = new List<int>();
            while (a < b)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                if (Prime(a))
                {
                    arr.Add(a);
                }
                a = a + 1;
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds>1000&&arr.Count>2)
                {
                    break;
                }
            }
            
            Random r = new Random((int)((a+b)/2));
            return arr[r.Next() % arr.Count];
        }

        public static int Gcd(int a, int b)///Алгоритм Евкліда(найбільший спільний дільник)
        {
            while (b != 0)
                b = a % (a = b);
            return a;
        }

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

        public static byte[] BBSMethod(int length, out int[] _xi, out int _p, out int _q, out int _n)
        {
            int p=3;
            int q=5;
            for (int i=100;i<999;++i) {
                p = RNPG(i , i+10 * length);
                q = RNPG(i+10 * length, i+20 * length);
                if (p % 4 == 3 && q % 4 == 3)
                {
                    break;
                }
                if (i == 998) i = 100;
            }
            int n = p * q;
            Random random = new Random(p*q/length);

            int x = random.Next();
            int x0 =x % n;
            if (Gcd(x, n) != 1)
            {
                while (Gcd(x, n) != 1)
                {
                    x = random.Next();
                    x0 = x % n;
                }
            }
            int[] xi = new int[length];
            byte[] arr = new byte[length];
            xi[0] = (x * x) % n;
            arr[0] = Convert.ToByte(Mod(xi[0], 2));
            for (int i =1; i< length;++i)
            {
                xi[i] = (xi[i-1] * xi[i-1]) % n;
                arr[i] = Convert.ToByte(Mod(xi[i],2));
            }
            _p = p;
            _q = q;
            _n = n;
            _xi = xi;
            return arr;
        }

        public static byte[] BBSMethod(int length, int n)
        {

            Random random = new Random(n / length);

            int x = random.Next();
            int x0 = x % n;
            if (Gcd(x, n) != 1)
            {
                while (Gcd(x, n) != 1)
                {
                    x = random.Next();
                    x0 = x % n;
                }
            }
            int[] xi = new int[length];
            byte[] arr = new byte[length];
            xi[0] = (x * x) % n;
            arr[0] = Convert.ToByte(Mod(xi[0], 2));
            for (int i = 1; i < length; ++i)
            {
                xi[i] = (xi[i - 1] * xi[i - 1]) % n;
                arr[i] = Convert.ToByte(Mod(xi[i], 2));
            }
            return arr;
        }



        public static string Encrypt(string text)
        {
            int[] xi;
            int n, p, q;
            byte[] arr = BBSMethod(text.Length, out xi, out p, out q, out n);
            StringBuilder builder = new StringBuilder(text.Length);
            for (int i=0;i<text.Length;++i)
            {
                builder.Append((char)(text[i]^arr[i]));
            }
            builder.AppendLine();
            builder.AppendLine($"n={n}, p={p}, q={q}");
            for (int i=0;i<arr.Length;++i)
            {
                builder.Append($"{arr[i]} ");
            }
            builder.AppendLine();
            for (int i = 0; i < xi.Length; ++i)
            {
                builder.Append($"{xi[i]} ");
            }
            return builder.ToString();
        }

        public static string Decrypt(string text, int n)
        {
            byte[] arr = BBSMethod(text.Length, n);
            StringBuilder builder = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; ++i)
            {
                builder.Append((char)(text[i] ^ arr[i]));
            }
            return builder.ToString();
        }
    }
}