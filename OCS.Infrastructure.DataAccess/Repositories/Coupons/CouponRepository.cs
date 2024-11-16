using OCS.Domain.Entities;

namespace OCS.Infrastructure.DataAccess.Repositories.Coupons;
public class CouponRepository(TaxSystemContext context) : Repository<Coupon>(context), ICouponRepository
{
}