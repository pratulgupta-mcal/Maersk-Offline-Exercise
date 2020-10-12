using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public interface ISortJobProcessor
    {
        Task<SortJob> Process(SortJob job);

        void Enqueue(SortJob job);

        Task ProcessQueue();

        List<SortJob> GetAllJobs();

        SortJob GetJobById(Guid id);
    }
}