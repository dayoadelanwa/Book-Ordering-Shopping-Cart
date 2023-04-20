using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context= context;
        }

        public void Update(OrderHeader obj)
        {
            _context.OrderHeaders.Update(obj);
        }
        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderfromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderfromDb != null)
            {
                orderfromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderfromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePayment(int id, string sessionId, string paymentInentId)
        {
            var orderfromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            orderfromDb.PaymentDate = DateTime.Now;

            orderfromDb.SessionId = sessionId;
            orderfromDb.PaymentIntentId = paymentInentId;
        }

    }
}
