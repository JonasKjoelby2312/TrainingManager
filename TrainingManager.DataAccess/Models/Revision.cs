using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class Revision
{
    public int Id { get; set; }
    public int RevisionNumber { get; set; }
    public bool IsActive { get; set; }
    public string HistoryText { get; set; }

    public Revision()
    {
        
    }
}
