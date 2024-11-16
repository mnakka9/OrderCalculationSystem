using OCS.Domain.Entities;

namespace OCS.Infrastructure.DataAccess.Repositories.Clients;
public class ClientRepository(TaxSystemContext context) : Repository<Client>(context), IClientRepository;
