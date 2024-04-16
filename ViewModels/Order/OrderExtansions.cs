using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class OrderExtansions
    {
        public static Expression<Func<Order, OrderViewModelForAdmin>> ToOrderDetailsViewModelForAdminExpression()
        {
            return i => new OrderViewModelForAdmin()
            {
                OrderFinancialDetails = i.ToOrderFinancialDetailsViewModel(),
                Property = i.Property.ToPropertyViewModelInOrderView(),
                UserInformation = i.User.ToUserInformationViewModel(),
            };
        }
        public static Expression<Func<Order, OrderViewModelForUser>> ToOrderDetailsViewModelForUserExpression()
        {
            return i => new OrderViewModelForUser()
            {
                OrderFinancialDetails = i.ToOrderFinancialDetailsViewModel(),
                Property = i.Property.ToPropertyViewModelInOrderView(),
            };
        }
        public static OrderFinancialDetailsViewModel ToOrderFinancialDetailsViewModel(this Order order)
        {
            return new OrderFinancialDetailsViewModel()
            {
                OrderId = order.Id,
                SharePrice = order.SharePrice,
                NumberOfShares = order.NumberOfShares,
                AnnualPriceAppreciation = order.AnnualPriceAppreciation,
                AnnualRentalYield = order.AnnualRentalYield,
                ConfirmationDate = order.ConfirmationDate,
                DeliveryInstallment = order.DeliveryInstallment,
                DownPayment = order.DownPayment,
                MaintenaceInstallment = order.MaintenaceInstallment,
                MaintenanceCost = order.MaintenanceCost,
                MonthlyInstallment = order.MonthlyInstallment,
                NumberOfYears = order.NumberOfYears,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                TransactionFees = order.TransactionFees,
            };
        }
        public static PropertyViewModelInListView ToPropertyViewModelInOrderView(this Property property)
        {
            return new PropertyViewModelInListView()
            {
                Address = property.Location,
                ProjectedRentalYield = property.AnnualRentalYield,
                ProjectedAnnualReturn = property.AnnualRentalYield + property.AnnualPriceAppreciation,
                City = property.City.NameEn,
                ImageUrl = property.PropertyImages.Select(image => image.ImageUrl).FirstOrDefault(),
                PropertyId = property.Id,
                AvailableTokens = property.AvailableShares,
                TokenPrice = property.SharePrice,
                IsDeleted = null
            };
        }
    }
}
