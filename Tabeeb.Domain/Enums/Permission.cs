using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Enums
{
    public  enum  Permission:byte
    {
        AllPermission=1,
        ViewCountry,
        AddCountry,
        UpdateCountry,
        DeleteCountry,
        ViewCity,
        AddCity,
        UpdateCity,
        DeleteCity,
        ViewClinic,
        AddClinic,
        UpdateClinic,
        DeleteClinic,
        ViewClinicDoctors,
        UpdateClinicDoctors,
        ViewClinicImages,
        UpdateClinicImages,
        ViewEmployee,
        AddEmployee,
        UpdateEmployee,
        DeleteEmployee,
        ViewCustomer,
        AddCustomer,
        UpdateCustomer,
        DeleteCustomer,
        AddClinicDoctors,
        DeleteClinicDoctors,
        ViewDoctorWorkTime,
        UpdateDoctorWorkTime,
        ViewOffer,
        AddOffer,
        UpdateOffer,
        DeleteOffer,
        ViewRolePermissions,
        UpdateRolePermissions,
        ViewSpecialization,
        AddSpecialization,
        UpdateSpecialization,
        DeleteSpecialization,
        ViewSubscriptionPackage,
        AddSubscriptionPackage,
        UpdateSubscriptionPackage,
        DeleteSubscriptionPackage,
        AddSubscription,
        CancelSubscription,
        RenewSubscription,
        ShowSubscriptionInvoice,
        ShowSubscriptionHistory
      






    }
}
