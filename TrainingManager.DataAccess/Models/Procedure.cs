using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class Procedure
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal RevisionNumber { get; set; }
    public bool IsActive { get; set; }
    public string HistoryText { get; set; }
}
