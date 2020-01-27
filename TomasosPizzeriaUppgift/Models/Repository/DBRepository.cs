using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaUppgift.Interface;
using TomasosPizzeriaUppgift.ViewModels;

namespace TomasosPizzeriaUppgift.Models.Repository
{
    public class DBRepository : IRepository
    {
        public Kund GetById(int id)
        {
            var model = new Kund();
            using (TomasosContext db = new TomasosContext())
            {
                model = db.Kund.FirstOrDefault(ratt => ratt.KundId == id);
            }
            return model;
                
        }
        public MenuPage GetMenuInfo()
        {
            var model = new MenuPage();

            using (TomasosContext db = new TomasosContext())
            {
                model.Matratter = db.Matratt.ToList();
                model.Ingredins.MatrattProdukt = db.MatrattProdukt.ToList();
                model.Ingredienses = db.Produkt.ToList();
                model.mattratttyper = db.MatrattTyp.ToList();

            }
            return model;
        }
        public void SaveUser(Kund user)
        {
            using (TomasosContext db = new TomasosContext())
            {
                db.Kund.Add(user);
                db.SaveChanges();
            }
        }
        public Kund GetUuserId(Kund customer)
        {
            var user = new Kund();
            using (TomasosContext db = new TomasosContext())
            {
                user = db.Kund.FirstOrDefault(r => r.AnvandarNamn == customer.AnvandarNamn && r.Losenord == customer.Losenord);
            }
            
            return user;
        }
        public Matratt GetMatratterToCustomerbasket(int id)
        {
            var model = new Matratt();
            using (TomasosContext db = new TomasosContext())
            {
                
                model = db.Matratt.FirstOrDefault(r => r.MatrattId == id);
            }
            return model;
        }

        public void SaveOrder(List<Matratt> matratter, int userid)
        {
            var customer = GetById(userid);
            var totalmoney = GetTotalPayment(matratter);
            var bestallning = new Bestallning()
            {
                BestallningDatum = DateTime.Now,
                KundId = customer.KundId,
                Totalbelopp = totalmoney,
                Levererad = false
            };


            using (TomasosContext db = new TomasosContext())
            {
                db.Add(bestallning);
                db.SaveChanges();
            }
            SaveBestallningMatratter(matratter);

        }

        public void SaveBestallningMatratter(List<Matratt> matratter)
        {
            var result = matratter.GroupBy(item => item)
                      .Select(item => new
                      {
                          Name = item.Key,
                          Count = item.Count()
                      })
                      .ToList();

            
            using (TomasosContext db = new TomasosContext())
            {
                var listbestallning = db.Bestallning.OrderByDescending(r => r.BestallningDatum).ToList();
                var bestallningsmatrattlista = new List<BestallningMatratt>();
                var id = 0;
                var first = 0;
                var count = 0;
                matratter.OrderBy(r => r.MatrattNamn).ToList();
                var best = new BestallningMatratt();
                for(var i = 0; i < matratter.Count; i++)
                {
                    
                    if(id != matratter[i].MatrattId)
                    {
                        first++;
                        id = matratter[i].MatrattId;
                        best.BestallningId = listbestallning[0].BestallningId;
                        best.MatrattId = matratter[i].MatrattId;
                        best.Antal = 1;
                        bestallningsmatrattlista.Add(best);

                    }
                    else if(id == matratter[i].MatrattId)
                    {
                        count = first - 1;
                        bestallningsmatrattlista[count].Antal++;

                    }

                    
                }
                db.AddRange(bestallningsmatrattlista);
                db.SaveChanges();
            }
        }

        public int GetTotalPayment(List<Matratt> matratter)
        {
            var totalmoney = 0;
            foreach (var matratt in matratter)
            {
                totalmoney += matratt.Pris;
            }
            return totalmoney;
        }
    }

}
