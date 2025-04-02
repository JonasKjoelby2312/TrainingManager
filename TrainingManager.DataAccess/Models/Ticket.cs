using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class Ticket
{
    public int Id { get; set; }
    public DateTime SubmissionDate { get; set; }
    public Employee Employee { get; set; }
    public Procedure Procedure { get; set; }
    public string TrainingType { get; set; }
}
