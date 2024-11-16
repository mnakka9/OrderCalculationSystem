using OCS.Domain.Entities;

namespace OCS.Infrastructure.DataAccess.Repositories.Promotions;

public class PromotionRepository(TaxSystemContext context) : Repository<Promotion>(context), IPromotionRepository
{
}
