using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.ViewModels;
using TomasosPizzeriaUppgift.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TomasosPizzeriaUppgift.Interface;
using TomasosPizzeriaUppgift.Models.Repository;

namespace TomasosPizzeriaUppgift.Services
{
    public class Services
    {
        private static Services instance = null;
        private static readonly Object padlock = new Object();
        private IRepository _repository;


        public static Services Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Services();
                        instance._repository = new DBRepository();

                    }
                    return instance;

                }
            }
        }


        public Services()
        {
        }




        public Kund GetUserId(Kund customer)
        {
            return _repository.GetUserId(customer);


        }

        public void SetInloggedCustomer(Kund customer)
        {

        }
        public Kund GetById(int id)
        {
            return _repository.GetById(id);
        }

        public MenuPage GetMenuInfo()
        {
            return _repository.GetMenuInfo(); ;
        }
        public void SaveUser(Kund user)
        {
            _repository.SaveUser(user);
        }

        public Matratt GetMatratterById(int id)
        {
            return _repository.GetMatratterToCustomerbasket(id);
        }
        public void UserPay(List<Matratt> matratter, int userid)
        {
            _repository.SaveOrder(matratter, userid);

        }
        public void UpdateUser(Kund user, int userid)
        {
            _repository.UpdateUser(user, userid);
        }
        public List<MatrattTyp> GetMatratttyper()
        {
            return _repository.GetMatrattTyper();
        }
        public Kund CheckUserName(Kund customer)
        {
            return _repository.CheckUserName(customer);
        }


    }
}
