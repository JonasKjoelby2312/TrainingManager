using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class TicketDAO : BaseDAO, ITicketDAO
{
    private readonly string GET_ALL_TICKETS = "";

    public TicketDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Ticket entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var tickets = await connection.QueryAsync<Ticket>(GET_ALL_TICKETS);
            connection.Close();
            return tickets;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all tickets, message was: {ex.Message}", ex);
        }
    }

    public Task<Ticket> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Ticket entity)
    {
        throw new NotImplementedException();
    }
}
