using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models
{
    public class MatrixParent
    {
        public List<string[]> Matrix {  get; set; }

        public MatrixParent()
        {
            Matrix = new List<string[]>();
        }
    }
}
