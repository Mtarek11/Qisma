using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Reposatiory
{
    public class OrderManager(LoftyContext _mydB, UnitOfWork _unitOfWork, BuyTrackerManager _buyTrackerManager, PropertyManager _propertyManager,
        AccountManager _accountManager) : MainManager<Order>(_mydB)
    {
        private readonly UnitOfWork unitOfWork = _unitOfWork;
        private readonly BuyTrackerManager buyTrackerManager = _buyTrackerManager;
        private readonly PropertyManager propertyManager = _propertyManager;
        private readonly AccountManager accountManager = _accountManager;
        public async Task<APIResult<string>> CreateOrderAsync(string userId, string propertyId, int numberOfShares)
        {
            APIResult<string> aPIResult = new();
            BuyTracker buyTracker = await buyTrackerManager.GetAll().Where(i => i.UserId == userId && i.PropertyId == propertyId).Select(i => new BuyTracker()
            {
                PropertyId = i.PropertyId,
                UserId = i.UserId,
                LastProceedDate = i.LastProceedDate,
            }).FirstOrDefaultAsync();
            if (buyTracker == null)
            {
                aPIResult.Message = "Cannot create order before going to property details page";
                aPIResult.StatusCode = 409;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            Property property = await propertyManager.GetAll().Where(i => i.Id == propertyId && i.IsDeleted == false).Select(i => new Property()
            {
                SharePrice = i.SharePrice,
                AvailableShares = i.AvailableShares,
                MinOfShares = i.MinOfShares,
                DeliveryInstallment = i.DeliveryInstallment,
                DownPayment = i.DownPayment,
                Id = i.Id,
                LastModificationDate = i.LastModificationDate,
                MaintenaceInstallment = i.MaintenaceInstallment,
                MaintenanceCost = i.MaintenanceCost,
                MonthlyInstallment = i.MonthlyInstallment,
                NumberOfYears = i.NumberOfYears,
                TransactionFees = i.TransactionFees,
                PropertyUnitPrices = i.PropertyUnitPrices,
                UsedShares = i.UsedShares,
                Location = i.Location,
                NumberOfShares = i.NumberOfShares
            }).FirstOrDefaultAsync();
            if (property == null)
            {
                aPIResult.Message = "Property not found";
                aPIResult.StatusCode = 404;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            else
            {
                if (buyTracker.LastProceedDate <= property.LastModificationDate)
                {
                    aPIResult.Message = "Property updated, go to property details page again";
                    aPIResult.StatusCode = 409;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
                if (numberOfShares > property.AvailableShares)
                {
                    aPIResult.Message = "Number of shares must be less than or equals to available shares";
                    aPIResult.StatusCode = 409;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
                if (numberOfShares < property.MinOfShares)
                {
                    aPIResult.Message = "Number of shares must be more than or equals to min shares";
                    aPIResult.StatusCode = 409;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
                User user = await accountManager.GetAll().Where(i => i.Id == userId).Select(i => new User()
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    DateOfBirth = i.DateOfBirth,
                    Email = i.Email
                }).FirstOrDefaultAsync();
                propertyManager.PartialUpdate(property);
                property.UsedShares += numberOfShares;
                Order order = new()
                {
                    UserId = userId,
                    OrderNumber = propertyId + user.FirstName.First() + user.LastName.Last() + user.DateOfBirth.Day.ToString() + user.DateOfBirth.Month,
                    PropertyId = propertyId,
                    SharePrice = property.SharePrice,
                    NumberOfShares = numberOfShares,
                    OrderStatus = OrderStatus.Pending,
                    TransactionFees = property.TransactionFees != null ? (property.TransactionFees / property.NumberOfShares) * numberOfShares : 0,
                    DownPayment = property.DownPayment != null ? property.DownPayment / numberOfShares : 0,
                    MonthlyInstallment = property.MonthlyInstallment != null ? property.MonthlyInstallment / numberOfShares : 0,
                    NumberOfYears = property.NumberOfYears != null ? property.NumberOfYears : 0,
                    MaintenaceInstallment = property.MaintenaceInstallment != null ? property.MaintenaceInstallment / numberOfShares : 0,
                    DeliveryInstallment = property.DeliveryInstallment != null ? property.DeliveryInstallment / numberOfShares : 0,
                    OrderDate = DateTime.Now,
                };
                await AddAsync(order);
                await unitOfWork.CommitAsync();
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("mohamedtarek70m@gmail.com", "ptuw vvlf rkue elgh");
                    client.EnableSsl = true;
                    MailAddress from = new MailAddress("mohamedtarek70m@gmail.com");
                    MailAddress to = new MailAddress(user.Email);
                    string subject = $"Your Order Confirmation - {property.Location} Fractional Ownership";
                    string body = $@"<html>
<head>
<style>
.bold {{
    font-weight: bold;
}}
</style>
</head>
<body>
Hello {user.FirstName} {user.LastName},<br>
Thank you for choosing Qisma for your investment in {property.Location}. We’re pleased to confirm that your purchase of fractional shares has been processed successfully.<br>
Someone from our customer service department will contact you shortly to walk you through the next steps. <br><br>
<span class='bold'>Here are the details of your investment:</span><br>
Property Name: {property.Location}<br>
Fractional Shares Purchased: {order.NumberOfShares}<br>
Total Investment: {(order.NumberOfShares * order.SharePrice).ToString("F2")}<br>
First Payment: {order.DownPayment?.ToString("F2")}<br>
Transaction ID: {order.OrderNumber}<br><br>
You are kindly required to complete the first payment, {order.DownPayment?.ToString("F2")} EGP, within 48 hours. If payment is done through bank transfer or Instapay, please send a proof of payment to billing@qisma.co<br><br>
We’ve attached the order confirmation form to this email for your records. If you have any questions or if there’s anything else you need, please contact our support team. <br><br>
We’re here to help!<br>
Thanks again for your trust,<br><br>
QISMA
</body>
</html>";
                    MailMessage message = new MailMessage(from, to)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true // Set to true to indicate HTML content
                    };
                    client.Send(message);
                }
                aPIResult.Message = "Order placed";
                aPIResult.StatusCode = 200;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
        public async Task RealasePendingSharesAsync()
        {
            List<Order> orders = await GetAll().Where(i => i.OrderDate.AddDays(2) < DateTime.Now && i.OrderStatus == OrderStatus.Pending).Select(i => new Order()
            {
                Id = i.Id,
                Property = new Property()
                {
                    Id = i.Property.Id,
                    UsedShares = i.Property.UsedShares,
                    NumberOfShares = i.Property.NumberOfShares,
                    AvailableShares = i.Property.AvailableShares
                }
            }).ToListAsync();
            if (orders.Count > 0)
            {
                foreach (Order order in orders)
                {
                    Property property = order.Property;
                    propertyManager.PartialUpdate(property);
                    property.UsedShares -= order.NumberOfShares;
                    Remove(order);
                }
                await unitOfWork.CommitAsync();
            }
        }
        public async Task<PaginationViewModel<OrderViewModelForAdmin>> GetAllOrdersForAdminAsync(int pageNumber, int pageSize, OrderStatus orderStatus)
        {
            int itemsToSkip = pageNumber * pageSize;
            int totalPageNumbers = 0;
            int totalItemsNumber = 0;
            List<OrderViewModelForAdmin> Orders = await GetAll().Where(i => i.OrderStatus == orderStatus).Select(OrderExtansions.ToOrderDetailsViewModelForAdminExpression())
                .Skip(itemsToSkip).Take(pageSize).ToListAsync();
            if (Orders.Count > 0)
            {
                int totalItems = await GetAll().Where(i => i.OrderStatus == orderStatus).CountAsync();
                totalItemsNumber = totalItems;
                totalPageNumbers = (int)Math.Ceiling((double)totalItems / pageSize);
            }
            PaginationViewModel<OrderViewModelForAdmin> paginationViewModel = new()
            {
                ItemsList = Orders,
                TotalPageNumbers = totalPageNumbers,
                TotalItemsNumber = totalItemsNumber
            };
            return paginationViewModel;
        }
        public async Task<APIResult<string>> ConfirmOrderPaymentAsync(int orderId)
        {
            APIResult<string> aPIResult = new();
            Order order = await GetAll().Where(i => i.Id == orderId).Select(i => new Order()
            {
                Id = i.Id,
                OrderStatus = i.OrderStatus,
                NumberOfShares = i.NumberOfShares,
                Property = new Property()
                {
                    Id = i.Property.Id,
                    UsedShares = i.Property.UsedShares,
                    NumberOfShares = i.Property.NumberOfShares,
                }
            }).FirstOrDefaultAsync();
            if (order != null)
            {
                if (order.OrderStatus == OrderStatus.Pending)
                {
                    PartialUpdate(order);
                    order.OrderStatus = OrderStatus.Confirmed;
                    order.Property.NumberOfShares = order.Property.NumberOfShares - order.NumberOfShares;
                    order.Property.UsedShares = order.Property.UsedShares - order.NumberOfShares;
                    await unitOfWork.CommitAsync();
                    aPIResult.Message = "Order confirmed";
                    aPIResult.StatusCode = 200;
                    aPIResult.IsSucceed = true;
                    return aPIResult;
                }
                else
                {
                    aPIResult.Message = "Order is already confirmed";
                    aPIResult.StatusCode = 409;
                    aPIResult.IsSucceed = false;
                    return aPIResult;
                }
            }
            else
            {
                aPIResult.Message = "Order not found";
                aPIResult.StatusCode = 404;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
        }
        public async Task<PaginationViewModel<OrderViewModelForUser>> GetAllOrdersForUserAsync(int pageNumber, int pageSize, string userId, OrderStatus orderStatus)
        {
            int itemsToSkip = pageNumber * pageSize;
            int totalPageNumbers = 0;
            int totalItemsNumber = 0;
            List<OrderViewModelForUser> Orders = await GetAll().Where(i => i.OrderStatus == orderStatus && i.UserId == userId)
                .Select(OrderExtansions.ToOrderDetailsViewModelForUserExpression())
                .Skip(itemsToSkip).Take(pageSize).ToListAsync();
            if (Orders.Count > 0)
            {
                int totalItems = await GetAll().Where(i => i.OrderStatus == orderStatus && i.UserId == userId).CountAsync();
                totalItemsNumber = totalItems;
                totalPageNumbers = (int)Math.Ceiling((double)totalItems / pageSize);
            }
            PaginationViewModel<OrderViewModelForUser> paginationViewModel = new()
            {
                ItemsList = Orders,
                TotalPageNumbers = totalPageNumbers,
                TotalItemsNumber = totalItemsNumber
            };
            return paginationViewModel;
        }
        //public async Task<UserPortfolioViewModel> GetUserPortfolioAsync(string userId)
        //{
        //    UserPortfolioViewModel userPortfolio = new();
        //    List<Order> userOrders = await GetAll().Where(i => i.UserId == userId && i.OrderStatus == OrderStatus.Confirmed).Select(i => new Order()
        //    {
        //        SharePrice = i.SharePrice,
        //        NumberOfShares = i.NumberOfShares,
        //        ConfirmationDate = i.ConfirmationDate,
        //        Property = new Property()
        //        {
        //          SharePrice = i.Property.SharePrice,
        //          NumberOfShares = i.Property.NumberOfShares,
        //          PropertyRentalYields = i.Property.PropertyRentalYields,
        //          AnnualPriceAppreciation = i.Property.AnnualPriceAppreciation,
        //          PropertyUnitPrices = i.Property.PropertyUnitPrices
        //        },
        //    }).ToListAsync();
        //    if (userOrders.Count > 0)
        //    {
        //        foreach(Order order in  userOrders)
        //        {
        //            userPortfolio.ProtfolioValue += (order.Property.SharePrice * order.NumberOfShares);
        //            DateTime orderConfirmationDate = (DateTime)order.ConfirmationDate;
        //            double propertyRentalYield = order.Property.PropertyRentalYields.Where(i =>i.To == null).Select(i => i.RentalYield).FirstOrDefault();
        //            List<PropertyUnitPrice> propertyUnitPrices = order.Property.PropertyUnitPrices.Where(i => i.To == null || i.To >= new DateTime(DateTime.Now.Year, 1, 1)
        //            ).ToList();
        //            foreach(PropertyUnitPrice propertyUnitPrice in propertyUnitPrices)
        //            {
        //                if (propertyUnitPrice.To == null)
        //                {
        //                    if (new DateTime(DateTime.Now.Year, 1, 1) >= orderConfirmationDate)
        //                    {
        //                        int numberOfMonthes = DateTime.Now.Month;
        //                        userPortfolio.GrossMonthlyIncome = userPortfolio.GrossMonthlyIncome + (propertyRentalYield * propertyUnitPrice.UnitPrice * order.NumberOfShares)
        //                            / (order.Property.NumberOfShares * numberOfMonthes);
        //                    }
        //                    else
        //                    {
        //                        int numberOfMonthes = DateTime.Now.Month - orderConfirmationDate.Month + 1;
        //                        userPortfolio.GrossMonthlyIncome = userPortfolio.GrossMonthlyIncome + (propertyRentalYield * propertyUnitPrice.UnitPrice * order.NumberOfShares)
        //                            / (order.Property.NumberOfShares * numberOfMonthes);
        //                    }
        //                }
        //                else
        //                {
        //                    if (propertyUnitPrice.)
        //                    {
        //                        int numberOfMonthes = DateTime.Now.Month;
        //                        userPortfolio.GrossMonthlyIncome = userPortfolio.GrossMonthlyIncome + (propertyRentalYield * propertyUnitPrice.UnitPrice * order.NumberOfShares)
        //                            / (order.Property.NumberOfShares * numberOfMonthes);
        //                    }
        //                    else
        //                    {
        //                        int numberOfMonthes = DateTime.Now.Month - orderConfirmationDate.Month + 1;
        //                        userPortfolio.GrossMonthlyIncome = userPortfolio.GrossMonthlyIncome + (propertyRentalYield * propertyUnitPrice.UnitPrice * order.NumberOfShares)
        //                            / (order.Property.NumberOfShares * numberOfMonthes);
        //                    }
        //                }
        //            }
        //        }
        //        userPortfolio.CurrentMonth = DateTime.Now.Month;
        //        userPortfolio.NumberOfProperties = userOrders.Count;
        //    }
        //}
    }
}
