
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace URUNTAKIPMVC.Models
{

using System;
    using System.Collections.Generic;
    
public partial class UrunGecmisi
{

    public int ID { get; set; }

    public int UrunID { get; set; }

    public string Notu { get; set; }



    public virtual Urunler Urunler { get; set; }

}

}
