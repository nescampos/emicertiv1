using EmiCerti.Data;

namespace EmiCerti.Models.ProjectModel
{
    public class CreateProjectViewModel
    {
        private ApplicationDbContext db;
        public CreateProjectFormModel Form { get; set; }

        public CreateProjectViewModel(ApplicationDbContext db)
        {
            Form = new CreateProjectFormModel();
            this.db = db;
        }

        

        
    }
}
