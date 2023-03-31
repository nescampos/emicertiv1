using EmiCerti.Data;

namespace EmiCerti.Models.ProjectModel
{
    public class ViewProjectViewModel
    {
        public Project Project { get; set; }
        public BuyTokenFormModel Form { get; set; }
        public IEnumerable<Transaction> Transactions { get; internal set; }

        public ViewProjectViewModel(ApplicationDbContext db, int id)
        {
            Project= db.Projects.SingleOrDefault(p => p.Id == id);
            Form = new BuyTokenFormModel();
            Form.Id = id;
        }
    }
}
