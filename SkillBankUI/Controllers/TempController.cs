using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web.UI;

namespace SkillBankWeb.Controllers
{
    public class TempController : Controller
    {
        //
        // GET: /Temp/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveImage(String imagePath)
        {
            int x = 0, y = 0, w = 100, h = 100;
            //string filename = Request["upload-photo"];

            byte[] image = CropImage(imagePath, w, h, x, y);
            SaveToDrive(image);
            return Json("true");
        }


        private byte[] CropImage(string Img, int Width, int Height, int X, int Y)
        {
            try
            {
                using (var OriginalImage = new Bitmap(Img))
                {
                    using (var bmp = new Bitmap(Width, Height, OriginalImage.PixelFormat))
                    {
                        bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                        using (Graphics Graphic = Graphics.FromImage(bmp))
                        {
                            Graphic.SmoothingMode = SmoothingMode.AntiAlias; Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, Width, Height), X, Y, Width, Height, GraphicsUnit.Pixel);
                            var ms = new MemoryStream();
                            bmp.Save(ms, OriginalImage.RawFormat);
                            return ms.GetBuffer();
                        }
                    }
                }
            }
            catch (Exception Ex) { throw (Ex); }
        }

        private void SaveToDrive(byte[] image)
        {
            //byte[] bytes = (byte[])dr["Image"];
            FileStream fs = new FileStream(@"E:\SourceCode\VS\SkillBank\SkillBankUI\" + "test.jpg", FileMode.Create, FileAccess.Write);
            fs.Write(image, 0, image.Length);
            fs.Flush();
            fs.Close();
        }

    }
}
