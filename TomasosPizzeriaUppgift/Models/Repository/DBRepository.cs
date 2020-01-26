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

    }
}
