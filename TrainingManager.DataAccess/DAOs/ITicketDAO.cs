using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public interface ITicketDAO
{
    Task<int> CreateAsync(Ticket entity);
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket> GetByIdAsync(int id);
    Task<int> UpdateAsync(Ticket entity);
}
