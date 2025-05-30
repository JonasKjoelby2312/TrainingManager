﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public interface IProcedureDAO
{
    Task<int> CreateAsync(Procedure entity);
    Task<IEnumerable<Procedure>> GetAllProceduresWithRevisionsAsync();
    Task<IEnumerable<Procedure>> GetRevisionsForProcedureAsync(string procedureName);
    Task<Procedure> GetByIdAsync(int id);
    Task<int> UpdateAsync(Procedure entity);

}
