using EmiCerti.Data;

namespace EmiCerti.Models.ProjectModel
{
    public class IndexProjectViewModel
    {
        public IEnumerable<Project> AvailableProjects { get; set; }
        private ApplicationDbContext _db { get; set; }

        public IndexProjectViewModel(ApplicationDbContext db)
        {
            _db = db;
            AvailableProjects = db.Projects.Where(x => x.Enable)
                .OrderByDescending(x => x.Expiration);
        }

        public long? GetTokenBought(int id)
        {
            var transactions = _db.Transactions.Where(x => x.ProjectId == id)
                .Sum(y => y.Quantity);
            return transactions;
        }

        public int GetTokenBoughtByQty(int id)
        {
            var transactions = _db.Transactions.Count(x => x.ProjectId == id);
            return transactions;
        }
    }
}
