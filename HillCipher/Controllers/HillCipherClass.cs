using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace HillCipher.Controllers
{
    public static class HillCipherClass
    {
        private static string alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz ";

        private static int Euler(int a, int b, out int x, out int y)///розширений алгоритм Ейлера
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
            int x1, y1;
            int d = Euler(b % a, a, out x1, out y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }

        private static int Det(int[,] a, int n)
        {
            int det = 0, p, h, k, i, j;
            int[,]temp=new int[n,n];
            if(n==1) {
                return a[0,0];
            } else if(n==2) {
                det=(a[0,0]* a[1,1]-a[0,1]* a[1,0]);
                return det;
            } else {
                for(p=0;p<n;p++) {
                    h = 0;
                    k = 0;
                    for(i=1;i<n;i++) {
                        for(j=0;j<n;j++) {
                            if(j==p) {
                                continue;
                            }
                            temp[h,k] = a[i,j];
                            k++;
                            if(k==n-1) {
                                h++;
                                k = 0;
                            }
                        }
                    }
                    det=det+a[0,p]* (int)Math.Pow(-1, p)* Det(temp, n-1);
                }
                return det;
            }
        }

        private static int[,] CreateMatKey(string kw)
        {
            int[,] matKey;
            int n = (int)Math.Sqrt(kw.Length) == Math.Sqrt(kw.Length) ? (int)Math.Sqrt(kw.Length) : 0;
            if (n == 0)
                throw new Exception("Hill cipher, bad key world(lenght="+n.ToString()+")");
            matKey = new int[n, n];
            for (int i = 0, k = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    matKey[i, j] = alphabet.IndexOf(kw[k++]);
                }
            }

            int det = Det(matKey, n);
            if (det==0)
            {
                throw new Exception("Hill chipher, bad det(matKey, det==0)");
            }
            if (Euler(det, n, out int a, out int b)!=1)
            {
                throw new Exception("Hill chipher, bad Euler(det, n)");
            }
            return matKey;
        }

        //////////////////////////////////////////////////////
        private static int Minor(int[,] a,int i, int j)
        {
            int n = (int)Math.Sqrt(a.Length);
            int m = n - 1;
            int[,] res = new int[m,m];
            for (int i1=0,k=0;i1<n;++i1,++k)
            {
                if (i1 == i)
                {
                    --k;
                    continue;
                }
                for (int j1=0,l=0;j1<n;++j1,++l)
                {
                    if (j1 == j)
                    {
                        --l;
                        continue;
                    }
                    res[k, l] = a[i1, j1];
                }
            }
            return (Det(res, m)*((int)Math.Pow(-1, (i+1)+(j+1))));
        }
        
        private static void Trans(ref int[,] a)
        {
            int n = (int)Math.Sqrt(a.Length);
            for (int i=0;i<n; ++i)
            {
                for (int j=i;j<n;++j)
                {
                    int temp = a[i, j];
                    a[i, j] = a[j, i];
                    a[j, i] = temp;
                }
            }
        }

        private static int InverseDet(int det, int m)
        {
            int x, y;
            int d = Euler(det,m,out x, out y);
            if (det < 0 && x > 0)
            {
                return x;
            }
            if (det > 0 && x < 0)
            {
                return m + x;
            }
            if (det > 0 && x > 0)
            {
                return x;
            }
            if (det < 0 && x < 0)
            {
                return -x;
            }
            return -1;
        }

        private static int[,] Inverse(int[,] a)
        {
            int n= (int)Math.Sqrt(a.Length);
            int det = Det(a,n);
            int inDet = InverseDet(det, alphabet.Length);
            if (inDet == -1)
            {
                throw new Exception("Hill Chipher, bad inverse det");
            }

            int[,] inverse = new int[n,n];
            for (int i=0;i<n;++i)
            {
                for (int j=0;j<n;++j)
                {
                    inverse[i, j] = Minor(a, i, j);
                }
            }
            Trans(ref inverse);

            for (int i=0;i< n;++i)
            {
                for (int j=0;j<n;++j)
                {
                    inverse[i, j] = inverse[i, j] * inDet;
                }
            }

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    inverse[i, j] = Mod(inverse[i, j], alphabet.Length);
                }
            }
            return inverse;
        }

        //////////////////////////////////////////////////////

        public static int[,] Mul(int[,] a, int[,] b)
        {
            int n = (int)Math.Sqrt(a.Length);
            int[,] res = new int[n,n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    for (int k = 0; k < n; ++k)
                    {
                        res[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return res;
        }

        private static int[] Mul(int[] a, int[,] b)
        {
            int n = a.Length;
            int[] res = new int[n];
            for (int i=0;i<n;++i)
            {
                for (int j=0;j<n;++j)
                {
                    res[i] += a[j] * b[j, i];
                }
            }
            return res;
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

        public static string Encrypt(string text, string keyWord)
        {
            int[,] matKey;
            try
            {
                matKey = CreateMatKey(keyWord);
            }
            catch (Exception mes)
            {
                return mes.Message;
            }
            int n = (int)Math.Sqrt(keyWord.Length);
            if (Det(matKey,n)<=0)
            {
                return "Det(A)<=0";
            }

            StringBuilder encrypt = new StringBuilder();
            for (int i = 0; i < text.Length; i += n)
            {
                int[] bloc = new int[n];
                for (int j = i, k = 0; j < n + i; ++j, ++k)
                {
                    if (j >= text.Length)
                    {
                        bloc[k] = alphabet.IndexOf(' ');
                        continue;
                    }
                    bloc[k] = alphabet.IndexOf(text[j]);
                }
                int[] res = Mul(bloc, matKey);
                for (int l = 0; l < n; ++l)
                {
                    res[l] = Mod(res[l], alphabet.Length);
                    encrypt.Append(alphabet[res[l]]);
                }
            }
            return encrypt.ToString();
        }

        public static string Decrypt(string text, string keyWord)
        {
            int[,] matKey;
            try
            {
                matKey = CreateMatKey(keyWord);
            }
            catch (Exception mes)
            {
                return mes.Message;
            }

            int n = (int)Math.Sqrt(keyWord.Length);

            int[,] matKeyIn = Inverse(matKey);
            int[,] resmul = Mul(matKeyIn, matKeyIn);

            for (int l = 0; l < n; ++l)
            {
                for (int l1=0; l1<n;++l1) {
                    resmul[l,l1] = Mod(resmul[l,l1], alphabet.Length);
                }
            }
            StringBuilder encrypt = new StringBuilder();
            for (int i = 0; i < text.Length; i += n)
            {
                int[] bloc = new int[n];
                for (int j = i, k = 0; j < n + i; ++j, ++k)
                {
                    if (j >= text.Length)
                    {
                        bloc[k] = alphabet.IndexOf(' ');
                        continue;
                    }
                    bloc[k] = alphabet.IndexOf(text[j]);
                }
                int[] res = Mul(bloc, matKeyIn);
                for (int l = 0; l < n; ++l)
                {
                    res[l] = Mod(res[l], alphabet.Length);
                    encrypt.Append(alphabet[res[l]]);
                }
            }
            return encrypt.ToString();
        }
    }
}