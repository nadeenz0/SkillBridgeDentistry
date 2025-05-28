using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public enum CaseStatus
    {
        Pending = 0,
        AwaitingConsultantReply = 1,
        TreatmentSent = 2,
        AwaitingRating = 3,
        Rated = 4

    }
}

