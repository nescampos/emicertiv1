using EmiCerti.Data;

namespace EmiCerti.Models.ProjectModel
{
    public class BuyTokenViewModel
    {
        private ApplicationDbContext db;
        public BuyTokenFormModel Form { get; set; }
        public Project Project { get; set; }

        public BuyTokenViewModel(ApplicationDbContext db, int id)
        {
            Form = new BuyTokenFormModel();
            Form.Id = id;
            this.db = db;
            Project = db.Projects.SingleOrDefault(x => x.Id == id);
        }
    }
}
