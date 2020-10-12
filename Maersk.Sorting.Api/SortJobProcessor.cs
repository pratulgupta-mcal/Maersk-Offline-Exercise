using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public class SortJobProcessor : ISortJobProcessor
    {
        private readonly ILogger<SortJobProcessor> _logger;
        private List<SortJob> srtJobs = new List<SortJob>();

        public SortJobProcessor(ILogger<SortJobProcessor> logger)
        {
            _logger = logger;
        }

        public async Task<SortJob> Process(SortJob job)
        {
            _logger.LogInformation("Processing job with ID '{JobId}'.", job.Id);

            var stopwatch = Stopwatch.StartNew();

            var output = job.Input.OrderBy(n => n).ToArray();
            await Task.Delay(5000); // NOTE: This is just to simulate a more expensive operation

            var duration = stopwatch.Elapsed;

            _logger.LogInformation("Completed processing job with ID '{JobId}'. Duration: '{Duration}'.", job.Id, duration);

            return new SortJob(
                id: job.Id,
                status: SortJobStatus.Completed,
                duration: duration,
                input: job.Input,
                output: output);
        }

        public async void Enqueue(SortJob job)
        {
            _logger.LogInformation("Enqueue Job in List '{JobId}'.", job.Id);
            srtJobs.Add(job);

            await Task.Delay(5000); //wait for 5 sec to process the queued job

            await ProcessQueue();
        }

        public async Task ProcessQueue()
        {
            //_logger.LogInformation("Enqueue Job in List '{JobId}'.", job.Id);
            foreach(SortJob sjob in srtJobs)
            {
                if(sjob.Status == SortJobStatus.Pending)
                {
                    var output = await Process(sjob);
                    srtJobs.Remove(sjob);
                    srtJobs.Add(output);
                }
            }
        }

        public List<SortJob> GetAllJobs()
        {
            return srtJobs;
        }

        public SortJob GetJobById(Guid id)
        {
            return srtJobs.Where(s => s.Id == id).FirstOrDefault();
        }
    }
}
