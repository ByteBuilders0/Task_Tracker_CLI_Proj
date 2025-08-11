using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Tracker_CLI_Proj.Models
{
    public class TaskEntities
    {
        public int Task_ID { get; set; }
        public string Task_Name { get; set; } = string.Empty;
        public string Task_Description { get; set; } = string.Empty;
        public TaskObservationStatus Task_Status { get; set; } = TaskObservationStatus.todo;
        public DateTime Task_Created_At { get; set; } = DateTime.UtcNow;
        public DateTime Task_Updated_At { get; set; }
    }

    public enum TaskObservationStatus
    {
        todo,
        in_progress,
        done
    }
}
