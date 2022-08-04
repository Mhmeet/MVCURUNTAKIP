using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URUNTAKIPMVC.Models
{
    public class MultiView
    {
        public IEnumerable<Urunler> UrunList { get; set; }
        public IEnumerable<Kullanici> KullaniciList { get; set; }
        public IEnumerable<Kategori> KategoriList { get; set; }
        public IEnumerable<Birim> BirimList { get; set; }
        public IEnumerable<UrunGecmisi> UrunGecmisList { get; set; }
        public IEnumerable<Durum> Durumlist { get; set; }
    }
}