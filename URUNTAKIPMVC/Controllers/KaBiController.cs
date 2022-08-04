using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URUNTAKIPMVC.Models;

namespace URUNTAKIPMVC.Controllers
{
    public class KaBiController : Controller
    {
        // GET: KaBi
        MVCURUNYONETIMEntities db = new MVCURUNYONETIMEntities();
        public ActionResult KateBirim()
        {
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login/Login");
            }
            MVCURUNYONETIMEntities db = new MVCURUNYONETIMEntities();
            MultiView mv = new MultiView();
            mv.KategoriList = db.Kategoris.ToList();
            mv.BirimList = db.Birims.ToList();
            return View(mv);

        }
        public ActionResult YeniKateEkle()
        {
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login/Login");
            }
            return View();
        }

        public JsonResult yenikateadd(Kategori kate)
        {
            var durum = 0;
            Kategori kategori = new Kategori();
            kategori.KategoriAdı = kate.KategoriAdı;
            if (string.IsNullOrEmpty(kate.KategoriAdı))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            db.Kategoris.Add(kategori);
            durum = db.SaveChanges();
            if (durum > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult katDetay(int id)
        {

            if (id > 0)
            {
                var temp = db.Kategoris.FirstOrDefault(model => model.ID.Equals(id));
                if (temp != null)
                {

                    return Json(new
                    {
                        Result = new
                        {
                            temp.KategoriAdı,

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
        public JsonResult kategoriguncelle(Kategori kate)
        {
            var duurum = 0;
            var temp = db.Kategoris.FirstOrDefault(x => x.ID == kate.ID);
            if (kate.ID == 1)
            {
                return Json("Boş", JsonRequestBehavior.AllowGet);
            }
            if (temp.ID > 0)
            {
                if (!string.IsNullOrEmpty(kate.KategoriAdı))
                {
                    temp.KategoriAdı = kate.KategoriAdı;
                    duurum = db.SaveChanges();
                }

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
        public JsonResult silkat(int id)
        {
            var durum = 0;
            if (id > 0)
            {
                var temp = db.Kategoris.Where(x => x.ID == id).FirstOrDefault();
                var urun = db.Urunlers.Where(x => x.KategoriID == id).ToList();

                if (urun.Count > 0)
                {
                    return Json("Var", JsonRequestBehavior.AllowGet);
                }
                else if (id == 1)
                {
                    return Json("Boş", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    db.Kategoris.Remove(temp);
                    durum = db.SaveChanges();
                }
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
        public ActionResult YeniBirimEkle()
        {
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login/Login");
            }
            return View();
        }
        [HttpPost]
        public JsonResult yenibirimadd(Birim birim)
        {
            var durum = 0;
            Birim birm = new Birim();
            birm.BirimAdı = birim.BirimAdı;
            if (string.IsNullOrEmpty(birim.BirimAdı))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            db.Birims.Add(birm);
            durum = db.SaveChanges();
            if (durum > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult birimDetay(int id)
        {

            if (id > 0)
            {
                var temp = db.Birims.FirstOrDefault(model => model.ID.Equals(id));
                if (temp != null)
                {
                    return Json(new
                    {
                        Result = new
                        {
                            temp.BirimAdı,

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
        public JsonResult birimguncelle(Birim birim)
        {
            var duurum = 0;
            var temp = db.Birims.FirstOrDefault(x => x.ID == birim.ID);
            if (birim.ID == 1)
            {
                return Json("Boş", JsonRequestBehavior.AllowGet);
            }
            if (temp.ID > 0)
            {
                if (!string.IsNullOrEmpty(birim.BirimAdı))
                {
                    temp.BirimAdı = birim.BirimAdı;
                    duurum = db.SaveChanges();
                }

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
        public JsonResult silbirim(int id)
        {
            var durum = 0;
            if (id > 0)
            {
                var temp = db.Birims.Where(x => x.ID == id).FirstOrDefault();
                var urun = db.Kullanicis.Where(x => x.BirimID == id).ToList();

                if (urun.Count > 0)
                {
                    return Json("Var", JsonRequestBehavior.AllowGet);
                }
                else if (id == 1)
                {
                    return Json("Boş", JsonRequestBehavior.AllowGet);

                }
                db.Birims.Remove(temp);
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
    }
}