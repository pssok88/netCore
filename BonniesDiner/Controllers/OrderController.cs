using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BonniesDiner.Data;
using BonniesDiner.Domain.Entity;
using BonniesDiner.Services;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonniesDiner.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly DinerContext _dinerContext;
        private readonly IEmailService _emailService;

        public OrderController(DinerContext dinerContext, IEmailService emailService)
        {
            _dinerContext = dinerContext;
            _emailService = emailService;
        }

        [HttpGet("[action]")]
        public IEnumerable<OrderEntity> GetOpenOrders()
        {
            var orders = _dinerContext.Order.Where(x => x.StatusFulfilled == null && x.StatusCancelled == null).ToList();

            foreach (OrderEntity order in orders)
            {
                List<OrderLineItemEntity> lineItems =
                    _dinerContext.OrderLineItem.Where(x => x.OrderEntityId == order.Id).ToList();
                foreach (OrderLineItemEntity lineItem in order.LineItems)
                {
                    lineItem.Item = _dinerContext.Menu.FirstOrDefault(x => x.Id == lineItem.ItemId);
                }

                order.LineItems = lineItems;
            }

            return orders;
        }
        [HttpGet("[action]")]
        public IEnumerable<OrderEntity> GetAllOrders()
        {
            var orders = _dinerContext.Order.ToList();

            foreach (OrderEntity order in orders)
            {
                List<OrderLineItemEntity> lineItems =
                    _dinerContext.OrderLineItem.Where(x => x.OrderEntityId == order.Id).ToList();
                foreach (OrderLineItemEntity lineItem in order.LineItems)
                {
                    lineItem.Item = _dinerContext.Menu.FirstOrDefault(x => x.Id == lineItem.ItemId);
                }

                order.LineItems = lineItems;
            }

            return orders;
        }
        [HttpGet("[action]/{orderId}")]
        public bool FulfillOrder(int orderId)
        {
            OrderEntity openOrder = _dinerContext.Order.FirstOrDefault(x => x.Id == orderId);

            if (openOrder == null) return false;

            openOrder.CompleteOrder(DateTime.Now);
            _dinerContext.Order.Update(openOrder);
            _dinerContext.SaveChanges();
            EmailMessage email = new EmailMessage();
            email.ToAddresses.Add(new EmailAddress { Address = User.Identity.Name, Name = User.Identity.Name });
            email.Subject = "Your Order has been completed!";
            email.Content = "Thanks for ordering! Your order has been completed!";
            email.FromAddresses.Add(new EmailAddress { Address = "bonniesdinerexsilio@gmail.com", Name = "Bonnies Diner" });
            
            _emailService.Send(email);
            return true;
        }
        [HttpGet("[action]/{orderId}")]
        public bool CancelOrder(int orderId)
        {
            OrderEntity openOrder = _dinerContext.Order.FirstOrDefault(x => x.Id == orderId);

            if (openOrder == null) return false;

            openOrder.CancelOrder(DateTime.Now);
            _dinerContext.Order.Update(openOrder);
            _dinerContext.SaveChanges();

            EmailMessage email = new EmailMessage();
            email.ToAddresses.Add(new EmailAddress { Address = User.Identity.Name, Name = User.Identity.Name });
            email.Subject = "Your Order has been cancelled!";
            email.Content = "Your order has been cancelled!";
            email.FromAddresses.Add(new EmailAddress { Address = "bonniesdinerexsilio@gmail.com", Name = "Bonnies Diner" });

            _emailService.Send(email);

            return true;
        }
        [HttpPost("[action]")]
        public bool CreateOrder([FromBody]List<CreateOrderEntity> order)
        {
            List<OrderLineItemEntity> items = new List<OrderLineItemEntity>();

            UserEntity currentUser = _dinerContext.User.FirstOrDefault(x => x.Email == User.Identity.Name);

            decimal total = 0m;

            foreach (CreateOrderEntity item in order)
            {
                MenuEntity menuItem = _dinerContext.Menu.FirstOrDefault(x => x.Id == item.MenuId);
                items.Add(new OrderLineItemEntity {Item = menuItem, Quantity = item.Quantity});
                total += menuItem.Price;
            }

            OrderEntity newOrder = new OrderEntity
            {
                LineItems = items,
                StatusNew = DateTime.Now,
                OrderTotal = total
            };

            currentUser.AddOrder(newOrder);
            _dinerContext.SaveChanges();

            EmailMessage email = new EmailMessage();
            email.ToAddresses.Add(new EmailAddress { Address = User.Identity.Name, Name = User.Identity.Name });
            email.Subject = "You have placed a new order!";
            email.Content = "Thanks for ordering! Your order will be ready shortly!";
            email.FromAddresses.Add(new EmailAddress { Address = "bonniesdinerexsilio@gmail.com", Name = "Bonnies Diner" });

            _emailService.Send(email);

            return true;
        }
    }
}