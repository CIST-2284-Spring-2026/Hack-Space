
namespace Server.Data.DALS{
public class EvaluatorsDAL: IEvaluatorsDAL{
        private ApplicationDbContext context;

        public EvaluatorsDAL (ApplicationDbContext context)
{
this.context = context;
}

public async Task<List<Evaluator>?> GetAllAsync()
{
return await this.context.Evaluators. Include(e => e. Badges).ToListAsync();
}


public async Task<Evaluator?> GetByIdAsync(Guid id)
{ return await this.context.Evaluators.Where(e =>e.Id
}

public async Task AddAsync (Evaluator evaluator)
{}
}

    public interface IEvaluatorsDAL
    {
    }
}