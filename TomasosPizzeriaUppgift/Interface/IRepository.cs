﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.ViewModels;

namespace TomasosPizzeriaUppgift.Interface
{
    interface IRepository
    {
        Kund GetById(int id);
        MenuPage GetMenuInfo();
        void SaveUser(Kund user);
        Kund GetUuserId(Kund customer);
        Matratt GetMatratterToCustomerbasket(int id);
    }
}