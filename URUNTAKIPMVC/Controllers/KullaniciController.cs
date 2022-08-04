using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URUNTAKIPMVC.Models;

namespace URUNTAKIPMVC.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        MVCURUNYONETIMEntities db = new MVCURUNYONETIMEntities();
        public ActionResult KullaniciList()
        {
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login/Login");
            }
            MultiView mv = new MultiView();
            mv.UrunGecmisList = db.UrunGecmisis.ToList();
            mv.KullaniciList = db.Kullanicis.ToList();
            mv.BirimList = db.Birims.ToList();
            return View(mv);
        }
        public ActionResult YeniKullaniciEkle()
        {
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login/Login");
            }
            MultiView mv = new MultiView();
            mv.BirimList = db.Birims.ToList();
            return View(mv);
        }
        [HttpPost]
        public JsonResult yenikullaniciadd(Kullanici kul)
        {
            var durum = 0;
            Kullanici kullanici = new Kullanici();
            kullanici.AdiSoyadi = kul.AdiSoyadi;
            kullanici.BirimID = kul.BirimID;
            if (string.IsNullOrEmpty(kul.AdiSoyadi) || kul.BirimID == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            db.Kullanicis.Add(kullanici);
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
        public JsonResult KullDetay(int id)
        {

            if (id > 0)
            {
                var temp = db.Kullanicis.FirstOrDefault(model => model.ID.Equals(id));
                if (temp != null)
                {

                    return Json(new
                    {
                        Result = new
                        {

                            temp.AdiSoyadi,
                            temp.BirimID

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
        public JsonResult kullaniciguncelle(Kullanici kull)
        {
            var duurum = 0;
            var temp = db.Kullanicis.FirstOrDefault(x => x.ID == kull.ID);
            if (kull.ID == 1)
            {
                return Json("Boş", JsonRequestBehavior.AllowGet);
            }
            if (temp.ID > 0)
            {
                if (!string.IsNullOrEmpty(kull.AdiSoyadi))
                {
                    temp.AdiSoyadi = kull.AdiSoyadi;
                    temp.BirimID = kull.BirimID;
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
        public JsonResult silkull(int id)
        {
            var durum = 0;
            if (id > 0)
            {
                var temp = db.Kullanicis.Where(x => x.ID == id).FirstOrDefault();
                var urun = db.Urunlers.Where(x => x.KullaniciID == id).ToList();

                if (urun.Count > 0)
                {
                    return Json("Var", JsonRequestBehavior.AllowGet);
                }
                else if (id == 1)
                {
                    return Json("Boş", JsonRequestBehavior.AllowGet);

                }
                db.Kullanicis.Remove(temp);
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

        public ActionResult kullaniciUrunlerr(int id)
        {
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login/Login");
            }
            MultiView mv = new MultiView();
            mv.KullaniciList = db.Kullanicis.Where(x => x.ID == id).ToList();
            mv.UrunList = db.Urunlers.Where(x => x.KullaniciID == id).ToList();
            return View(mv);
        }
        [HttpPost]
        public JsonResult Yazdir(int id, int tmp)
        {
            var temp = db.Urunlers.Where(x => x.KullaniciID.Equals(id)).ToList();
            foreach (var item in temp)
            {
                if (tmp != 1)
                {
                    item.PDF = true;
                    db.SaveChanges();
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}