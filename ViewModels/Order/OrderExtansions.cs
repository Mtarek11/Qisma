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
                OrderFinancialDetails = new OrderFinancialDetailsViewModel()
                {
                    OrderId = i.Id,
                    SharePrice = i.SharePrice,
                    NumberOfShares = i.NumberOfShares,
                    ConfirmationDate = i.ConfirmationDate,
                    DeliveryInstallment = i.DeliveryInstallment,
                    DownPayment = i.DownPayment,
                    MaintenaceInstallment = i.MaintenaceInstallment,
                    MonthlyInstallment = i.MonthlyInstallment,
                    NumberOfYears = i.NumberOfYears,
                    OrderDate = i.OrderDate,
                    OrderNumber = i.OrderNumber,
                    TransactionFees = i.TransactionFees,
                },
                Property = new PropertyViewModelInListView()
                {
                    Address = i.Property.Title,
                    ProjectedRentalYield = Math.Round(i.Property.PropertyRentalYields.Where(y => y.To == null).Select(y => y.RentalYield).FirstOrDefault() * 100, 2),
                    ProjectedAnnualReturn = Math.Round((i.Property.PropertyRentalYields.Where(y => y.To == null).Select(y => y.RentalYield).FirstOrDefault() + i.Property.AnnualPriceAppreciation) * 100, 2),
                    City = i.Property.City.NameEn,
                    ImageUrl = i.Property.PropertyImages.Select(image => image.ImageUrl).FirstOrDefault(),
                    PropertyId = i.Property.Id,
                    AvailableTokens = i.Property.AvailableShares,
                    TokenPrice = i.Property.SharePrice,
                    IsDeleted = i.Property.IsDeleted
                },
                UserInformation = new UserFullInformationViewModel()
                {
                    Address = i.User.Address,
                    CompanyName = i.User.CompanyName,
                    DateOfBirth = i.User.DateOfBirth,
                    Email = i.User.Email,
                    FirstName = i.User.FirstName,
                    IdentityImageUrl = i.User.IdentityImageUrl,
                    IdentityNumber = i.User.IdentityNumber,
                    InvestorTypeId = i.User.InvestoreType,
                    InvestorType = i.User.InvestoreType == InvestorType.Institutional ? "Institutional" : "Retail",    
                    LastName = i.User.LastName,
                    MiddleName = i.User.MiddleName,
                    Occupation = i.User.Occupation,
                    PhoneNumber = i.User.PhoneNumber,
                    ReciveEmails = i.User.ReciveEmails,
                    UserId = i.User.Id,
                },
                OrderStatus = i.OrderStatus,
                OrderStatusName = i.OrderStatus == OrderStatus.Pending ? "Pending" : "Confirmed",
                OrderPdfUrl = $"{i.OrderNumber}.pdf"
            };
        }
        public static Expression<Func<Order, OrderViewModelForUser>> ToOrderDetailsViewModelForUserExpression()
        {
            return i => new OrderViewModelForUser()
            {
                OrderFinancialDetails = new OrderFinancialDetailsViewModel()
                {
                    OrderId = i.Id,
                    SharePrice = i.SharePrice,
                    NumberOfShares = i.NumberOfShares,
                    ConfirmationDate = i.ConfirmationDate,
                    DeliveryInstallment = i.DeliveryInstallment,
                    DownPayment = i.DownPayment,
                    MaintenaceInstallment = i.MaintenaceInstallment,
                    MonthlyInstallment = i.MonthlyInstallment,
                    NumberOfYears = i.NumberOfYears,
                    OrderDate = i.OrderDate,
                    OrderNumber = i.OrderNumber,
                    TransactionFees = i.TransactionFees,
                },
                Property = new PropertyViewModelInListView()
                {
                    Address = i.Property.Title,
                    ProjectedRentalYield = Math.Round(i.Property.PropertyRentalYields.Where(y => y.To == null).Select(y => y.RentalYield).FirstOrDefault() * 100, 2),
                    ProjectedAnnualReturn = Math.Round((i.Property.PropertyRentalYields.Where(y => y.To == null).Select(y => y.RentalYield).FirstOrDefault() + i.Property.AnnualPriceAppreciation) * 100, 2),
                    City = i.Property.City.NameEn,
                    ImageUrl = i.Property.PropertyImages.Select(image => image.ImageUrl).FirstOrDefault(),
                    PropertyId = i.Property.Id,
                    AvailableTokens = i.Property.AvailableShares,
                    TokenPrice = i.Property.SharePrice,
                    IsDeleted = null
                },
                OrderStatus = i.OrderStatus,
                OrderStatusName = i.OrderStatus == OrderStatus.Pending ? "Pending" : "Confirmed",
                OrderPdfUrl = $"{i.OrderNumber}.pdf"
            };
        }
    }
}
