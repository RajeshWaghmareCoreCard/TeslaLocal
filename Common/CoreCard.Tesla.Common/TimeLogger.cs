using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace CoreCard.Tesla.Utilities
{
    public class TimeLogger : IDisposable
    {
        Dictionary<string, Stopwatch> _watcherList;
        ILogger<TimeLogger> _logger;
        public TimeLogger(ILogger<TimeLogger> logger)
        {
            _watcherList = new Dictionary<string, Stopwatch>();
            _logger = logger;
        }

        public void Start(string watcherName)
        {
            Stopwatch stopWatch;
            if (_watcherList.TryGetValue(watcherName, out stopWatch))
            {
                stopWatch.Restart();
            }
            else
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
                _watcherList.Add(watcherName, stopWatch);
            }
        }

        public bool Restart(string watcherName)
        {
            Stopwatch stopWatch;
            if (_watcherList.TryGetValue(watcherName, out stopWatch))
            {
                stopWatch.Restart();
                return true;
            }
            else
            {
                throw new Exception($"Watcher '{watcherName}' doesn't exist.");
            }
        }

        public long Stop(string watcherName)
        {
            Stopwatch stopWatch;
            if (_watcherList.TryGetValue(watcherName, out stopWatch))
            {
                stopWatch.Stop();
                return stopWatch.ElapsedMilliseconds;
            }
            else
            {
                throw new Exception($"Watcher '{watcherName}' doesn't exist.");
            }
        }

        public void StopAll()
        {
            foreach (KeyValuePair<string, Stopwatch> watcher in _watcherList)
            {
                watcher.Value.Stop();
            }
        }

        public void StopAndLogAll()
        {
            foreach (KeyValuePair<string, Stopwatch> watcher in _watcherList)
            {
                if (watcher.Value.IsRunning)
                    watcher.Value.Stop();
                _logger.LogInformation($"{watcher.Key} takes '{watcher.Value.ElapsedMilliseconds}' ms.");
            }
        }


        public long StopAndLog(string watcherName)
        {
            Stopwatch stopWatch;
            if (_watcherList.TryGetValue(watcherName, out stopWatch))
            {
                if (stopWatch.IsRunning)
                    stopWatch.Stop();
                _logger.LogInformation($"{watcherName} takes '{stopWatch.ElapsedMilliseconds}' ms.");
                return stopWatch.ElapsedMilliseconds;
            }
            else
            {
                throw new Exception($"Watcher '{watcherName}' doesn't exist.");
            }
        }

        public void Dispose()
        {
            if (_watcherList != null)
            {
                _watcherList.Clear();
                _watcherList = null;
            }
        }
    }
}
