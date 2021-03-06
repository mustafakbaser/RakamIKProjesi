﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RakamIKProjesi.Models.Siniflar;
using PagedList;
using PagedList.Mvc;

namespace RakamIKProjesi.Controllers
{
    public class ProjelerController : Controller
    {
        // GET: Projeler
        Context context = new Context();
        int sayfa = 1;
        public ActionResult IndexArama(string p)
        {
            var projeler = from x in context.Personels select x;
            if (!string.IsNullOrEmpty(p))
            {
                projeler = projeler.Where(y => y.PersonelAd.Contains(p));
            }
            return View(projeler.ToList().ToPagedList(sayfa,5));
        }

        public ActionResult Index(int sayfa = 1)
        {
            var projeler = context.Projelers.ToList().ToPagedList(sayfa, 5);
            return View(projeler);
        }

        [HttpGet]
        public ActionResult YeniProje()
        {
            List<SelectListItem> birimKayit = (from p in context.Birims.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = p.BIRIM,
                                                   Value = p.BirimID.ToString()
                                               }).ToList();
            ViewBag.birimKayit = birimKayit;
            return View();
            //List<SelectListItem> deger1 = (from x in context.Projelers.ToList() select new SelectListItem {   Text = x.KullanilanTeknoloji, Value = x.ProjeID.ToString()}).ToList(); ViewBag.dgr1 = deger1;
            return View();
        }

        [HttpPost]
        public ActionResult YeniProje(Projeler proje)
        {
            context.Projelers.Add(proje);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ProjeTamamla(int id)
        {
            var projeDegeri = context.Projelers.Find(id);
            projeDegeri.ProjeAktifligi = false;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ProjeDevamEt(int id)
        {
            var projeDegeri = context.Projelers.Find(id);
            projeDegeri.ProjeAktifligi = true;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ProjeVeri(int id)
        {
            var projeDegeri = context.Projelers.Find(id);
            return View("ProjeVeri", projeDegeri);
        }
        public ActionResult ProjeGuncelle(Projeler proje)
        {
            List<SelectListItem> birimKayit = (from p in context.Birims.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = p.BIRIM,
                                                   Value = p.BirimID.ToString()
                                               }).ToList();
            ViewBag.birimKayit = birimKayit;
            return View();
            var mevcutDeger = context.Projelers.Find(proje.ProjeID);
            mevcutDeger.ProjeAd = proje.ProjeAd;
            mevcutDeger.KullanilanTeknoloji = proje.KullanilanTeknoloji;
            mevcutDeger.BirimID = proje.BirimID;
            mevcutDeger.ProjeAktifligi = proje.ProjeAktifligi;
            mevcutDeger.BaslangicTarihi = proje.BaslangicTarihi;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}