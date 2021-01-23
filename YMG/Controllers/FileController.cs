using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;


namespace YMG.Controllers
{
    [System.Web.Http.RoutePrefix("api/File")]
    public class FileController : ApiController
    {
        
      
        [Route("Upload")]
        [HttpPost]
       
        public string Upload()
        {
            Guid guid =  Guid.NewGuid();//Resim yolları benzersiz olabilmesi için Guid üretildi
            var request = HttpContext.Current.Request;//Mobilden gelen resmi alır
            var photo = request.Files["photo"];//mobilden gelen key değerini alır
            string uzanti = Path.GetExtension(photo.FileName);//resmin uzantısını alır
            photo.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads/"+ photo.FileName + guid + uzanti));//resmi Uploads klasörüne kaydeder
           
            Bitmap bmp = new Bitmap(HttpContext.Current.Server.MapPath("~/Uploads/" + photo.FileName + guid + uzanti)); //mobilden gelen resim kaydedilip Bitmap nesnesine atar

            int boyut = bmp.Size.Height * bmp.Size.Width * 3*8;//resmin boyutunun kaç bit olduğunu bulur
            int boyut2 = bmp.Size.Height * bmp.Size.Width * 3;
          int[] veri = new int[boyut];//resim boyutu kadar dizi tanımlanır
           
                //Gelen resim fraktal kullanarak tek boyutlu veri[] dizisine atılır
                int k = 0;
            int ilksatir = 0;
            int sonsatir = 1;
            while (ilksatir < bmp.Size.Width)
            {
                for (int i = 0; i < bmp.Size.Height; i++)
                {
                    Color renk = bmp.GetPixel(ilksatir, i);
                    int[] dizi = donustur((int)(renk.R));
                    int ver = (int)(renk.R);

                    for (int p = dizi.Length - 1; p > 0; p--)
                    {
                        veri[k] = dizi[p];
                        k++;

                    }
                    int[] dizi1 = donustur((int)(renk.G));
                    int vert = (int)(renk.G);
                    for (int t = dizi.Length - 1; t > 0; t--)
                    {
                        veri[k] = dizi1[t];
                        k++;
                    }
                    int[] dizi2 = donustur((int)(renk.B));
                    int verti = (int)(renk.G);
                    for (int t = dizi.Length - 1; t > 0; t--)
                    {
                        veri[k] = dizi2[t];
                        k++;
                    }
                }
                ilksatir = ilksatir + 1;
                if (sonsatir > bmp.Size.Width)
                {
                    for (int i = bmp.Size.Height - 1; i >= 0; i--)
                    {
                        Color renk = bmp.GetPixel(sonsatir, i);
                        int[] dizi = donustur((int)(renk.R));//gelen piksel değerlerini 8 bitlik bit değerlerine dönüştürür ör:255=11111111
                        int veri1 = (int)(renk.R);

                        for (int p = dizi.Length - 1; p > 0; p--)//8 bitlik değerleri tek tek veri dizisine atar
                        {
                            veri[k] = dizi[p];
                            k++;

                        }
                        int[] dizi1 = donustur((int)(renk.G));
                        int veri2 = (int)(renk.G);
                        for (int t = dizi.Length - 1; t > 0; t--)
                        {
                            veri[k] = dizi1[t];
                            k++;
                        }
                        int[] dizi2 = donustur((int)(renk.B));
                        int veri3 = (int)(renk.G);
                        for (int t = dizi.Length - 1; t > 0; t--)
                        {
                            veri[k] = dizi2[t];
                            k++;
                        }
                    }
                }
                sonsatir = sonsatir + 1;
            }

            //gelen resim tek boyutlu veri[] dizisine atıldı

           
            //Resim pikselleri boyut kadar veriyi bit dizisine attı
           /*int[] ip1 = sha3(boyut);*///IP1 DEN GELEN RAM VERİLERİ VE SHA3 FONKSİYONU KULANILARAK ELDE EDİLİMİŞ DİZİ
            int[] ip3 = IP3(boyut); //IP3 DEN GELEN KAOTİK DİZİ
            int[] Xor = XOR(ip3 , ip3, boyut);//IP1 VE IP3 XOR İŞLEMİ
            
            int[] Resim = XOR(Xor, veri, boyut);//XOR İŞLEMİNDEN DÖNEN SONUÇ İLE RESİM ARASINDA XOR İŞLEMİ
            
            int[] Deger = ddd(Resim, boyut2);

            //şifrelenmiş bitleri tekrar resim haline getiriri

            Bitmap ornek = new Bitmap(bmp.Size.Width, bmp.Size.Height);
            int parametre = 0;
            for (int i = 0; i < bmp.Size.Width; i++)
            {
                for (int j = 0; j < bmp.Size.Height; j++)
                {
                    byte[] renkkodu = BitConverter.GetBytes(Deger[parametre]);
                    byte[] renkkodu1 = BitConverter.GetBytes(Deger[parametre + 1]);
                    byte[] renkkodu2 = BitConverter.GetBytes(Deger[parametre + 2]);
                    ornek.SetPixel(i, j, Color.FromArgb(renkkodu[0], renkkodu1[0], renkkodu2[0]));
                    parametre += 3;
                }
            }
            //..SON


            Image image = (Image)ornek;
           ornek.Save(HttpContext.Current.Server.MapPath("~/Uploads/"+"Sifrelimetin"+guid+uzanti));//Şifrelenmiş veriyi klasöre kaydeder
            return "webmenudemo.online/Uploads/"+"Sifrelimetin"+guid + uzanti;//resim yolu geri dönderilir
           

        }
        //resimden gelen byte dizisinin 8 bitlik binary dizisine çevir
        public int[] donustur(int sayı)
        {
            int[] dizi = new int[9];
            
            int toplam = 0;
            int sayac = 0;
            while (sayı >= 2)
            {
                toplam = Convert.ToInt32(toplam + sayı % 2 * Math.Pow(10, sayac));
                sayı = sayı / 2;
                sayac++;
            }

            toplam += Convert.ToInt32(sayı * Math.Pow(10, sayac));
            string deger = toplam.ToString();
            char[] chars = deger.ToCharArray();
            Console.WriteLine(chars.Length + " " + dizi.Length);
            for (int i = dizi.Length - 1; i > 0; i--)
            {
                if (chars.Length < i)
                {
                    dizi[8 - i] = 0;
                }
                else
                {
                    dizi[i] = Convert.ToInt32(chars[chars.Length - i].ToString());
                }
               
            }
            return dizi;

        }


        //Sha3 ten gelen değeri Binary Çevirme
        private static readonly Dictionary<char, string> hexCharacterToBinary = new Dictionary<char, string> {
    { '0', "0000" },
    { '1', "0001" },
    { '2', "0010" },
    { '3', "0011" },
    { '4', "0100" },
    { '5', "0101" },
    { '6', "0110" },
    { '7', "0111" },
    { '8', "1000" },
    { '9', "1001" },
    { 'a', "1010" },
    { 'b', "1011" },
    { 'c', "1100" },
    { 'd', "1101" },
    { 'e', "1110" },
    { 'f', "1111" }
};
        //Sha3 ten gelen değeri Binary Çevirme
        public static int[] HexStringToBinary()
        {
            StringBuilder result = new StringBuilder();
           string hex=ramveri.veri();
            int[] dizi = new int[256];
            foreach (char c in hex)
            {
                result.Append(hexCharacterToBinary[char.ToLower(c)]);//karekerler ekleniyor
            }
            for (int i = 0; i < dizi.Length; i++)
            {
                dizi[i] = Convert.ToInt32(result[i].ToString());
            }

            return dizi;
        }



        //resim boyutuna eşit olana kadar Sha3 ten veri çek
        public int[] sha3(int boyut)
        {
            int[] dizi = new int[boyut];
           
            int i = 0;
            int[] dizi2 = new int[256];
            while (i < boyut)
            {
                
                if (i < boyut)
                {
                    int[] dizi3 = HexStringToBinary();
                    dizi2 = dizi3;
                }
               
                int j = 0;
                while (j < dizi2.Length)
                {
                   
                    if (i == boyut)
                    {
                        break;
                    }
                    else
                    {
                        dizi[i] = dizi2[j];
                        i++;
                        j++;
                    }
                   
                }
               

            }

            return dizi;

            
                
                
            
        }


        //IP3 üretilen kaotik değerler
        public int[] IP3(int boyut)
        {
            double xYeni = 0;
            Random rnd = new Random();
            double[] kaotik = {
      0.623295737125813,
      0.133421210716500,
      0.149521122345238,
      0.459420242005092,
      0.907702731014926,
      0.811794118685255,
      0.521594771719064,

        };
           
            int[] dizi = new int[boyut];
            int k = 0;
            while (k < dizi.Length)
            {
                double xEski = kaotik[rnd.Next(kaotik.Length)];
                for (int i = 0; i < 1000000; i++)
                {
                    if (k == dizi.Length)
                    {
                        break;
                    }
                    else
                    {
                        xYeni = xEski * (1 - xEski) * 4;
                        if (xYeni < 0.5)
                            dizi[k] = 1;
                        else
                            dizi[k] = 0;
                        xEski = xYeni;
                    }


                    k++;
                }
            }
            return dizi;
        }


        public int[] XOR(int[] dizi,int[] dizi1,int boyut)
        {
            //int[] dizi = { 0, 1, 0, 1, 0, 1 };
            //int[] dizi2 = { 1, 0, 0, 0, 0, 1 };
            int[] dizi3 = new int[boyut];
            for (int i = 0; i < dizi3.Length; i++)
            {
                dizi3[i] = dizi[i] ^ dizi1[i];
            }
            return dizi3;
        }


        //binary diziyi byte çevirme

        public static int binaryToDecimal(int n)
        {

            int dec_value = 0;

            int base1 = 1;

            int temp = n;
            while (temp > 0)
            {
                int last_digit = temp % 10;
                temp = temp / 10;

                dec_value += last_digit * base1;

                base1 = base1 * 2;
            }

            return dec_value;
        }

        public static int[] ddd(int[] dizi,int boyut)
        {
           
            int[] dizi1 = new int[boyut];
            int[] dizi2 = new int[boyut];
            int i = 0;
            int k = 0;
            int j = 0;
            while (i < dizi.Length)
            {

                string a = dizi[k].ToString();
                k++;
                string b = dizi[k].ToString();
                k++;
                string c = dizi[k].ToString();
                k++;
                string d = dizi[k].ToString();
                k++;
                string e = dizi[k].ToString();
                k++;
                string f = dizi[k].ToString();
                k++;
                string g = dizi[k].ToString();
                k++;
                string h = dizi[k].ToString();
                k++;

                dizi1[j] = Convert.ToInt32(string.Concat(a, b, c, d, e, f, g, h));
                dizi2[j] = binaryToDecimal(dizi1[j]);
                j++;
                a = "";
                b = "";
                c = "";
                d = "";
                e = "";
                f = "";
                g = "";
                h = "";


                i += 8;
            }
            return dizi2;
        }






    }


    }

   

