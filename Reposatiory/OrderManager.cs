using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                aPIResult.Message = "Cannot create order before go to ordering page";
                aPIResult.StatusCode = 409;
                aPIResult.IsSucceed = false;
                return aPIResult;
            }
            Property property = await propertyManager.GetAll().Where(i => i.Id == propertyId).Select(i => new Property()
            {
                SharePrice = i.SharePrice,
                AvailableShares = i.AvailableShares,
                MinOfShares = i.MinOfShares,
                AnnualPriceAppreciation = i.AnnualPriceAppreciation,
                AnnualRentalYield = i.AnnualRentalYield,
                DeliveryInstallment = i.DeliveryInstallment,
                DownPayment = i.DownPayment,
                Id = i.Id,
                LastModificationDate = i.LastModificationDate,
                MaintenaceInstallment = i.MaintenaceInstallment,
                MaintenanceCost = i.MaintenanceCost,
                MonthlyInstallment = i.MonthlyInstallment,
                NumberOfYears = i.NumberOfYears,
                TransactionFees = i.TransactionFees,
                UnitPrice = i.UnitPrice,
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
                    aPIResult.Message = "Property updated, go to ordering page again";
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
                    DateOfBirth = i.DateOfBirth
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
                    AnnualPriceAppreciation = property.AnnualPriceAppreciation,
                    AnnualRentalYield = property.AnnualRentalYield,
                    DeliveryInstallment = property.DeliveryInstallment != null ? property.DeliveryInstallment / numberOfShares : 0,
                    DownPayment = property.DownPayment != null ? property.DownPayment / numberOfShares : 0,
                    MaintenaceInstallment = property.MaintenaceInstallment != null ? property.MaintenaceInstallment / numberOfShares : 0,
                    MaintenanceCost = property.MaintenanceCost != null ? property.MaintenanceCost / numberOfShares : 0,
                    MonthlyInstallment = property.MonthlyInstallment != null ? property.MonthlyInstallment / numberOfShares : 0,
                    NumberOfYears = property.NumberOfYears != null ? property.NumberOfYears : 0,
                    OrderDate = DateTime.Now,
                    TransactionFees = property.TransactionFees != null ? property.TransactionFees / numberOfShares : 0
                };
                await AddAsync(order);
                await unitOfWork.CommitAsync();
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
            }).ToListAsync();
            if (orders.Count > 0)
            {
                foreach(Order order in orders)
                {
                    Remove(order);
                }
                await unitOfWork.CommitAsync();
            }
        }
        public async Task<PaginationViewModel<OrderViewModelForAdmin>> GetAllPendingOrdersForAdminAsync(int pageNumber, int pageSize)
        {
            int itemsToSkip = pageNumber * pageSize;
            int totalPageNumbers = 0;
            int totalItemsNumber = 0;
            List<OrderViewModelForAdmin> Orders = await GetAll().Where(i => i.OrderStatus == OrderStatus.Pending).Select(OrderExtansions.ToOrderDetailsViewModelForAdminExpression())
                .Skip(itemsToSkip).Take(pageSize).ToListAsync();
            if (Orders.Count > 0)
            {
                int totalItems = await GetAll().Where(i => i.OrderStatus == OrderStatus.Pending).CountAsync();
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
        public async Task<PaginationViewModel<OrderViewModelForAdmin>> GetAllConfirmedOrdersForAdminAsync(int pageNumber, int pageSize)
        {
            int itemsToSkip = pageNumber * pageSize;
            int totalPageNumbers = 0;
            int totalItemsNumber = 0;
            List<OrderViewModelForAdmin> Orders = await GetAll().Where(i => i.OrderStatus == OrderStatus.Confirmed).Select(OrderExtansions.ToOrderDetailsViewModelForAdminExpression())
                .Skip(itemsToSkip).Take(pageSize).ToListAsync();
            if (Orders.Count > 0)
            {
                int totalItems = await GetAll().Where(i => i.OrderStatus == OrderStatus.Confirmed).CountAsync();
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
                OrderStatus = i.OrderStatus
            }).FirstOrDefaultAsync();
            if (order != null)
            {
                if (order.OrderStatus == OrderStatus.Pending)
                {
                    PartialUpdate(order);
                    order.OrderStatus = OrderStatus.Confirmed;
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
        public async Task<PaginationViewModel<OrderViewModelForUser>> GetAllPendingOrdersForUserAsync(int pageNumber, int pageSize, string userId)
        {
            int itemsToSkip = pageNumber * pageSize;
            int totalPageNumbers = 0;
            int totalItemsNumber = 0;
            List<OrderViewModelForUser> Orders = await GetAll().Where(i => i.OrderStatus == OrderStatus.Pending && i.UserId == userId)
                .Select(OrderExtansions.ToOrderDetailsViewModelForUserExpression())
                .Skip(itemsToSkip).Take(pageSize).ToListAsync();
            if (Orders.Count > 0)
            {
                int totalItems = await GetAll().Where(i => i.OrderStatus == OrderStatus.Pending && i.UserId == userId).CountAsync();
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
        public async Task<PaginationViewModel<OrderViewModelForUser>> GetAllConfirmedOrdersForUserAsync(int pageNumber, int pageSize, string userId)
        {
            int itemsToSkip = pageNumber * pageSize;
            int totalPageNumbers = 0;
            int totalItemsNumber = 0;
            List<OrderViewModelForUser> Orders = await GetAll().Where(i => i.OrderStatus == OrderStatus.Confirmed && i.UserId == userId)
                .Select(OrderExtansions.ToOrderDetailsViewModelForUserExpression())
                .Skip(itemsToSkip).Take(pageSize).ToListAsync();
            if (Orders.Count > 0)
            {
                int totalItems = await GetAll().Where(i => i.OrderStatus == OrderStatus.Confirmed && i.UserId == userId).CountAsync();
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
        public async Task<UserPortfolioViewModel> GetUserPortfolioAsync(string userId)
        {
            UserPortfolioViewModel userPortfolio = new();
            List<Order> userOrders = await GetAll().Where(i => i.UserId == userId && i.OrderStatus == OrderStatus.Confirmed).Select(i => new Order()
            {
                SharePrice = i.SharePrice,
                NumberOfShares = i.NumberOfShares,
                ConfirmationDate = i.ConfirmationDate,
                Property = new Property()
                {
                  Status = i.Property.Status,
                  AnnualPriceAppreciation = i.an
                },
            }).ToListAsync();
            if (userOrders.Count > 0)
            {
                foreach(Order order in  userOrders)
                {
                    userPortfolio.ProtfolioValue += (order.SharePrice * order.NumberOfShares);
                }
                userPortfolio.CurrentMonth = DateTime.Now.Month;
                userPortfolio.NumberOfProperties = userOrders.Count;
                userPortfolio.
            }
        }
    }
}
