﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public interface IRolesRequiredTrainingDAO
{
    Task<IEnumerable<RolesRequiredTraining>> GetAllRolesRequiredTraining();

    Task UpdateTrainingRequirementAsync(UpdateTrainingRequirement dto);
}
