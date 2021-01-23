using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static System.Net.WebRequestMethods;

namespace YMG.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
       
        public string Get()
        {
            return ramveri.veri();
        }
        
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]

        public int[] Post()
        {
            Guid guid = Guid.NewGuid();
            var request = HttpContext.Current.Request;
            var photo = request.Files["photo"];
            string uzanti = Path.GetExtension(photo.FileName);
            photo.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads/" + photo.FileName + guid + uzanti));
            //mobilden gelen resim kaydedilip Bitmap nesnesine atılıdı
            Bitmap bmp = new Bitmap(HttpContext.Current.Server.MapPath("~/Uploads/" + photo.FileName + guid + uzanti));
            int boyut = bmp.Size.Height * bmp.Size.Width * 3 * 8;
           
            int boyut2 = bmp.Size.Height * bmp.Size.Width * 3;
            int[] veri = new int[boyut];
            int k = 0;
            
                 
                for (int i = 0; i < bmp.Size.Width; i++)
                {
                for (int j = 0; j < bmp.Size.Height; j++)
                {
                    Color renk = bmp.GetPixel(i,j);
                    veri[k] = (int)(renk.R);
                    veri[k + 1] = (int)(renk.G);
                    veri[k + 2] = (int)(renk.B);
                    k += 3;
                }
                
                
                
            }







            string[] ip1 = sha3(boyut);
            string[] ip3 = IP3(boyut);
            int[] Xor = XOR(ip1, ip3, boyut);
            
            int[] Deger = XOR2(Xor, veri, boyut2);
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
            ornek.Save(HttpContext.Current.Server.MapPath("~/Uploads/" + "Sifreli metin2" + guid + uzanti));
            return veri;

        }
        public static byte[] GetBytesFromHexString(string hexString)
        {
            if (hexString == null)
                return null;

            if (hexString.Length % 2 == 1)
                hexString = '0' + hexString; // Up to you whether to pad the first or last byte

            byte[] data = new byte[hexString.Length / 2];

            for (int i = 0; i < data.Length; i++)
                data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            Console.WriteLine(data);
            return data;
        }


        //resim boyutuna eşit olana kadar Sha3 ten veri çek
        public string[] sha3(int boyut)
        {
            byte[] dizi=new byte[32];
           
            string[] dizi2 = new string[boyut];
            int i = 0;
            while (i < boyut)
            {
                if (i < boyut)
                {
                    dizi = GetBytesFromHexString(ramveri.veri());
                }
                int n = dizi.Length;
                int j = 0;
                while (j < n)
                {
                    if (i == boyut)
                    {
                        break;
                    }
                    else
                    {
                        string deger = Convert.ToString(dizi[j], 2).PadLeft(8, '0');
                        dizi2[i] = deger;
                        i++;
                        j++;
                    }
                }

            }


            return dizi2;





        }

        public string[] IP3(int boyut)
        {
            double xYeni = 0;
            Random rnd = new Random();
            double[] kaotik = {
      0.468789897897,
      0.623295737125813,
      0.133421210716500,
      0.149521122345238,
      0.459420242005092,
      0.907702731014926,
      0.811794118685255,
      0.521594771719064,


        };

            string[] dizi = new string[boyut];
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
                        string s = "";
                        int t = 0;
                        while (t < 8)
                        {



                            xYeni = xEski * (1 - xEski) * 4;

                            if (xYeni < 0.5)
                                s = s + "1";
                            else
                                s = s + "0";
                            xEski = xYeni;



                            t++;
                        }
                        k += 8;
                        dizi[i] = s;
                    }
                    

                }
            }
            return dizi;
        }

        public int[] XOR(string[] dizi, string[] dizi1, int boyut)
        {
            //int[] dizi = { 0, 1, 0, 1, 0, 1 };
            //int[] dizi2 = { 1, 0, 0, 0, 0, 1 };
           int[] dizi3 = new int[boyut];
            for (int i = 0; i < dizi3.Length; i++)
            {
                dizi3[i] = Convert.ToInt32(Convert.ToString(Convert.ToInt32(dizi[i], 2), 10)) ^ Convert.ToInt32(Convert.ToString(Convert.ToInt32(dizi1[i], 2), 10));
            }
            return dizi3;
        }
        public int[] XORu(int[] dizi, int[] dizi1, int boyut)
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
        public int[] XOR2(int[] dizi, int[] dizi1, int boyut)
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

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
