using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.HR
{
   public class DegreeTypes
    {
        public DegreeTypes()
        {

        }

        public List<string> GetDegreesList()
        {
            List<string> degreeType = new List<string>();
            degreeType.Add("Nill");
            degreeType.Add("Primary");
            degreeType.Add("Middle");
            degreeType.Add("Matriculation/O-Level");
            degreeType.Add("Intermediate/A-Level");
            degreeType.Add("Bachlors(2 Years)");
            degreeType.Add("Bachlors(4 Years)");
            degreeType.Add("Masters(2 Years)");
            degreeType.Add("MS/Mphil");
            degreeType.Add("PhD");
            degreeType.Add("Course");
            degreeType.Add("Certification");

            return degreeType;
        }
    }
}
