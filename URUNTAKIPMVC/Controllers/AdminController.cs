using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using URUNTAKIPMVC.Models;

namespace URUNTAKIPMVC.Controllers
{
    public class AdminController : Controller
    {
        MVCURUNYONETIMEntities db = new MVCURUNYONETIMEntities();
        // GET: Admin
        public ActionResult AnaSayfa()
        {
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login/Login");
            }
            MultiView mv = new MultiView();
            mv.KullaniciList = db.Kullanicis.ToList();
            mv.UrunList = db.Urunlers.ToList();
            mv.BirimList = db.Birims.ToList();
            mv.KategoriList = db.Kategoris.ToList();
            mv.Durumlist = db.Durums.ToList();
            return View(mv);
        }
        public ActionResult YeniUrunEkle()
        {
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login/Login");
            }
            MultiView mv = new MultiView();
            mv.BirimList = db.Birims.ToList();
            mv.KategoriList = db.Kategoris.ToList();
            mv.Durumlist = db.Durums.ToList();
            return View(mv);
        }
        [HttpPost]
        public JsonResult YeniUrunEkle(Urunler urunnler)
        {
            DateTime time = new DateTime(2000, 1, 1, 0, 0, 0);
            var durum = 0;
            int adet = 1;

            if (urunnler.Adet != 0 || urunnler.Adet != 1)
            {
                adet = urunnler.Adet;
            }
            for (int i = 0; i < adet; i++)
            {
                Urunler newUrun = new Urunler();
                newUrun.KullaniciID = 1;
                newUrun.DurumID = urunnler.DurumID;
                newUrun.Marka = urunnler.Marka;
                newUrun.Model = urunnler.Model;
                newUrun.Adet = 1;
                newUrun.SeriNo = urunnler.SeriNo;
                newUrun.atandiMi = urunnler.atandiMi;
                newUrun.KategoriID = urunnler.KategoriID;
                newUrun.PDF = false;
                newUrun.Fiyat = urunnler.Fiyat;
                newUrun.notlar = urunnler.notlar;
                newUrun.VerilenTarih = time;
                newUrun.AlinanTarih = time;
                if (string.IsNullOrEmpty(urunnler.Marka) || string.IsNullOrEmpty(urunnler.Model) || urunnler.Fiyat == 0 || urunnler.Adet == 0 || string.IsNullOrEmpty(urunnler.SeriNo))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                db.Urunlers.Add(newUrun);
                durum = db.SaveChanges();

            }
            if (durum > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        //public string RelativeToAbsoluteURLS(string text)
        //{
        //    if (String.IsNullOrEmpty(text))
        //        return text;

        //    string absoluteUrl = Request.PhysicalApplicationPath;
        //    String value = Regex.Replace(text, "<(.*?)(src)=\"(?!http)(.*?)\"(.*?)>", "<$1$2=\"" + absoluteUrl + "$3\"$4>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //    return value.Replace(absoluteUrl + "/", absoluteUrl);
        //}

        [HttpPost]
        public JsonResult UrunDetay(int id)
        {

            if (id > 0)
            {
                var temp = db.Urunlers.FirstOrDefault(model => model.ID.Equals(id));
                if (temp != null)
                {
                    string atanMi;
                    if (temp.atandiMi == true)
                    {
                        atanMi = "evet";
                    }
                    else
                    {
                        atanMi = "hayır";
                    }
                    return Json(new
                    {
                        Result = new
                        {
                            temp.Marka,
                            temp.Model,
                            temp.SeriNo,
                            temp.DurumID,
                            temp.KategoriID,
                            temp.Fiyat,
                            temp.notlar,
                            atanMi = atanMi,

                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult urunguncelle(Urunler urun)
        {
            var duurum = 0;
            MVCURUNYONETIMEntities db = new MVCURUNYONETIMEntities();
            var temp = db.Urunlers.FirstOrDefault(x => x.ID == urun.ID);
            if (temp.ID > 0)
            {
                temp.KategoriID = urun.KategoriID;
                temp.DurumID = urun.DurumID;
                temp.Model = urun.Model;
                temp.Marka = urun.Marka;
                temp.SeriNo = urun.SeriNo;
                temp.Fiyat = urun.Fiyat;
                temp.notlar = urun.notlar;
                duurum = db.SaveChanges();
            }
            if (duurum > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpPost]
        public JsonResult Atama(int id, int idisim)
        {
            var temp = db.Urunlers.Where(x => x.ID == id).FirstOrDefault();
            if (temp == null)
            {
                return Json("urunbos", JsonRequestBehavior.AllowGet);
            }
            else if (idisim == 1)
            {
                return Json("atanmadi", JsonRequestBehavior.AllowGet);
            }
            else
            {
                temp.VerilenTarih = DateTime.Now;
                temp.KullaniciID = idisim;
                temp.atandiMi = true;
                temp.PDF = false;
                temp.DurumID = 2;
                db.SaveChanges();
                return Json("atandi", JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult gerial(int id, int kuid)
        {
            string ugnotu;
            UrunGecmisi ug = new UrunGecmisi();
            var temp = db.Urunlers.Where(x => x.ID == id).FirstOrDefault();
            var kul = db.Kullanicis.Where(x => x.ID == kuid).FirstOrDefault();
            temp.KullaniciID = 1;
            temp.AlinanTarih = DateTime.Now;
            temp.atandiMi = false;
            temp.PDF = false;
            ug.UrunID = id;
            ugnotu = kul.AdiSoyadi + " İsimli Kişi " + temp.VerilenTarih + " Tarihinde ürünü almış ve " + DateTime.Now + " yerine teslim etmiştir";
            ug.Notu = ugnotu;
            db.UrunGecmisis.Add(ug);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult silurun(int id)
        {
            var durum = 0;
            if (id > 0)
            {
                var temp = db.Urunlers.Where(x => x.ID == id).FirstOrDefault();
                var urungecmis = db.UrunGecmisis.Where(x => x.UrunID == id).ToList();
                var dosyalarr = db.Dosyalars.Where(x => x.UrunID == id).ToList();
                for (int i = 0; i < dosyalarr.Count; i++)
                {
                    db.Dosyalars.Remove(dosyalarr[i]);
                }
                for (int i = 0; i < urungecmis.Count; i++)
                {
                    db.UrunGecmisis.Remove(urungecmis[i]);

                }
                db.Urunlers.Remove(temp);
                durum = db.SaveChanges();
            }
            if (durum > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult kullaniciList()
        {
            MVCURUNYONETIMEntities db = new MVCURUNYONETIMEntities();
            MultiView mv = new MultiView();
            mv.UrunGecmisList = db.UrunGecmisis.ToList();
            mv.KullaniciList = db.Kullanicis.ToList();
            mv.BirimList = db.Birims.ToList();
            return View(mv);
        }



        public ActionResult urungecmis(int id)
        {
            MultiView mv = new MultiView();
            mv.UrunGecmisList = db.UrunGecmisis.Where(x => x.UrunID == id).ToList();
            return View(mv);
        }


        public ActionResult PrintPDF(string id)
        {

            string[] splitli = id.Split(',');
            int kulid = int.Parse(splitli[0]);
            int urunid = int.Parse(splitli[1]);


            MultiView mv = new MultiView();
            mv.UrunList = db.Urunlers.Where(x => x.ID == urunid).ToList();
            mv.KullaniciList = db.Kullanicis.Where(x => x.ID == kulid).ToList();
            return View(mv);
            //return new PartialViewAsPdf("PrintPDF", mv)
            //{
            //    FileName = "OKSITEMURUNTAKIP.pdf"
            //};
        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult ExportHTML(string ExportData)
        //{
        //    //MemoryStream stream = new MemoryStream();
        //    //StringReader reader = new StringReader(ExportData);
        //    //Document PdfFile = new Document(PageSize.A4);
        //    //PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
        //    //PdfFile.Open();
        //    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
        //    //PdfFile.Close();
        //    //return File(stream.ToArray(), "application/pdf", "ExportData.pdf");
        //    HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
        //    HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
        //    HtmlDocument doc = new HtmlDocument();
        //    doc.OptionFixNestedTags = true;
        //    doc.LoadHtml(ExportData);
        //    ExportData = doc.DocumentNode.OuterHtml;

        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        Encoding unicode = Encoding.UTF8;
        //        StringReader sr = new StringReader(ExportData);
        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //        pdfDoc.Close();
        //        return File(stream.ToArray(), "application/pdf", "ExportData.pdf");
        //    }
        //}

    }
}