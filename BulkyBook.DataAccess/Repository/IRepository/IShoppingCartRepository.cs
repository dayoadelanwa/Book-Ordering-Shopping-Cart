﻿using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncremetCount(ShoppingCart shoppingCart, int count);
        int DecremetCount(ShoppingCart shoppingCart, int count);
    }
}
