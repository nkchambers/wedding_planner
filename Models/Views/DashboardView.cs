using System.Collections.Generic;

namespace wedding_planner.Models
{
    public class DashboardView
    {
        public int LoggedUserId { get; set; } 
        public string UserFirstName { get; set; }
        public List<Wedding> AllWeddings { get; set; }
    }
}