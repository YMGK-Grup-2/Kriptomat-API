using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace YMG.Controllers
{
    public class DenemeController : ApiController
    {
        [Route("Resim")]
        [HttpPost]
        public static string Resim()
        {
            Guid guid = Guid.NewGuid();//Resim yolları benzersiz olabilmesi için Guid üretildi
            var request = HttpContext.Current.Request;//Mobilden gelen resmi alır
            var photo = request.Files["photo"];//mobilden gelen key değerini alır
            string uzanti = Path.GetExtension(photo.FileName);//resmin uzantısını alır
            
           photo.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads/" + photo.FileName + guid + uzanti));//resmi Uploads klasörüne kaydeder
            string uzan = HttpContext.Current.Server.MapPath("~/Uploads/" + photo.FileName + guid + uzanti);
            return uzan;
        }
        [HttpGet]
        public int Dosya()
        {
           
           string deger = Resim();
            Bitmap bmp = new Bitmap(deger);
            int boyut = bmp.Size.Height;
            return boyut;
        }
    }
}
